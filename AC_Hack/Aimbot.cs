using System;
using System.Collections.Generic;

namespace AC_Hack.Properties
{
    class Aimbot
    {
        public float Distance { get; private set; }
        public float Hyp { get; private set; }
        public float PitchX { get; private set; }
        public float YawY { get; private set; }
        public List<Enemy> enemy { get; private set; }
        public Enemy bot { get; private set; }
        public Player player { get; private set; }

        public Aimbot(List<Enemy> enemy, Enemy bot, Player player)
        {
            this.enemy = enemy;
            this.bot = bot;
            this.player = player;
        }

        public void Run(VAMemory vam, int NumberOfPlayers)
        {
            int closestEnemy;
            for (int i = 0; i < NumberOfPlayers - 1; i++)
            {
                player.GetValuesFromPointers(vam);
                bot.getBotData(vam, i);
                enemy.Add(new Enemy(bot.Health, bot.XPos, bot.YPos, bot.ZPos, bot.Visible, bot.NickName));
            }
            closestEnemy = getClosestEnemy();
            AimTarget(vam, closestEnemy);
            enemy.Clear();
        }

        private void CalculateAngles(int x)
        {
            Hyp = (float)Math.Sqrt(((enemy[x].XPos - player.XPos) * (enemy[x].XPos - player.XPos)) +
                       ((enemy[x].YPos - player.YPos) * (enemy[x].YPos - player.YPos)) +
                       ((enemy[x].ZPos - player.ZPos) * (enemy[x].ZPos - player.ZPos)));

            YawY = (float)Math.Atan2(enemy[x].ZPos - player.ZPos, Hyp) * 180 / (float)Math.PI;

            PitchX = (float)Math.Atan2(enemy[x].YPos - player.YPos, enemy[x].XPos - player.XPos) / (float)Math.PI * 180 + 90;
        }

        private void AimTarget(VAMemory vam, int x)
        {
            CalculateAngles(x);
            if (enemy[x].Health > 0 && enemy[x].Visible > -1)
            {
                vam.WriteFloat(player.MOUSE_X_ADDRESS, PitchX);
                vam.WriteFloat(player.MOUSE_Y_ADDRESS, YawY);
            }
        }

        private void getDistance(int i)
        {
            Distance = (float)Math.Sqrt(Math.Abs((enemy[i].XPos - player.XPos)) + Math.Abs((enemy[i].YPos - player.YPos)) + Math.Abs((enemy[i].ZPos - player.ZPos)));
        }

        private int getClosestEnemy()
        {
            float maxDistance = 100;
            int iterator = 0;
            int iEnemy = 0;
            foreach (var bot in enemy)
            {
                getDistance(iterator);
                if (Distance < maxDistance && bot.Health > 0 && bot.Visible > -1)
                {
                    maxDistance = Distance;
                    iEnemy = iterator;
                }
                iterator++;
            }
            return iEnemy;
        }
    }
}
