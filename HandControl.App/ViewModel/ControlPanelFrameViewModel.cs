using HandControl.App.Configuration;
using HandControl.App.UdpServer;
using System.Windows.Input;

namespace HandControl.App.ViewModel;

public class ControlPanelFrameViewModel : ControlPanelBaseViewModel
{
    private FrameConfig _config;

    public bool IsDetect
    {
        get { return _config.IsDetect; }
        set { _config.IsDetect = value; OnPropertyChanged(nameof(IsDetect)); }
    }

    public bool IsFlip
    {
        get { return _config.IsFlip; }
        set { _config.IsFlip = value; OnPropertyChanged(nameof(IsFlip)); }
    }

    public bool IsPreview
    {
        get { return _config.IsPreview; }
        set { _config.IsPreview = value; OnPropertyChanged(nameof(IsPreview)); }
    }

    public ICommand FrameCommand { get; private set; }

    public ControlPanelFrameViewModel()
    {
        _config = Settings.Instance.FrameConfig;
        Title = "Конфигурация кадра";
        FrameCommand = new Command((_) => SingleManager.ConversationManager.CommandFrameApply(IsDetect, IsFlip, IsPreview));
    }
}
