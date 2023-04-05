using System.Text.Json.Serialization;


namespace HandControl.App.Configuration
{
    public class UdpConfig
    {
        public string HostName { get; set; } = string.Empty;
        public int PortIn { get; set; }
        public int PortOut { get; set; }
    }
}
