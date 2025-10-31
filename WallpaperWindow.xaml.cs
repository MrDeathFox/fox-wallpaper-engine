using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace FoxWallpaperEngine
{
    public partial class WallpaperWindow : Window
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        public WallpaperWindow()
        {
            InitializeComponent();
            Loaded += WallpaperWindow_Loaded;
        }

        private void WallpaperWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr progman = FindWindow("Progman", null);
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            SetParent(hwnd, progman);

            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            Topmost = false;
            WindowState = WindowState.Maximized;
        }
    }
}
