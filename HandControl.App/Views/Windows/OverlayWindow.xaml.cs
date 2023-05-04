using HandControl.App.Configuration;
using HandControl.App.Views.Windows;
using System.Windows;
using System.Windows.Media;
using HandControl.App.ViewModel;

namespace HandControl.App.Views.Windows;

public partial class OverlayWindow : Window
{
    private const int R = 255;
    private const int G = 255;
    private const int B = 255;
    private const int A = 0;
    private System.Windows.Forms.NotifyIcon? _icon;
    private ControlWindow? _window;

    public OverlayWindow()
    {
        StartInit();
        InitializeComponent();
    }

    private void StartInit()
    {

        System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem("Выход");
        item.Click += (s, e) => this.Close();

        _icon = new System.Windows.Forms.NotifyIcon();
        _icon.Icon = new System.Drawing.Icon(Defaults.Constants.TRAY_ICON_PATH);
        _icon.Visible = false;
        _icon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
        _icon.ContextMenuStrip.Items.Add(item);
        _icon.DoubleClick += (sender, args) =>
        {
            if (_window == null) return;
            if (_window.Visibility == Visibility.Hidden)
                _window.Visibility = Visibility.Visible;
            else
                _window.Visibility = Visibility.Hidden;
        };

        this.ShowInTaskbar = false;
        this.WindowStyle = WindowStyle.None;
        this.AllowsTransparency = true;
        this.Topmost = true;
        this.ResizeMode = ResizeMode.NoResize;
        this.Background = new SolidColorBrush(Color.FromArgb(A, R, G, B));
        this.Width = SingleManager.CursorApi.ScreenWidth;
        this.Height = SingleManager.CursorApi.ScreenHeight;
        this.WindowState = WindowState.Maximized;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        SingleManager.ConversationManager.CommandCreate();
        LoadWindow lw = new LoadWindow();
        bool? result = lw.ShowDialog();
        if (result == true)
        {
            if (_window == null)
            {
                _window = new ControlWindow();
                if (_icon != null)
                    _icon.Visible = true;

            }

        }
    }

    private void Window_Closed(object sender, System.EventArgs e)
    {
        _window?.CloseWindow();
        Settings.Save();
        SingleManager.ConversationManager.CommandKill();
    }
}
