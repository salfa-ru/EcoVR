using HandControl.App.ViewModel;
using System.Windows;

namespace HandControl.App.Windows
{

    public partial class ControlWindow : Window
    {
        private bool _hide = true;
        
        public ControlWindow()
        {
             InitializeComponent();
             camConfig.DataContext = new ControlPanelCameraViewModel();
             frConfig.DataContext = new ControlPanelFrameViewModel();
             remCapture.DataContext = new ControlPanelMainCommandViewModel();
             stConfig.DataContext = new ControlPanelStatusViewModel();
             sensConfig.DataContext = new ControlPanelHandViewModel();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {                      
            if (_hide)
            {
                e.Cancel = _hide;
                this.Hide();
            }
        }
        public void CloseWindow()
        {
            _hide = false;
            this.Close();
        }
    }
}
