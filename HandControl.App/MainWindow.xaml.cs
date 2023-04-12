using HandControl.App.Configuration;
using HandControl.App.Windows;
using System.Windows;
using System.Windows.Media;
using HandControl.App.ViewModel;

namespace HandControl.App
{
    public partial class MainWindow : Window
    {
        private const int R = 255;
        private const int G = 255;
        private const int B = 255;
        private const int A = 0;
        private System.Windows.Forms.NotifyIcon? _icon;
        private ControlWindow? _window;



        public MainWindow()
        {           
            StartInit();
            InitializeComponent();
            canvas.DataContext = new ControlPanelDesktopViewModel();
        }

        private void StartInit()
        {

            System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem("Exit");
            item.Click += (s, e) => this.Close();

            _icon = new System.Windows.Forms.NotifyIcon();
            _icon.Icon = new System.Drawing.Icon(Defaults.Constants.TRAY_ICON_PATH);
            _icon.Visible = true;
            _icon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            _icon.ContextMenuStrip.Items.Add(item);
            _icon.DoubleClick += (sender, args) => _window?.Show();
            
            this.ShowInTaskbar = false;
            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.Topmost = true;
            this.ResizeMode = ResizeMode.NoResize;
            this.Background = new SolidColorBrush(Color.FromArgb(A, R, G, B));
            this.Left = 0;
            this.Top = 0;
            this.Width = SingleManager.CursorApi.ScreenWidth;
            this.Height = SingleManager.CursorApi.ScreenHeight;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            SingleManager.ConversationManager.CommandCreate();
            if (_window == null)  _window = new ControlWindow();           
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            _window?.CloseWindow();
            Settings.Save();
            SingleManager.ConversationManager.CommandKill();
        }
    }
}
