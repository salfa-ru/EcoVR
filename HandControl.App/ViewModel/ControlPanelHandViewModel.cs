using HandControl.App.Model;

namespace HandControl.App.ViewModel;

public class ControlPanelHandViewModel : ControlPanelBaseViewModel
{
    MouseHandController _controller = SingleManager.MouseHandController;

    public ControlPanelHandViewModel()
    {
        Title = "Настройка сработки событий";
    }

    [RangeFloat(0.1F, 0.9F)]
    public float NonsensitiveZoneVerticalValue
    {
        get => _controller.NonsensitiveZoneVerticalValue;
        set
        {
            _controller.NonsensitiveZoneVerticalValue = value;
            OnPropertyChanged(nameof(NonsensitiveZoneVerticalValue));
        }
    }
    

    [RangeFloat(0.1F, 0.9F)]
    public float NonsensitiveZoneHorizontalValue
    {
        get => _controller.NonsensitiveZoneHorizontalValue;
        set
        {
            _controller.NonsensitiveZoneHorizontalValue = value;
            OnPropertyChanged(nameof(NonsensitiveZoneHorizontalValue));
        }
    }

    [RangeFloat(-0.2F, 0.2F)]
    public float HorizontalOffset
    {
        get => _controller.HorizontalOffset;
        set
        {
            _controller.HorizontalOffset = value;
            OnPropertyChanged(nameof(HorizontalOffset));
        }
    }

    [RangeFloat(-0.2F, 0.2F)]
    public float VerticalOffset
    {
        get => _controller.VerticalOffset;
        set
        {
            _controller.VerticalOffset = value;
            OnPropertyChanged(nameof(VerticalOffset));
        }
    }

    [RangeFloat(0.1F, 0.5F)]
    public float ActivationLeftMouseZone
    {
        get => _controller.ActivationLeftMouseZone;
        set
        {
            _controller.ActivationLeftMouseZone = value;
            OnPropertyChanged(nameof(ActivationLeftMouseZone));
        }
    }

    [RangeFloat(0.0F, 0.5F)]
    public float ActivationRightMouseZone
    {
        get => _controller.ActivationRightMouseZone;
        set
        {
            _controller.ActivationRightMouseZone = value;
            OnPropertyChanged(nameof(ActivationRightMouseZone));
        }
    }
}
