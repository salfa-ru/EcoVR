using HandControl.App.Model;
using System;

namespace HandControl.App.ViewModel;

public class ControlPanelMovingSettingsViewModel : ControlPanelBaseViewModel
{
    MouseHandController _controller = SingleManager.MouseHandController;

    public ControlPanelMovingSettingsViewModel()
    {
        Title = "Перемещение";
    }

    public int NonSensitiveZone
    {
        get => _controller.NonSensitiveZone;
        set
        {
            _controller.NonSensitiveZone = value;
            OnPropertyChanged(nameof(NonSensitiveZone));
        }
    }

    public float EdgeOffset
    {
        get => _controller.EdgeOffset;
        set
        {
            _controller.EdgeOffset = value;
            OnPropertyChanged(nameof(EdgeOffset));
        }
    }

    public int Smoother
    {
        get => _controller.Smoother;
        set
        {
            _controller.Smoother = value;
            OnPropertyChanged(nameof(Smoother));

        }
    }
}
