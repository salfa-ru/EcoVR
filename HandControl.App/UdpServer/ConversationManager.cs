using HandControl.App.Configuration;
using HandControl.App.ReceivedDataHandlers;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            //CorrectingPath();
            StartScriptEnvironment();
        });

        static void StartScriptEnvironment()
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
            psi.RedirectStandardInput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = false;
            Process? cmd = Process.Start(psi);
            if (cmd != null)
            {
                cmd.StandardInput.WriteLine("call detector\\myenv\\Scripts\\activate.bat");
                cmd.StandardInput.WriteLine("python --version");
                cmd.StandardInput.WriteLine("pip install opencv-python");
                cmd.StandardInput.WriteLine("pip install cvzone");
                cmd.StandardInput.WriteLine("pip install mediapipe");
                cmd.StandardInput.WriteLine("python detector\\main.py");
                cmd.WaitForExit();
            }
        }
        static void CorrectingPath()
        {
            var path = Directory.GetCurrentDirectory() + "\\detector\\myenv\\Scripts\\activate";
            var pathBat = Directory.GetCurrentDirectory() + "\\detector\\myenv\\Scripts\\activate.bat";
            var pathCon = Directory.GetCurrentDirectory() + "\\detector\\myenv\\pyvenv.cfg";
            var pathEnv = Directory.GetCurrentDirectory() + "\\detector\\myenv";
            
            var lines = File.ReadAllLines(path);
            StringBuilder sb = new StringBuilder();
            foreach (var item in lines)
            {
                if (item.StartsWith("VIRTUAL_ENV=")) sb.Append("VIRTUAL_ENV=\"" + Directory.GetCurrentDirectory() + "\\detector\\myenv\"\r\n");
                else sb.Append(item + "\r\n");
            }
            File.WriteAllText(path, sb.ToString());

            lines = File.ReadAllLines(pathBat);
            sb = new StringBuilder();
            foreach (var item in lines)
            {
                if (item.StartsWith("set VIRTUAL_ENV")) sb.Append("set VIRTUAL_ENV" + Directory.GetCurrentDirectory() + "\\detector\\myenv\r\n");
                else sb.Append(item + "\r\n");
            }
            File.WriteAllText(pathBat, sb.ToString());

            lines = File.ReadAllLines(pathCon);
            sb = new StringBuilder();
            foreach (var item in lines)
            {
                if (item.StartsWith("home = ")) sb.Append("home = " + Directory.GetCurrentDirectory() + "\\detector\\myenv\\Scripts\r\n");
                else sb.Append(item + "\r\n");
            }
            File.WriteAllText(pathCon, sb.ToString());

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
