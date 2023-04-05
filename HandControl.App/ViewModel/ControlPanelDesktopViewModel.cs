using HandControl.App.Model;
using System.Windows;
using System.Windows.Media;

namespace HandControl.App.ViewModel;

public class ControlPanelDesktopViewModel : ControlPanelBaseViewModel
{
    MouseHandController _controller;
    private Brush DISABLED => new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
    private Brush ENABLED => new SolidColorBrush(Color.FromArgb(10, 255, 255, 255));

    public Brush IsMoveLeft =>_controller.IsMoveLeft ? DISABLED : ENABLED;
    public Brush IsMoveRight =>_controller.IsMoveRight ? DISABLED : ENABLED;
    public Brush IsMoveUp =>_controller.IsMoveUp ? DISABLED : ENABLED;
    public Brush IsMoveDown =>_controller.IsMoveDown ? DISABLED : ENABLED;

    public Brush IsLeftButtonPress => _controller.MouseLeftDownTrigger ? DISABLED : ENABLED;
    public Brush IsRightButtonPress => _controller.MouseRightDownTrigger ? DISABLED : ENABLED;

    public double LeftProp { get; set; }
    public double TopProp { get; set; }


    public ControlPanelDesktopViewModel()
    {
        _controller = SingleManager.MouseHandController;
        LeftProp = SingleManager.CursorApi.ScreenWidth - 200;
        TopProp = SingleManager.CursorApi.ScreenHeight - 250;
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
