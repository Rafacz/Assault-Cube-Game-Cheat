using System;

namespace AC_Hack
{
    class Enemy
    {
        public int BASE_POINTER { get; } = 0x0050F4F8;
        public IntPtr BASE_ADDRESS { get; private set; }
        public IntPtr PLAYER_ADDRESS { get; private set; }

        public IntPtr X_POSITION_ADDRESS { get;  } 
        public IntPtr Y_POSITION_ADDRESS { get; } 
        public IntPtr Z_POSITION_ADDRESS { get; }

        public int Health { get; private set; }
        public float XPos { get; private set; }
        public float YPos { get; private set; }
        public float ZPos { get; private set; }
        public float Visible { get; private set; }

        public Enemy()
        {

        }

        public Enemy(int health, float xPos, float yPos, float zPos, float visible)
        {
            Health = health;
            XPos = xPos;
            YPos = yPos;
            ZPos = zPos;
            Visible = visible;
        }

        private void setBaseAddress(VAMemory vam)
        {
            BASE_ADDRESS = (IntPtr)vam.ReadInt32((IntPtr)BASE_POINTER);
        }

        private void setPlayerAddress(VAMemory vam, int PlayerNumber)
        {
            PLAYER_ADDRESS = (IntPtr)vam.ReadInt32(BASE_ADDRESS + 0x04 + (PlayerNumber * 0x04));
        }

        public void getBotData(VAMemory vam, int PlayerNumber)
        {
            setBaseAddress(vam);
            setPlayerAddress(vam, PlayerNumber);

            Health = vam.ReadInt32(PLAYER_ADDRESS + 0xF8);
            XPos = vam.ReadFloat(PLAYER_ADDRESS + 0x34);
            YPos = vam.ReadFloat(PLAYER_ADDRESS + 0x38);
            ZPos = vam.ReadFloat(PLAYER_ADDRESS + 0x3C);
            Visible = vam.ReadFloat(PLAYER_ADDRESS + 0x408);
        }
    }
}
