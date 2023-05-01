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
            //basicSettings.DataContext = new ControlPanelBasicSettingsViewModel();
            moveSettings.DataContext = new ControlPanelMovingSettingsViewModel();
            //scrollSettings.DataContext = new ControlPanelScrollSettingsViewModel();
            btnLeftSetting.DataContext = new ControlPanelLeftButtonSettingsViewModel();
            btnRightSetting.DataContext = new ControlPanelRightButtonSettingsViewModel();
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
