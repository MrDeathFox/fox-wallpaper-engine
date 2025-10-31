using System;
using System.Drawing; // For Icon
using WinForms = System.Windows.Forms; // For NotifyIcon, ContextMenuStrip
using WpfApp = System.Windows; // For WPF Application
using System.Windows; // For Window, RoutedEventArgs
using Microsoft.Web.WebView2.Core; // For WebView2
using MessageBox = System.Windows.MessageBox;

namespace FoxWallpaperEngine
{
    public partial class MainWindow : Window
    {
        private WinForms.NotifyIcon trayIcon;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize WebView2
            InitializeAsync();

            // Example wallpapers
            WallpaperList.Items.Add("ColorWaves (HTML)");
            WallpaperList.Items.Add("OceanVideo (MP4)");
            WallpaperList.Items.Add("Visualizer (Audio)");

            // Hook up button events
            AddWallpaperButton.Click += AddWallpaperButton_Click;
            ApplyButton.Click += ApplyButton_Click;

            // Setup tray icon
            trayIcon = new WinForms.NotifyIcon
            {
                Text = "Fox Wallpaper Engine",
                Icon = SystemIcons.Application, // Replace with your custom fox icon
                Visible = true
            };

            // Create tray context menu
            var contextMenu = new WinForms.ContextMenuStrip();
            contextMenu.Items.Add("Open", null, (s, e) => ShowWindow());
            contextMenu.Items.Add("Exit", null, (s, e) => ExitApp());
            trayIcon.ContextMenuStrip = contextMenu;

            // Double-click tray icon to reopen window
            trayIcon.DoubleClick += (s, e) => ShowWindow();
        }

        // --- Initialize WebView2 ---
        private async void InitializeAsync()
        {
            await WallpaperPreview.EnsureCoreWebView2Async(null);

            // Load default wallpaper (replace with local HTML or online URL)
            WallpaperPreview.CoreWebView2.Navigate("https://example.com");
        }

        // --- Button Handlers ---
        private void AddWallpaperButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Feature coming soon: Add new wallpaper!", "Coming Soon",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (WallpaperList.SelectedItem == null)
            {
                MessageBox.Show("Please select a wallpaper first.", "No Wallpaper Selected",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string selected = WallpaperList.SelectedItem.ToString();

            // Example: map selection to HTML URL (you can change to local files later)
            string url = selected switch
            {
                "ColorWaves (HTML)" => "pack://siteoforigin:,,,/Wallpapers/ColorWaves.html",
                "OceanVideo (MP4)" => "pack://siteoforigin:,,,/Wallpapers/OceanVideo.html",
                "Visualizer (Audio)" => "pack://siteoforigin:,,,/Wallpapers/Visualizer.html",
                _ => "https://example.com"
            };

            WallpaperPreview.CoreWebView2.Navigate(url);
            MessageBox.Show($"Applying wallpaper: {selected}", "Wallpaper Applied",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // --- Tray Icon ---
        private void ShowWindow()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        private void ExitApp()
        {
            trayIcon.Visible = false;
            trayIcon.Dispose();
            WpfApp.Application.Current.Shutdown(); // Explicitly call WPF app shutdown
        }

        // --- Window Events ---
        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            if (WindowState == WindowState.Minimized)
                this.Hide(); // Hide from taskbar when minimized
        }

        protected override void OnClosed(EventArgs e)
        {
            trayIcon.Visible = false;
            trayIcon.Dispose();
            base.OnClosed(e);
        }
    }
}
