using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AC_Hack
{
    class ESP
    {
        public List<Enemy> enemy { get; private set; }
        public Enemy bot { get; private set; }
        public Player player { get; private set; }
        public VAMemory vam { get; private set; }
        public Form overlay { get; private set; }
        public Calculator calc { get; private set; }
        public float Z_Diff { get; set; }

        public ESP()
        {

        }

        public ESP(List<Enemy> enemy, Enemy bot, Player player, Form overlay, Calculator calc, VAMemory vam)
        {
            this.enemy = enemy;
            this.bot = bot;
            this.player = player;
            this.overlay = overlay;
            this.calc = calc;
            this.vam = vam;
        }

        public void Run(Graphics g, Form overlay, int NumberOfPlayers)
        {
            for (int i = 0; i < NumberOfPlayers - 1; i++)
            {
                player.GetValuesFromPointers(vam);
                bot.GetBotData(vam, i);
                enemy.Add(new Enemy(bot.Health, bot.XPos, bot.YPos, bot.ZPos, bot.Visible, bot.NickName));
                calc.GetVectors(i);
                calc.GetDistance(i);
                calc.CalculateAngles(i);
                DrawBitmap(g, i);
            }
            enemy.Clear();
        }

        private void DrawBitmap(Graphics g, int i)
        {
            Font font = new Font("Consolas", 7);
            Font font2 = new Font("Consolas", 8);
            Brush b = Brushes.WhiteSmoke;
            Pen pen = new Pen(Color.CornflowerBlue, 2);
            Pen pen2 = new Pen(Color.LightGreen, 1);
            Pen pen3 = new Pen(Color.ForestGreen, 4);

            UpdateDrawingSettings(pen, pen2, pen3);

            if (calc.VectorX < 1300 && calc.VectorY > 1 && calc.GetEnemyHealth(i) > 0 && calc.IsDrawable)
            {
                g.DrawRectangle(pen, calc.VectorX - overlay.Height / calc.Hyp,
                                     calc.VectorY - overlay.Width / calc.Hyp * 2 + calc.Distance - (float)(Math.Abs(Z_Diff * 0.5)) + (float)(calc.Distance * 0.3),
                                     (overlay.Width / 2) / calc.Hyp * 3, (overlay.Width / 2) / calc.Hyp * 5);

                g.DrawLine(pen2, calc.VectorX, (float)(calc.VectorY + calc.Distance * 1.6),
                          (overlay.Width / 2), overlay.Height + 40);

                g.DrawString(Convert.ToInt32(calc.Distance) + "m", font, b,
                             calc.VectorX - overlay.Height / calc.Hyp + (overlay.Width / 2) / calc.Hyp * 3,
                             calc.VectorY - overlay.Width / calc.Hyp);

                g.DrawString(bot.NickName, font2, b,
                             calc.VectorX - overlay.Height / calc.Hyp,
                             calc.VectorY - overlay.Width / calc.Hyp * 2 + calc.Distance - (float)(Math.Abs(Z_Diff * 0.5)) + (float)(calc.Distance * 0.7) + (overlay.Width / 2) / calc.Hyp * 5 + 5);

                g.DrawLine(pen3, calc.VectorX - overlay.Height / calc.Hyp,
                                 calc.VectorY - overlay.Width / calc.Hyp * 2 + calc.Distance - (float)(Math.Abs(Z_Diff * 0.5)) + (float)(calc.Distance * 0.7) + (overlay.Width / 2) / calc.Hyp * 5,
                                 calc.VectorX - overlay.Height / calc.Hyp + (overlay.Width / 2) / calc.Hyp * 3 * calc.GetEnemyHealth(i) / 100, //HEALTH
                                 calc.VectorY - overlay.Width / calc.Hyp * 2 + calc.Distance - (float)(Math.Abs(Z_Diff * 0.5)) + (float)(calc.Distance * 0.7) + (overlay.Width / 2) / calc.Hyp * 5);

            }
        }

        private void UpdateDrawingSettings(Pen pen, Pen pen2, Pen pen3)
        {
            if (calc.Distance < 5)
            {
                pen.Width = 3;
                pen.Color = Color.Red;
                pen2.Color = Color.Red;
                pen2.Width = 3;
                pen3.Width = 8;
            }
            else if (calc.Distance < 7 && calc.Distance > 5)
            {
                bot.NickName = bot.NickName.Remove(bot.NickName.Length - 4);
                pen.Width = 2;
                pen2.Color = Color.Orange;
                pen2.Width = 2;
                pen3.Width = 6;
            }
            else if (calc.Distance < 9 && calc.Distance > 7)
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
