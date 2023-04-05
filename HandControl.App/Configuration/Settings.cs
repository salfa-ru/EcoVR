using System.IO;
using Newtonsoft.Json;


namespace HandControl.App.Configuration
{
    public class Settings
    {
        private static string _path => Defaults.Constants.SETTINGS_PATH;

        public UdpConfig UdpConfig { get; set; } = null!;

        public ResolutionConfig ResolutionConfig { get; set; } = null!;

        public DetectorConfig DetectorConfig { get; set; } = null!;

        public FrameConfig FrameConfig { get; set; } = null!;

        public static void Save() 
        {
            var json = JsonConvert.SerializeObject(_instance);
            File.WriteAllText(_path, json.ToString());
        }

        private static Settings _instance = null!;
        
        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(_path)) ?? new Settings();
                }
                return _instance;
            }
        }

        private static Settings GetDefault()
        {
            return new() { 
                UdpConfig = new() { 
                    HostName = "localhost", 
                    PortIn = 6001, 
                    PortOut = 6000},
                ResolutionConfig = new() { 
                    Width = 800,
                    Height = 600,
                },
                DetectorConfig = new()
                { 
                    MaxHands = 1,
                    Detection = 0.8
                }, 
                FrameConfig = new() { 
                    IsDetect = true,
                    IsFlip = false,
                    IsPreview = true
                }
            };
        }
    }
}
