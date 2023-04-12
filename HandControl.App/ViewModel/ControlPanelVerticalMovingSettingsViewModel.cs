using HandControl.App.Model;
using System;

namespace HandControl.App.ViewModel;

public class ControlPanelVerticalMovingSettingsViewModel : ControlPanelBaseViewModel
{
    MouseHandController _controller = SingleManager.MouseHandController;

    public ControlPanelVerticalMovingSettingsViewModel()
    {
        Title = "Перемещение по вертикали";
    }

    public Mark[] Items => (Mark[])Enum.GetValues(typeof(Mark));

    public int SelectedIndexRemote
    {
        get => (int)_controller.VerticalRemote;
        set
        {
            _controller.VerticalRemote = (Mark)value;
            OnPropertyChanged(nameof(SelectedIndexRemote));
        }

    }
    public int SelectedIndexMain
    {
        get => (int)_controller.VerticalMain;
        set
        {
            _controller.VerticalMain = (Mark)value;
            OnPropertyChanged(nameof(SelectedIndexMain));
        }
    }

    public float Offset
    {
        get => _controller.VerticalOffset;
        set
        {
            _controller.VerticalOffset = value;
            OnPropertyChanged(nameof(Offset));
        }
    }

    public float NonSensitiveZone
    {
        get => _controller.VerticalNonSensitiveZone;
        set
        {
            _controller.VerticalNonSensitiveZone = value;
            OnPropertyChanged(nameof(NonSensitiveZone));
        }
    }

    public float Accelerate
    {
        get => _controller.VerticalAcseleration;
        set
        {
            _controller.VerticalAcseleration = value;
            OnPropertyChanged(nameof(Accelerate));
        }
    }

    public float SpeedLimit
    {
        get => _controller.VerticalLimitSpeed;
        set
        {
            _controller.VerticalLimitSpeed = value;
            OnPropertyChanged(nameof(SpeedLimit));
        }
    }
}
