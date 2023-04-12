using HandControl.App.Configuration;
using HandControl.App.ReceivedDataHandlers;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HandControl.App.UdpServer;

public class ConversationManager
{
    private Settings _settings = Settings.Instance;

    public ConversationManager() 
    {
        var host = _settings.UdpConfig.HostName;
        var sendPort = _settings.UdpConfig.PortOut;
        var receivePort = _settings.UdpConfig.PortIn;
        _server = new Server(host, sendPort, receivePort);
        _server.OnDataReceived += Server_OnDataReceived;
    }

    private void Server_OnDataReceived(Server server, string data)
    {
        new DataParser().Parse(data);
    }

    private Server _server;

    private void SendJson(Message message) => _server.SendTo(JsonConvert.SerializeObject(message));

    public void CommandStart(int index, int width, int height)
    {
        SendJson(new Message() { Status = "main", Command = "start",
            MessageArgs = new MessageResolutionArgs()
            {
                Index = index,
                Width = width,
                Height = height
            }
        });
    }

    public void CommandKill()
    {
        SendJson(new Message() { Status = "main", Command = "kill" });
    }

    public void CommandCreate()
    {
        Task.Run(() =>
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
            psi.RedirectStandardInput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            Process? cmd = Process.Start(psi);
            if (cmd != null)
            {
                cmd.StandardInput.WriteLine("call detector\\myenv\\Scripts\\activate.bat");
                cmd.StandardInput.WriteLine("python detector\\main.py");
                cmd.WaitForExit();
            }
        });
    }
 
    public void CommandStop()
    {
        SendJson(new Message() { Status = "main", Command = "stop" });
    }

    public void CommandCameraSetResolution(int index, int width, int height)
    {
        SendJson(
            new Message()
            {
                Status = "camera",
                Command = "resolution",
                MessageArgs = new MessageResolutionArgs()
                { 
                    Index = index,
                    Width = width,
                    Height = height 
                } 
            });       
    }

    public void CommandFrameApply(bool isDetect, bool isFlip, bool isPreview)
    {
        SendJson(
            new Message()
            {
                Status = "camera",
                Command = "flags",
                MessageArgs = new MessageFrameArgs()
                {
                    IsDetect = isDetect,
                    IsFlip = isFlip,
                    IsPreview = isPreview
                }
            });
    }

    public void CommandStartDetectingData()
    {
        SendJson(
            new Message()
            {
                Status = "data",
                Command = "start_send"
            });
    }

    public void CommandStopDetectingData()
    {
        SendJson(
            new Message()
            {
                Status = "data",
                Command = "stop_send"
            });
    }

    public void CommandDrawRectangle(int left, int top, int right, int bottom)
    {
        SendJson(
            new Message()
            {
                Status = "draw",
                Command = "rectangle",
                MessageArgs = new MessageRectangleArgs()
                {
                    Left = left,
                    Right = right,
                    Top = top,
                    Bottom = bottom
                }
            });
    }
}
