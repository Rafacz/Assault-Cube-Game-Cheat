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

            VAMemory vam = new VAMemory(process); //DLL READ_PROCESS_MEMORY
            Player player = new Player();
            Enemy bot = new Enemy();
            List<Enemy> enemy = new List<Enemy>();
            Aimbot aimbot = new Aimbot(enemy, bot, player);
            Overlay overlay = new Overlay();
            ESP esp = new ESP(enemy, bot, player, overlay);
            Graphics g = overlay.CreateGraphics();

            #endregion

            overlay.Show();

            while (true)
            {
                overlay.Refresh();
                player.GetNumberOfPlayers(vam);
                esp.Run(g, overlay, vam, player.NumberOfPlayers);
                aimbot.Run(vam, player.NumberOfPlayers);
                Thread.Sleep(13);
            }
        }  
    }
}
