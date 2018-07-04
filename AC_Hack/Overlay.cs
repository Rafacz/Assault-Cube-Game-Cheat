using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AC_Hack
{
    public partial class Overlay : Form
    {
        RECT rect;
        public const string WINDOW_NAME = "AssaultCube";
        IntPtr windowHandle = FindWindow(null, WINDOW_NAME);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        public struct RECT
        {
            public int left, top, right, bottom;
        }

        public Overlay()
        {
            InitializeComponent();
        }

        private void Overlay_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            this.BackColor = Color.Black;
            this.TransparencyKey = Color.Black;
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;

            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);

            GetWindowRect(windowHandle, out rect);
            this.Size = new Size(rect.right - rect.left, rect.bottom - rect.top);
            this.Top = rect.top;
            this.Left = rect.left;
        }
    }
}
