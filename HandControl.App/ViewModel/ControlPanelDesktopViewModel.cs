using HandControl.App.Model;
using System.Windows.Media;

namespace HandControl.App.ViewModel;

public class ControlPanelDesktopViewModel : ControlPanelBaseViewModel
{
    MouseHandController _controller;
    private Brush DISABLED => new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
    private Brush ENABLED => new SolidColorBrush(Color.FromArgb(10, 255, 255, 255));

    public Brush IsMoveLeft => _controller?.IsMoveLeft == true ? DISABLED : ENABLED;
    public Brush IsMoveRight => _controller?.IsMoveRight == true ? DISABLED : ENABLED;
    public Brush IsMoveUp => _controller?.IsMoveUp == true ? DISABLED : ENABLED;
    public Brush IsMoveDown => _controller?.IsMoveDown == true ? DISABLED : ENABLED;
    public Brush IsLeftButtonPress => _controller?.LeftTrigger.Trigger == true ? DISABLED : ENABLED;
    public Brush IsRightButtonPress => _controller?.RightTrigger.Trigger == true ? DISABLED : ENABLED;

    public double LeftProp { get; set; } = SingleManager.CursorApi.ScreenWidth - 200;
    public double TopProp { get; set; } = SingleManager.CursorApi.ScreenHeight - 250;

    public ControlPanelDesktopViewModel()
    {
        _controller = SingleManager.MouseHandController;
        _controller.OnMouseHandStateChanged += (s, a) => {
            OnPropertyChanged(nameof(IsMoveLeft));
            OnPropertyChanged(nameof(IsMoveRight));
            OnPropertyChanged(nameof(IsMoveUp));
            OnPropertyChanged(nameof(IsMoveDown));
            OnPropertyChanged(nameof(IsLeftButtonPress));
            OnPropertyChanged(nameof(IsRightButtonPress));
        };
    }
}
