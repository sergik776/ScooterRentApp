using ScooterRent.Hardware.HAL.HardwareProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static ScooterRent.Hardware.HAL.Enums;

namespace ScooterRent.Hardware.HAL
{
    public class ScooterService
    {
        public List<Scooter> Scooters { get { return _Scooters.Select(x => x.Value).ToList(); } }

        public event PropertyHandler? PropertyChanged;
        Dictionary<byte[], Scooter> _Scooters;
        TcpListener listener;

        public ScooterService()
        {
            _Scooters = new Dictionary<byte[], Scooter>();
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
                                    if(pack.Property == RecieveProperty.MAC)
                                    {
                                        var sc = new Scooter(pack.Value, tcpClient);
                                        sc.PropertyChanged += PropertyChanged;
                                        _Scooters.Add(((PhysicalAddress)pack.Value).GetAddressBytes(), sc);
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
