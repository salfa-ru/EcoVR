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

    [RangeFloat(0.1F, 1.0F)]
    public float AccelerateX
    {
        get => _controller.StepX;
        set
        {
            _controller.StepX = value;
            OnPropertyChanged(nameof(AccelerateX));
        }
    }

    [RangeFloat(0.1F, 1.0F)]
    public float AccelerateY
    {
        get => _controller.StepY;
        set
        {
            _controller.StepY = value;
            OnPropertyChanged(nameof(AccelerateY));
        }
    }

    [RangeFloat(10F, 100F)]
    public float SpeedLimitX
    {
        get => _controller.LimitX;
        set
        {
            _controller.LimitX = value;
            OnPropertyChanged(nameof(SpeedLimitX));
        }
    }

    [RangeFloat(10F, 100F)]
    public float SpeedLimitY
    {
        get => _controller.LimitY;
        set
        {
            _controller.LimitY = value;
            OnPropertyChanged(nameof(SpeedLimitY));
        }
    }

}
