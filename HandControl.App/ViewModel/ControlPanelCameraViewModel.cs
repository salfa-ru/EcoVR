using HandControl.App.Configuration;
using HandControl.App.UdpServer;
using Accord.Video.DirectShow;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using HandControl.App.ReceivedDataHandlers;

namespace HandControl.App.ViewModel;

public class ControlPanelCameraViewModel : ControlPanelBaseViewModel
{
    private ResolutionConfig _config;
    private Settings _settings = Settings.Instance;
    private FilterInfoCollection _videoDevices;
    private int _selectedResolution;
    public ICommand StartCapturingCommand { get; init; } = null!;
    public ICommand StopCapturingCommand { get; init; } = null!;
    public ICommand RestartCapturingCommand { get; init; } = null!;


    public List<Resolution> Resolutions => Resolution.GetResolutions();

    public IEnumerable<string> CamerasNames => _videoDevices.Select(s => s.Name).ToList();

    public int Index
    {
        get { return _config.Index; }
        set { _config.Index = value; OnPropertyChanged(nameof(Index)); }
    }

    public int SelectedResolution
    {
        get { return _selectedResolution; }
        set
        {
            _selectedResolution = value;
            OnPropertyChanged(nameof(SelectedResolution));
            _config.Width = Resolutions[_selectedResolution].Width;
            _config.Height = Resolutions[_selectedResolution].Height;
        }
    }

    public bool IsCanStart { get; set; }
    public bool IsCanStop { get; set; }
    public bool IsCanRestart { get; set; }
    


    public ControlPanelCameraViewModel()
    {
        _config = _settings.ResolutionConfig;
        var conversation = SingleManager.ConversationManager;
        Title = "Конфигурация камеры";
        StartCapturingCommand = new Command((_)=> conversation.CommandStart(_config.Index, _config.Width, _config.Height));
        StopCapturingCommand = new Command((_) => conversation.CommandStop());
        RestartCapturingCommand = new Command((_) => conversation.CommandCameraSetResolution(_config.Index, _config.Width, _config.Height));
        _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        SelectedResolution = Find();
        StatusData status = SingleManager.StatusData;
        status.OnStateChanged += (sender, args) => {
            IsCanStart = status.State == StatusState.Stopped;
            IsCanStop = status.State == StatusState.Captured;
            IsCanRestart = status.State == StatusState.Captured;           
            OnPropertyChanged(nameof(IsCanStart));  
            OnPropertyChanged(nameof(IsCanStop));  
            OnPropertyChanged(nameof(IsCanRestart));  
        };
    }

    private int Find()
    {
        for (int i = 0; i < Resolutions.Count; i++)
            if (Resolutions[i].Width == _config.Width && Resolutions[i].Height == _config.Height) return i;        
        return -1;
    }
}



