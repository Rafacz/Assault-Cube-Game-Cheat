using AC_Hack.Properties;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace AC_Hack
{
    class Program
    {
        static void Main(string[] args)
        {
            string process = "ac_client";

            #region instance

            Overlay overlay = new Overlay(); //INVISIBLE OVERLAY WINDOW
            VAMemory vam = new VAMemory(process); //DLL READ_PROCESS_MEMORY
            Player player = new Player();
            Enemy bot = new Enemy();

            List<Enemy> enemy = new List<Enemy>(); //LIST OF ENEMIES
            Calculator calc = new Calculator(enemy, player, vam, overlay); //MATH
            Graphics g = overlay.CreateGraphics();

            Aimbot aimbot = new Aimbot(enemy, calc, bot, player, vam);
            ESP esp = new ESP(enemy, bot, player, overlay, calc, vam);
            Radar radar = new Radar(player, enemy, bot, vam, calc);

            #endregion

            player.GetNumberOfPlayers(vam);
            overlay.Show();

            while (true)
            {
                overlay.Refresh();
                esp.Run(g, overlay, player.NumberOfPlayers);
                //aimbot.Run(player.NumberOfPlayers);
                radar.DrawRadar(g, player.NumberOfPlayers);
                Thread.Sleep(12);
            }
        }
    }
}