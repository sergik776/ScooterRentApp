using ScooterRent.Hardware.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static ScooterRent.Hardware.HAL.Enums;
using ScooterRentApp.Hardware.Server.HardwareProtocol;

namespace ScooterRentApp.Hardware.Server
{
    public class ScooterService
    {
        public List<Scooter> Scooters { get { return _Scooters.Select(x => x.Value).ToList(); } }
        public Scooter GetById(PhysicalAddress k)
        {
            return _Scooters[BitConverter.ToString(((PhysicalAddress)k).GetAddressBytes())];
        }
        public event PropertyHandler? PropertyChanged;
        Dictionary<string, Scooter> _Scooters;
        TcpListener listener;

        public ScooterService()
        {
            _Scooters = new Dictionary<string, Scooter>();
            listener = new TcpListener(IPAddress.Any, 8888);
            listener.Start();

            Task.Factory.StartNew(() => {
                try
                {
                    while (true)
                    {
                        try
                        {
                            var tcpClient = listener.AcceptTcpClientAsync().Result;
                            Task.Factory.StartNew(() => {
                                byte[] buffer = new byte[17];
                                tcpClient.Client.Receive(buffer);
                                try
                                {
                                    var pack = PacketDesiarizable.Desiarizable(buffer);
                                    if (pack.Property == RecieveProperty.MAC)
                                    {
                                        var sc = new Scooter(pack.Value, tcpClient);
                                        sc.PropertyChanged += PropertyChanged;
                                        _Scooters.Add(BitConverter.ToString(((PhysicalAddress)pack.Value).GetAddressBytes()), sc);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine(ex.Message);
                                }
                            });
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine(ex.Message);
                        }
                    }
                }
                finally
                {
                    listener.Stop();
                }
            });
        }
    }
}
