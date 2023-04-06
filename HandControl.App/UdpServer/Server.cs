using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System;

namespace HandControl.App.UdpServer
{
    public class Server : IDisposable
    {
        public string HostName { get; private set; }
        public int SendPort { get; private set; }
        public int ReceivePort { get; private set; }

        public delegate void DataReceived(Server server, string data);
        public event DataReceived OnDataReceived;

        private Task? _listener;

        public Server(string hostName, int sendPort, int receivePort)
        {
            HostName = hostName;
            SendPort = sendPort;
            ReceivePort = receivePort;
            Listen();
        }

        public void Listen()
        {
            UdpClient udpClient = new UdpClient(ReceivePort);
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] receiveBytes;
            string returnData;
            _listener = Task.Run(() => {
                while (true)
                {
                    receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                    returnData = Encoding.ASCII.GetString(receiveBytes);
                    Console.WriteLine($"RECEIVED: {returnData}");        //TODO Logger
                    OnDataReceived?.Invoke(this, returnData);
                }
            });
        }

        public void Dispose()
        {
           if(_listener != null) 
                _listener.Dispose();
        }

        public void SendTo(string data)
        {
            Console.WriteLine($"SENDED: {data}");      //TODO Logger
            UdpClient udpClient = new UdpClient();
            udpClient.Connect(HostName, SendPort);
            byte[] sendBytes = Encoding.ASCII.GetBytes(data);
            udpClient.Send(sendBytes, sendBytes.Length);
        }
    }
}