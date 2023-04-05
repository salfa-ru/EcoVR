using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HandControl.App.ViewModel;

public class ControlPanelBaseViewModel : INotifyPropertyChanged
{
    public string Title { get; init; }=string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

