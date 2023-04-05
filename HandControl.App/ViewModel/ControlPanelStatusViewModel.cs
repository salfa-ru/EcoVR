using HandControl.App.ReceivedDataHandlers;

namespace HandControl.App.ViewModel;

public class ControlPanelStatusViewModel :ControlPanelBaseViewModel
{
    public string State { get; set; } = string.Empty;
    private StatusData _model;
    public ControlPanelStatusViewModel()
    { 
        Title = "Статус";
        _model = SingleManager.StatusData;
        _model.OnStateChanged += StatusModel_OnStateChanged;
    }

    private void StatusModel_OnStateChanged(object? sender, System.EventArgs e)
    {
        State = _model.Status;
        OnPropertyChanged(nameof(State));
    }
}
