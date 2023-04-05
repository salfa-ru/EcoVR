using System;

namespace HandControl.App.UdpServer;

public class MessageRectangleArgs: MessageArgs
{
    public int Left { get; set; }
    public int Right { get; set; }
    public int Top { get; set; }
    public int Bottom { get; set; }
    

}        
