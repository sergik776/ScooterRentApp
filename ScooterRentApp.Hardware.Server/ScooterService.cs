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
using Grpc.Net.Client;
using System.Net.Http;
using ScooterRentApp.Hardware.Server.HardwareProtocol.Commands;

namespace ScooterRentApp.Hardware.Server
{
    public class ScooterService
    {
        public List<IScooterManager> Scooters { get { return _Scooters.Select(x => x.Value).ToList(); } }
        public IScooterManager GetById(PhysicalAddress k)
        {
            return _Scooters[BitConverter.ToString(((PhysicalAddress)k).GetAddressBytes())];
        }
        public event PropertyHandler? PropertyChanged;
        Dictionary<string, IScooterManager> _Scooters;
        TcpListener listener;
        TcpListener CommandListner;

        public void SetScooterTime(string mac, ushort seconds)
        {
            _Scooters[mac].SetRentalTime(seconds);
        }

        GrpcChannel _channel;
        ScooterGRPCService.ScooterGRPCServiceClient _grpcClient;

        public ScooterService()
        {
            _channel = GrpcChannel.ForAddress("https://localhost:7018");
            _grpcClient = new ScooterGRPCService.ScooterGRPCServiceClient(_channel);


            _Scooters = new Dictionary<string, IScooterManager>();
            listener = new TcpListener(IPAddress.Any, 8888);
            listener.Start();

            CommandListner = new TcpListener(IPAddress.Any, 8889);
            CommandListner.Start();

            Task.Factory.StartNew(() => 
            {
                try
                {
                    while (true)
                    {
                        using (var client = CommandListner.AcceptTcpClient())
                        {
                            byte[] buffer = new byte[9];
                            client.Client.Receive(buffer);
                            var command = SetRentalTime.GetTime(buffer);
                            _Scooters[BitConverter.ToString(command.MAC)].SetRentalTime(command.Seconds);
                            //client.Dispose();
                        }
                    }
                }
                catch(Exception ex) 
                {
                    Console.WriteLine(ex.Message);
                }
            });

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
                                        var sc = new Scooter(pack.Value, tcpClient, _grpcClient);
                                        sc.PropertyChanged += PropertyChanged;
                                        _Scooters.Add(BitConverter.ToString(((PhysicalAddress)pack.Value).GetAddressBytes()), sc);
                                        var reply = _grpcClient.SetScooter(new ScooterRequest() 
                                        { 
                                            Mac = sc.MAC.ToString(),
                                            BatteryLevel = sc.BatteryLevel + "%",
                                            Position = sc.Position.ToString(),
                                            RentalTime = sc.RentalTime.ToString(),
                                            Speed = sc.Speed + "km/h"
                                        });
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
