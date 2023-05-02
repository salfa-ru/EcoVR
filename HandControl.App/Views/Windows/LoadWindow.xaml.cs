using HandControl.App.ViewModel;
using System.Windows;

namespace HandControl.App.Views.Windows;

public partial class LoadWindow : Window
{
    public LoadWindow()
    {
        InitializeComponent();

        stackState.DataContext = new ControlPanelLoadViewModel();
        Closing += (sender, e) =>
        {
            DialogResult = true;
        };
        SingleManager.LoadStatus.StateChange += (sender) =>
        {
            if (sender.Status.Contains("Запуск"))
                Dispatcher.BeginInvoke(Close);
        };
    }
}
