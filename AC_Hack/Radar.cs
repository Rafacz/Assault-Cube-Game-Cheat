using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AC_Hack
{
    class Radar
    {
        public Player player { get; private set; }
        public List<Enemy> enemies { get; private set; }
        public Enemy bot { get; private set; }
        public VAMemory vam { get; private set; }
        public Calculator calc { get; private set; }

        public Radar(Player player, List<Enemy> enemies, Enemy bot, VAMemory vam, Calculator calc)
        {
            this.player = player;
            this.enemies = enemies;
            this.bot = bot;
            this.vam = vam;
            this.calc = calc;
        }

        public void DrawRadar(Graphics g, int numberOfPlayers)
        {
            Brush radarBackground = Brushes.DarkGreen;
            Brush FOV = Brushes.Lime;
            Brush enemyPointer = Brushes.Red;
            Brush playerPointer = Brushes.Blue;

            g.FillRectangle(radarBackground, 1080, 30, 190, 190);
            g.FillPie(FOV, 1080, 30, 190, 190, 225, 91);

            for (int i = 0; i < numberOfPlayers; i++)
            {
                player.GetValuesFromPointers(vam);
                bot.GetBotData(vam, i);
                enemies.Add(new Enemy(bot.Health, bot.XPos, bot.YPos, bot.ZPos, bot.Visible, bot.NickName));

                float x = Math.Abs(enemies[i].XPos) - player.XPos;
                float y = Math.Abs(enemies[i].YPos) - player.YPos;

                float yaw = (float)((360 - player.RotationAngle) * (3.14 * 2 / 360));

                float x1 = (float)(Math.Cos(yaw) * x - Math.Sin(yaw) * y);
                float y1 = (float)(Math.Sin(yaw) * x + Math.Cos(yaw) * y);

                if (i != player.NumberOfPlayers - 1 && bot.Health > - 1)
                {
                    g.FillEllipse(enemyPointer, 1170 + x1, 122 + y1, 8, 8);
                }
                
                Console.WriteLine(x1 + " " + y1);
            }
            g.FillEllipse(playerPointer, 1170, 120, 8, 8);
            enemies.Clear();
        }
    }
}
