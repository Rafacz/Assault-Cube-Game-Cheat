using AC_Hack.Properties;
using System.Collections.Generic;
using System.Threading;

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

            while (true)
            {
                aimbot.Run(vam, 8);
                Thread.Sleep(1);
            }
        }  
    }
}
