using HandControl.App.ViewModel;
using System.Windows;

namespace HandControl.App.Views.Windows
{

    public partial class ControlWindow : Window
    {
        private bool _hide = true;
        
        public ControlWindow()
        {
            InitializeComponent();
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
