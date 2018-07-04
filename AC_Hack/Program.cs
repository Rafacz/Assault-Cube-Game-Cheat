using AC_Hack.Properties;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AC_Hack
{
    class Program
    {
        static void Main(string[] args)
        {
            string process = "ac_client";

            VAMemory vam = new VAMemory(process);
            Player player = new Player();
            Enemy bot = new Enemy();
            List<Enemy> enemy = new List<Enemy>();
            Aimbot aimbot = new Aimbot(enemy, bot, player);
            Overlay overlay = new Overlay();

            ESP esp = new ESP(enemy, bot, player);

            Pen pen = new Pen(Color.SkyBlue, 1);
            Graphics g = overlay.CreateGraphics();


            overlay.Show();


            while (true)
            {
                //aimbot.Run(vam, 2);
                overlay.Refresh();
                esp.Run(g, pen, overlay, vam, 2);

                Thread.Sleep(7);
            }


        }  
    }
}
