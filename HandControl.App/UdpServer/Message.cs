namespace HandControl.App.UdpServer;

public class Message
{
    public string Status { get; set; } = null!;
    public string Command { get; set; } = null!;
    public MessageArgs? MessageArgs { get; set; }
}
