namespace HandControl.App.ViewModel;

public class ControlPanelLoadViewModel : ControlPanelBaseViewModel
{
    private string state = string.Empty;

    public string ProgramTitle { get; set; } = "HandControl";
    public string ProgramDescription { get; set; } = "Description field";
    public string ProgramRights { get; set; } = "RightsField";
    public string State { 
        get => state;
        set 
        { 
            state = value; 
            OnPropertyChanged(nameof(State));
        }
    } 

    public ControlPanelLoadViewModel()
    {
        SingleManager.LoadStatus.StateChange += (status) => {
            State = status.Status;
        };
    }
}

