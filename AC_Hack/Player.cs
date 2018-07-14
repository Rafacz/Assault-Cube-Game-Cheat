using System;

namespace AC_Hack
{
    class Player
    {
        public IntPtr PLAYER_BASE_ADDRESS { get; private set; } = (IntPtr)0x00509B74;
        public IntPtr PLAYER_BASE_POINTER { get; private set; }
        public IntPtr MOUSE_X_ADDRESS { get { return PLAYER_BASE_POINTER + 0x40; } }
        public IntPtr MOUSE_Y_ADDRESS { get { return PLAYER_BASE_POINTER + 0x44; } }
        public IntPtr POSITION_X_ADDRESS { get { return PLAYER_BASE_POINTER + 0x34; } }
        public IntPtr POSITION_Y_ADDRESS { get { return PLAYER_BASE_POINTER + 0x38; } }
        public IntPtr POSITION_Z_ADDRESS { get { return PLAYER_BASE_POINTER + 0x3C; } }
        public IntPtr HEALTH_ADDRESS { get { return PLAYER_BASE_POINTER + 0xF8; } }
        public IntPtr NUMBER_OF_PLAYERS_ADDRESS { get; private set; } = (IntPtr)0x0050f500;
        public IntPtr ROTATION_ANGLE_ADDRESS { get; private set; } = (IntPtr)0x0050F4F4;

        public int NumberOfPlayers { get; private set; }
        public float MouseXPos { get; private set; }
        public float MouseYPos { get; private set; }
        public int Health { get; private set; }
        public float XPos { get; private set; }
        public float YPos { get; private set; }
        public float ZPos { get; private set; }
        public float RotationAngle { get; private set; }

        public void GetNumberOfPlayers(VAMemory vam)
        {
            NumberOfPlayers = vam.ReadInt32(NUMBER_OF_PLAYERS_ADDRESS);
        }

        private void GetRotationAngle(VAMemory vam)
        {
            int temp = vam.ReadInt32(ROTATION_ANGLE_ADDRESS);
            RotationAngle = vam.ReadFloat((IntPtr)temp + 0x40);
        }

        public void GetValuesFromPointers(VAMemory vam)
        {
            PLAYER_BASE_POINTER = (IntPtr)vam.ReadInt32(PLAYER_BASE_ADDRESS);
            MouseXPos = vam.ReadFloat(MOUSE_X_ADDRESS);
            MouseYPos = vam.ReadFloat(MOUSE_Y_ADDRESS);
            Health = vam.ReadInt32(HEALTH_ADDRESS);
            XPos = vam.ReadFloat(POSITION_X_ADDRESS);
            YPos = vam.ReadFloat(POSITION_Y_ADDRESS);
            ZPos = vam.ReadFloat(POSITION_Z_ADDRESS);
            GetRotationAngle(vam);
        }
    }
}
