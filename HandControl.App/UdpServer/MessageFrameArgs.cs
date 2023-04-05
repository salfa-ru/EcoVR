namespace HandControl.App.UdpServer;

public class MessageFrameArgs : MessageArgs
{
    public bool IsDetect { get; set; }
    public bool IsFlip { get; set; }
    public bool IsPreview { get; set; }
}
