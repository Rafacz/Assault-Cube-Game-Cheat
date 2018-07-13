using System.Collections.Generic;

namespace AC_Hack.Properties
{
    class Aimbot
    {
        public List<Enemy> enemy { get; private set; }
        public Calculator calc { get; set; }
        public Enemy bot { get; private set; }
        public Player player { get; private set; }
        public VAMemory vam { get; set; }

                public Aimbot(List<Enemy> enemy, Calculator calc, Enemy bot, Player player, VAMemory vam)
        {
            this.enemy = enemy;
            this.calc = calc;
            this.bot = bot;
            this.player = player;
            this.vam = vam;
        }

        public void Run(int NumberOfPlayers)
        {
            for (int i = 0; i < NumberOfPlayers - 1; i++)
            {
                player.GetValuesFromPointers(vam);
                bot.GetBotData(vam, i);
                enemy.Add(new Enemy(bot.Health, bot.XPos, bot.YPos, bot.ZPos, bot.Visible, bot.NickName));
            }
            AimTarget(GetClosestEnemy());
            enemy.Clear();
        }

        private void AimTarget(int x)
        {
            calc.CalculateAngles(x);
            if (enemy[x].Health > 0 && enemy[x].Visible > -1)
            {
                vam.WriteFloat(player.MOUSE_X_ADDRESS, calc.PitchX);
                vam.WriteFloat(player.MOUSE_Y_ADDRESS, calc.YawY);
            }
        }

        private int GetClosestEnemy()
        {
            float maxDistance = 100;
            int iterator = 0;
            int iEnemy = 0;
            foreach (var bot in enemy)
            {
                calc.GetDistance(iterator);
                if (calc.Distance < maxDistance && bot.Health > 0 && bot.Visible > -1)
                {
                    maxDistance = calc.Distance;
                    iEnemy = iterator;
                }
                iterator++;
            }
            return iEnemy;
        }

    }
}