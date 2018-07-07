using AC_Hack.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AC_Hack
{
    class ESP
    {

        public IntPtr MATRIX_BASE_ADDRESS { get; private set; } = (IntPtr)0x00501AE8;
        public float[] Matrice { get; private set; }
        public float VectorX { get; private set; }
        public float VectorY { get; private set; }
        public List<Enemy> enemy { get; private set; }
        public Enemy bot { get; private set; }
        public Aimbot aimbot { get; private set; }
        public Player player { get; private set; }
        public Form overlay { get; private set; }
        public float Distance { get; private set; }
        public bool isDrawable { get; private set; }
        public float Hyp { get; private set; }
        public float Z_Diff { get; set; }

        public ESP(List<Enemy> enemy, Enemy bot, Player player, Form overlay)
        {
            this.enemy = enemy;
            this.bot = bot;
            this.player = player;
            this.aimbot = aimbot;
            this.overlay = overlay;
        }

        public ESP()
        {

        }

        private void GetVectors(VAMemory vam, int i)
        {
            GetMatrixValues(vam);
            VectorX = Matrice[0] * enemy[i].XPos + Matrice[4] * enemy[i].YPos + Matrice[8] * enemy[i].ZPos + Matrice[12];
            VectorY = Matrice[1] * enemy[i].XPos + Matrice[5] * enemy[i].YPos + Matrice[9] * enemy[i].ZPos + Matrice[13];

            float VectorZ = Matrice[3] * enemy[i].XPos + Matrice[7] * enemy[i].YPos + Matrice[11] * enemy[i].ZPos + Matrice[15];

            if (VectorZ < 0.01f)
            {
                isDrawable = false;
            }
            else
            {
                isDrawable = true;
            }

            float INVW = 1.0f / VectorZ;
            VectorX *= INVW;
            VectorY *= INVW;

            int width = 1280;
            int height = 768;

            float x = overlay.Width / 2;
            float y = overlay.Height / 2 - 12;

            x += 0.5f * VectorX * width + 0.5f;
            y -= 0.5f * VectorY * height + 0.5f;

            VectorX = x;  //rc.left;
            VectorY = y; //rc.top;
        }

        public void Run(Graphics g, Form overlay, VAMemory vam, int NumberOfPlayers)
        {
            Font font = new Font("Consolas", 7);
            Font font2 = new Font("Consolas", 8);
            Brush b = Brushes.WhiteSmoke;

            Pen pen = new Pen(Color.CornflowerBlue, 2);
            Pen pen2 = new Pen(Color.LightGreen, 1);
            Pen pen3 = new Pen(Color.ForestGreen, 4);

            for (int i = 0; i < NumberOfPlayers - 1; i++)
            {
                player.GetValuesFromPointers(vam);
                bot.getBotData(vam, i);
                enemy.Add(new Enemy(bot.Health, bot.XPos, bot.YPos, bot.ZPos, bot.Visible, bot.NickName));
                GetVectors(vam, i);
                getDistance(i);
                CalculateAngles();
                UpdateDrawSettings(pen, pen2, pen3);

                if (VectorX < 1300 && VectorY > 1 && GetEnemyHealth(i) > 0 && isDrawable)
                {
                    g.DrawRectangle(pen, VectorX - overlay.Height / Hyp, 
                                         VectorY - overlay.Width / Hyp * 2 + Distance - (float)(Math.Abs(Z_Diff * 0.5)) + (float)(Distance * 0.7), 
                                         (overlay.Width / 2) / Hyp * 3, (overlay.Width / 2) / Hyp * 5);

                    g.DrawLine(pen2, VectorX, (float)(VectorY + Distance * 1.6),
                              (overlay.Width / 2), overlay.Height + 40);

                    g.DrawString(Convert.ToInt32(Distance) + "m", font, b,
                                 VectorX - overlay.Height / Hyp + (overlay.Width / 2) / Hyp * 3, 
                                 VectorY - overlay.Width / Hyp);

                    g.DrawString(bot.NickName, font2, b,
                                 VectorX - overlay.Height / Hyp,
                                 VectorY - overlay.Width / Hyp * 2 + Distance - (float)(Math.Abs(Z_Diff * 0.5)) + (float)(Distance * 0.7) + (overlay.Width / 2) / Hyp * 5 + 5);

                    g.DrawLine(pen3, VectorX - overlay.Height / Hyp,
                                     VectorY - overlay.Width / Hyp * 2 + Distance - (float)(Math.Abs(Z_Diff * 0.5)) + (float)(Distance * 0.7) + (overlay.Width / 2) / Hyp * 5,
                                     VectorX - overlay.Height / Hyp + (overlay.Width / 2) / Hyp * 3 * GetEnemyHealth(i) / 100, //HEALTH
                                     VectorY - overlay.Width / Hyp * 2 + Distance - (float)(Math.Abs(Z_Diff * 0.5)) + (float)(Distance * 0.7) + (overlay.Width / 2) / Hyp * 5);

                }
            }
            enemy.Clear();
        }

        private void GetMatrixValues(VAMemory vam)
        {
            Matrice = new float[16];
            for (int i = 0; i < 16; i++)
            {
                IntPtr temp = (MATRIX_BASE_ADDRESS + (0x04 * i));
                Matrice[i] = vam.ReadFloat(temp);
            }
        }

        private void getDistance(int i)
        {
            Distance = (float)Math.Sqrt(Math.Abs((enemy[i].XPos - player.XPos)) + Math.Abs((enemy[i].YPos - player.YPos)) + Math.Abs((enemy[i].ZPos - player.ZPos)));
        }

        private int GetEnemyHealth(int i)
        {
            return enemy[i].Health;
        }

        private void CalculateAngles()
        {
            Hyp = (float)Math.Sqrt(((bot.XPos - player.XPos) * (bot.XPos - player.XPos)) +
                       ((bot.YPos - player.YPos) * (bot.YPos - player.YPos)) +
                       ((bot.ZPos - player.ZPos) * (bot.ZPos - player.ZPos)));

            Z_Diff = (float)Math.Atan2(bot.ZPos - player.ZPos, Hyp) * 180 / (float)Math.PI;
        }

        private void UpdateDrawSettings(Pen pen, Pen pen2, Pen pen3)
        {
            if (Distance < 5)
            {
                pen.Width = 3;
                pen2.Color = Color.Red;
                pen2.Width = 3;
                pen3.Width = 8;
            }
            else if (Distance < 7 && Distance > 5)
            {
                bot.NickName = bot.NickName.Remove(bot.NickName.Length - 4);
                pen.Width = 2;
                pen2.Color = Color.Orange;
                pen2.Width = 2;
                pen3.Width = 6;
            }
            else if (Distance < 9 && Distance > 7)
            {
                bot.NickName = bot.NickName.Remove(bot.NickName.Length - 8);
                pen.Width = 1;
                pen2.Color = Color.LightGreen;
                pen2.Width = 1;
                pen3.Width = 4;
            }
            else
            {
                bot.NickName = bot.NickName.Remove(bot.NickName.Length - 13);
                pen.Width = 1;
                pen2.Color = Color.LightGreen;
                pen2.Width = 1;
                pen3.Width = 4;
            }
        }
    }
}
