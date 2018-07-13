using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AC_Hack
{
    class Calculator
    {
        public List<Enemy> enemy { get; private set; }
        public Player player { get; private set; }
        public VAMemory vam { get; set; }
        public Form overlay { get; private set; }
        public IntPtr MATRIX_BASE_ADDRESS { get; private set; } = (IntPtr)0x00501AE8;
        public float Distance { get; private set; }
        public float Hyp { get; private set; }
        public float PitchX { get; private set; }
        public float YawY { get; private set; }
        public float[] Matrice { get; private set; }
        public float VectorX { get; private set; }
        public float VectorY { get; private set; }
        public bool IsDrawable { get; private set; }

        public Calculator(List<Enemy> enemy, Player player, VAMemory vam, Form overlay)
        {
            this.enemy = enemy;
            this.player = player;
            this.vam = vam;
            this.overlay = overlay;
        }

        public void GetDistance(int i)
        {
            Distance = (float)Math.Sqrt(Math.Abs((enemy[i].XPos - player.XPos)) + Math.Abs((enemy[i].YPos - player.YPos)) + Math.Abs((enemy[i].ZPos - player.ZPos)));
        }

        public void CalculateAngles(int x)
        {
            Hyp = (float)Math.Sqrt(((enemy[x].XPos - player.XPos) * (enemy[x].XPos - player.XPos)) +
                       ((enemy[x].YPos - player.YPos) * (enemy[x].YPos - player.YPos)) +
                       ((enemy[x].ZPos - player.ZPos) * (enemy[x].ZPos - player.ZPos)));

            YawY = (float)Math.Atan2(enemy[x].ZPos - player.ZPos, Hyp) * 180 / (float)Math.PI;

            PitchX = (float)Math.Atan2(enemy[x].YPos - player.YPos, enemy[x].XPos - player.XPos) / (float)Math.PI * 180 + 90;
        }

        public int GetEnemyHealth(int i)
        {
            return enemy[i].Health;
        }

        private void GetMatrixValues()
        {
            Matrice = new float[16];
            for (int i = 0; i < 16; i++)
            {
                IntPtr temp = (MATRIX_BASE_ADDRESS + (0x04 * i));
                Matrice[i] = vam.ReadFloat(temp);
            }
        }

        public void GetVectors(int i)
        {
            GetMatrixValues();
            VectorX = Matrice[0] * enemy[i].XPos + Matrice[4] * enemy[i].YPos + Matrice[8] * enemy[i].ZPos + Matrice[12];
            VectorY = Matrice[1] * enemy[i].XPos + Matrice[5] * enemy[i].YPos + Matrice[9] * enemy[i].ZPos + Matrice[13];

            float VectorZ = Matrice[3] * enemy[i].XPos + Matrice[7] * enemy[i].YPos + Matrice[11] * enemy[i].ZPos + Matrice[15];

            if (VectorZ < 0.01f)
            {
                IsDrawable = false;
            }
            else
            {
                IsDrawable = true;
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

    }
}
