using Accord.Video.DirectShow;
using HandControl.App.Configuration;
using HandControl.App.ReceivedDataHandlers;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;

namespace HandControl.App.ViewModel
{
    public class ControlPanelBasicSettingsViewModel : ControlPanelBaseViewModel
    {
        private FrameConfig _frameConfig;
        private ResolutionConfig _resolutionConfig;
        private Settings _settings = Settings.Instance;
        private FilterInfoCollection _videoDevices;
        private int _selectedResolution;
        private StatusData _status;
        private bool isEnable;

        public ICommand StartCapturingCommand { get; init; } = null!;
        public ICommand StopCapturingCommand { get; init; } = null!;
        public ICommand RestartCapturingCommand { get; init; } = null!;
        public bool? IsPreview
        {
            get { return _frameConfig.IsPreview; }
            set
            {
                _frameConfig.IsPreview = value == true;
                OnPropertyChanged(nameof(IsPreview));
                SingleManager.ConversationManager.CommandFrameApply(true, false, IsPreview == true);
            }
        }
        public string State { get; set; } = string.Empty;

        public bool IsSendingData
        {
            get => isEnable;
            set
            {
                isEnable = value;
                if (value) SingleManager.ConversationManager.CommandStartDetectingData();
                else SingleManager.ConversationManager.CommandStopDetectingData();
                OnPropertyChanged(nameof(IsSendingData));
            }
        }

        public ControlPanelBasicSettingsViewModel()
        {
            _status = SingleManager.StatusData;
            _status.OnStateChanged += (s, a) =>
            {
                State = _status.Status;
                OnPropertyChanged(nameof(State));
            };
            _frameConfig = Settings.Instance.FrameConfig;
            _resolutionConfig = _settings.ResolutionConfig;
            var conversation = SingleManager.ConversationManager;
            Title = "Конфигурация камеры";
            StartCapturingCommand = new Command((_) => conversation.CommandStart(_resolutionConfig.Index, _resolutionConfig.Width, _resolutionConfig.Height));
            StopCapturingCommand = new Command((_) => conversation.CommandStop());
            RestartCapturingCommand = new Command((_) => conversation.CommandCameraSetResolution(_resolutionConfig.Index, _resolutionConfig.Width, _resolutionConfig.Height));
            _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            
            SelectedResolution = Find();
            StatusData status = SingleManager.StatusData;
            status.OnStateChanged += (sender, args) =>
            {
                IsCanStart = status.State == StatusState.Stopped;
                IsCanStop = status.State == StatusState.Captured;
                IsCanRestart = status.State == StatusState.Captured;
                IsCanDataSending = status.State == StatusState.Captured;
                OnPropertyChanged(nameof(IsCanStart));
                OnPropertyChanged(nameof(IsCanStop));
                OnPropertyChanged(nameof(IsCanRestart));
                OnPropertyChanged(nameof(IsCanDataSending));
            };
        }

        public List<Resolution> Resolutions => Resolution.GetResolutions();
        public IEnumerable<string> CamerasNames => _videoDevices.Select(s => s.Name).ToList();
        public int Index
        {
            get { return _resolutionConfig.Index; }
            set { _resolutionConfig.Index = value; OnPropertyChanged(nameof(Index)); }
        }
        public int SelectedResolution
        {
            get { return _selectedResolution; }
            set
            {
                _selectedResolution = value;
                OnPropertyChanged(nameof(SelectedResolution));
                _resolutionConfig.Width = Resolutions[_selectedResolution].Width;
                _resolutionConfig.Height = Resolutions[_selectedResolution].Height;
            }
        }
        public bool IsCanStart { get; set; }
        public bool IsCanStop { get; set; }
        public bool IsCanRestart { get; set; }
        public bool IsCanDataSending { get; set; }
        private int Find()
        {
            for (int i = 0; i < Resolutions.Count; i++)
                if (Resolutions[i].Width == _resolutionConfig.Width && Resolutions[i].Height == _resolutionConfig.Height) return i;
            return -1;
        }
    }
}
