using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.SignalR;
using ScooterRent.Hardware.HAL;
using ScooterRent.Software.Server.Hubs;
using ScooterRent.Software.Server.Models;
using System.Collections.Generic;

namespace ScooterRentApp.Software.Server.Services
{
    public class ScooterListService
    {
        private readonly ILogger<ScooterListService> logger;

        public IEnumerable<IBaseScooter> Scooters { get { return _Scooters.Values.ToList(); } }

        public Dictionary<string, ServerScooter> _Scooters;

        ScooterEventHub _EventHub;

        public ScooterListService(ILogger<ScooterListService> _logger, ScooterEventHub hub)
        {
            _Scooters = new Dictionary<string, ServerScooter>();
            this.logger = _logger;
            _EventHub = hub;
        }

        public void AddScooter(MacRequest request)
        {
            string mac = BitConverter.ToString(request.Mac.ToByteArray());
            _Scooters.TryAdd(mac, new ServerScooter(request.Mac.ToByteArray()));
            _EventHub.Clients.Group("Manager").SendAsync("AddScooter", new ScooterProperty(mac.Replace("-", ""), Enums.RecieveProperty.MAC, request.Mac));
            return;
        }

        public void ChangeBatteryLevel(BatteryLevelRequest request)
        {
            string mac = BitConverter.ToString(request.Mac.ToByteArray());
            if(_Scooters.ContainsKey(BitConverter.ToString(request.Mac.ToByteArray())))
            {
                var scooter = _Scooters[mac];
                scooter.BatteryLevel = (byte)(request.BatteryLevel & 0xFF);

                if(scooter.RentalTime != 0)
                {
                    _EventHub.Clients.Group("User").SendAsync("UpdateScooter", new ScooterProperty(mac.Replace("-", ""), Enums.RecieveProperty.BateryLevel, request.BatteryLevel));
                }
                else
                {
                    _EventHub.Clients.Group("Manager").SendAsync("UpdateScooter", new ScooterProperty(mac.Replace("-", ""), Enums.RecieveProperty.BateryLevel, request.BatteryLevel));
                }
                return;
            }
        }

        public void ChangePosition(PositionRequest request)
        {
            string mac = BitConverter.ToString(request.Mac.ToByteArray());
            if (_Scooters.ContainsKey(BitConverter.ToString(request.Mac.ToByteArray())))
            {
                var scooter = _Scooters[mac];
                scooter.Position.Latitude = request.Latitude;
                scooter.Position.Longitude = request.Longitude;

                if (scooter.RentalTime != 0)
                {
                    _EventHub.Clients.Group("User").SendAsync("UpdateScooter", new ScooterProperty(mac.Replace("-", ""), Enums.RecieveProperty.Position, request.Latitude));
                }
                else
                {
                    _EventHub.Clients.Group("Manager").SendAsync("UpdateScooter", new ScooterProperty(mac.Replace("-", ""), Enums.RecieveProperty.Position, request.Latitude));
                }
                return;
            }
        }

        public void ChangeRentalTime(RentalTimeRequest request)
        {
            string mac = BitConverter.ToString(request.Mac.ToByteArray());
            if (_Scooters.ContainsKey(BitConverter.ToString(request.Mac.ToByteArray())))
            {
                var scooter = _Scooters[mac];
                scooter.RentalTime = (ushort)request.RentalTime;

                if (scooter.RentalTime != 0)
                {
                    _EventHub.Clients.Group("User").SendAsync("UpdateScooter", new ScooterProperty(mac.Replace("-", ""), Enums.RecieveProperty.RentalTime, request.RentalTime));
                }
                else
                {
                    _EventHub.Clients.Group("Manager").SendAsync("UpdateScooter", new ScooterProperty(mac.Replace("-", ""), Enums.RecieveProperty.RentalTime, request.RentalTime));
                }
                return;
            }
        }

        public void ChangeSpeed(SpeedRequest request)
        {
            string mac = BitConverter.ToString(request.Mac.ToByteArray());
            if (_Scooters.ContainsKey(BitConverter.ToString(request.Mac.ToByteArray())))
            {
                var scooter = _Scooters[mac];
                scooter.Speed = (sbyte)request.Speed;

                if (scooter.RentalTime != 0)
                {
                    _EventHub.Clients.Group("User").SendAsync("UpdateScooter", new ScooterProperty(mac.Replace("-", ""), Enums.RecieveProperty.Speed, request.Speed));
                }
                else
                {
                    _EventHub.Clients.Group("Manager").SendAsync("UpdateScooter", new ScooterProperty(mac.Replace("-", ""), Enums.RecieveProperty.Speed, request.Speed));
                }
                return;
            }
        }
    }
}
