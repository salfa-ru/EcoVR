using HandControl.App.ViewModel;
using System.Windows;


namespace HandControl.App
{
    /// <summary>
    /// Interaction logic for LoadWindow.xaml
    /// </summary>
    public partial class LoadWindow : Window
    {
        public LoadWindow()
        {
            InitializeComponent();
         
            stackState.DataContext = new ControlPanelLoadViewModel();
            this.Closing += (sender, e) => {
                this.DialogResult = true;
            };
            SingleManager.LoadStatus.StateChange += (sender) =>
            {
                if (sender.Status.Contains("Запуск"))
                    Dispatcher.BeginInvoke(Close);
            };
        }
    }
}
