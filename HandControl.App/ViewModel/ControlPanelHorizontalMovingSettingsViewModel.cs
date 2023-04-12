using HandControl.App.Model;
using System;

namespace HandControl.App.ViewModel;

public class ControlPanelHorizontalMovingSettingsViewModel : ControlPanelBaseViewModel
{
    MouseHandController _controller = SingleManager.MouseHandController;
    private int selectedIndexRemote;
    private int selectedIndexMain;

    public ControlPanelHorizontalMovingSettingsViewModel()
    {
        Title = "Перемещение по горизонтали";
        SelectedIndexRemote = (int)_controller.HorizontalRemote;
        SelectedIndexMain = (int)_controller.HorizontalMain;
    }

    public Mark[] Items => (Mark[])Enum.GetValues(typeof(Mark));

    public int SelectedIndexRemote
    {
        get => selectedIndexRemote;
        set
        {
            selectedIndexRemote = value;
            OnPropertyChanged(nameof(SelectedIndexRemote));
        }

    }
    public int SelectedIndexMain
    {
        get => selectedIndexMain;
        set
        {
            selectedIndexMain = value;
            OnPropertyChanged(nameof(SelectedIndexMain));
        }
    }

    public float HorizontalOffset 
    { 
        get => _controller.HorizontalOffset;
        set
        {
            _controller.HorizontalOffset = value;
            OnPropertyChanged(nameof(HorizontalOffset));
        }
    }

    public float NonSensitiveZone
    {
        get => _controller.HorizontalNonSensitiveZone;
        set
        {
            _controller.HorizontalNonSensitiveZone = value;
            OnPropertyChanged(nameof(NonSensitiveZone));
        }
    }

    public float Accelerate
    {
        get => _controller.HorizontalAcseleration;
        set
        {
            _controller.HorizontalAcseleration = value;
            OnPropertyChanged(nameof(Accelerate));
        }
    }

    public float SpeedLimit
    {
        get => _controller.HorizontalLimitSpeed;
        set
        {
            _controller.HorizontalLimitSpeed = value;
            OnPropertyChanged(nameof(SpeedLimit));
        }
    }
}
