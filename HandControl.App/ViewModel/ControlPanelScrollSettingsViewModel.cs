using HandControl.App.Model;
using System;

namespace HandControl.App.ViewModel;

public class ControlPanelScrollSettingsViewModel : ControlPanelBaseViewModel
{
    MouseHandController _controller = SingleManager.MouseHandController;

    public ControlPanelScrollSettingsViewModel()
    {
        Title = "Скролинг";
    }

    public Mark[] Items => (Mark[])Enum.GetValues(typeof(Mark));

    public int SelectedIndexRemote
    {
        get => (int)_controller.ScrollRemote;
        set
        {
            _controller.ScrollRemote = (Mark)value;
            OnPropertyChanged(nameof(SelectedIndexRemote));
        }

    }
    public int SelectedIndexMain
    {
        get => (int)_controller.ScrollMain;
        set
        {
            _controller.ScrollMain = (Mark)value;
            OnPropertyChanged(nameof(SelectedIndexMain));
        }
    }

    public float Offset
    {
        get => _controller.ScrollOffset;
        set
        {
            _controller.ScrollOffset = value;
            OnPropertyChanged(nameof(Offset));
        }
    }

    public float NonSensitiveZone
    {
        get => _controller.ScrollNonsensitiveZone;
        set
        {
            _controller.ScrollNonsensitiveZone = value;
            OnPropertyChanged(nameof(NonSensitiveZone));
        }
    }

    public float Accelerate
    {
        get => _controller.ScrollAcseleration;
        set
        {
            _controller.ScrollAcseleration = value;
            OnPropertyChanged(nameof(Accelerate));
        }
    }

    public float SpeedLimit
    {
        get => _controller.ScrollLimitSpeed;
        set
        {
            _controller.ScrollLimitSpeed = value;
            OnPropertyChanged(nameof(SpeedLimit));
        }
    }
}
