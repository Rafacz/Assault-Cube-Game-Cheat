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
        public float VectorZ { get; private set; }
        public float INVW { get; private set; }
        public List<Enemy> enemy { get; private set; }
        public Enemy bot { get; private set; }
        public Player player { get; private set; }
        public float Distance { get; private set; }
        public bool isDrawable { get; private set; }


        public ESP(List<Enemy> enemy, Enemy bot, Player player)
        {
            this.enemy = enemy;
            this.bot = bot;
            this.player = player;
        }

        public ESP()
        {

        }

        private void GetVectors(VAMemory vam, int i)
        {
            GetMatrixValues(vam);
            //3D TO 2D SCREEN "WORLD2SCREEN"
            VectorX = Matrice[0] * enemy[i].XPos + Matrice[4] * enemy[i].YPos + Matrice[8] * enemy[i].ZPos + Matrice[12];
            VectorY = Matrice[1] * enemy[i].XPos + Matrice[5] * enemy[i].YPos + Matrice[9] * enemy[i].ZPos + Matrice[13];

            VectorZ = Matrice[3] * enemy[i].XPos + Matrice[7] * enemy[i].YPos + Matrice[11] * enemy[i].ZPos + Matrice[15];

            if (VectorZ < 0.01f)
            {
                isDrawable = false;
            }
            else
            {
                isDrawable = true;
            }

            INVW = 1.0f / VectorZ;
            VectorX *= INVW;
            VectorY *= INVW;

            int width = 1280;
            int height = 968;

            float x = width / 2;
            float y = height / 2;

            x += 0.5f * VectorX * width + 0.5f;
            y -= 0.5f * VectorY * height + 0.5f;

            VectorX = x;  //rc.left;
            VectorY = y; //rc.top;
        }

        public void Run(Graphics g, Pen pen, Form overlay, VAMemory vam, int NumberOfPlayers)
        {
            Font font = new Font("Consolas", 9);
            Brush b = Brushes.White;
            pen.Color = Color.SkyBlue;

            for (int i = 0; i < NumberOfPlayers - 1; i++)
            {
                player.getValuesFromPointers(vam);
                bot.getBotData(vam, i);
                enemy.Add(new Enemy(bot.Health, bot.XPos, bot.YPos, bot.ZPos, bot.Visible));
                GetVectors(vam, i);
                getDistance(i);
                Console.WriteLine(Distance + " " + INVW);

                if (VectorX < 1280 && VectorY > 1 && GetEnemyHealth(i) > 0 && isDrawable)
                {
                    g.DrawRectangle(pen, VectorX - 80 / Distance, VectorY - 375 / Distance, 50 - (Distance * VectorZ / 30), 100 + (-Distance * VectorZ / 40));
                    g.DrawString("Player " + (i + 1), font, b, VectorX - 100 / Distance, VectorY - 22 - 400 / Distance);
                    g.DrawString(GetEnemyHealth(i) + " hp", font, b, VectorX - 100 / Distance, VectorY - 12 - 400 / Distance);
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

    }
}
