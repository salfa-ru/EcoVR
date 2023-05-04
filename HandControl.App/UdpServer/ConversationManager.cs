using HandControl.App.Configuration;
using HandControl.App.Loger;
using HandControl.App.ReceivedDataHandlers;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Channels;
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
        SendJson(new Message()
        {
            Status = "main",
            Command = "start",
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
            StartScriptEnvironment();
        });

        static void StartScriptEnvironment()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;

                Process? cmd = Process.Start(psi);
                if (cmd != null)
                {
                    cmd.OutputDataReceived += (s, e) =>
                    {
                        if (e.Data != null)
                        {
                            SingleManager.LoadStatus.Status = e.Data;

                        }
                    };
                    cmd.BeginOutputReadLine();
                    cmd.StandardInput.WriteLine("cd detector");
                    if (!Directory.Exists("detector\\myenv"))
                    {
                        cmd.StandardInput.WriteLine("start /WAIT /B ./python310/python.exe ./python310/get-pip.py");
                        cmd.StandardInput.WriteLine("start /WAIT /B ./python310/python.exe -m pip install virtualenv");
                        cmd.StandardInput.WriteLine("start /WAIT /B ./python310/python.exe -m virtualenv myenv");
                        cmd.StandardInput.WriteLine("call ./myenv/Scripts/activate.bat");
                        cmd.StandardInput.WriteLine("python --version");
                        cmd.StandardInput.WriteLine("pip install opencv-python");
                        cmd.StandardInput.WriteLine("pip install cvzone");
                        cmd.StandardInput.WriteLine("pip install mediapipe");
                        cmd.StandardInput.WriteLine("call ./myenv/Scripts/deactivate.bat");
                    }
                    cmd.StandardInput.WriteLine("call ./myenv/Scripts/activate.bat");
                    cmd.StandardInput.WriteLine("python --version");
                    cmd.StandardInput.WriteLine("python main.py 6000 6001");
                    cmd.WaitForExit();
                }
            }
            catch (Exception err)
            {

                ErrorLoger.Log(err.Message);
            }
        }
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
