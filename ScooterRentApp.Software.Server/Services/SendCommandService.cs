using System.Net.Sockets;
using System.Security.Cryptography;

namespace ScooterRentApp.Software.Server.Services
{
    public class SendCommandService
    {
        private TcpClient _tcpClient;

        public SendCommandService()
        {
            
        }

        public void SendRentalTime(byte[] mac, ushort sec)
        {
            if (_tcpClient != null)
            {
                _tcpClient.Dispose();
            }
            _tcpClient = new TcpClient();
            _tcpClient.Connect("127.0.0.1", 8889);
            Thread.Sleep(10);
            _tcpClient.Client.Send(new byte[] { 0xFF }.Concat(mac).Concat(BitConverter.GetBytes(sec)).ToArray());
        }
    }
}
