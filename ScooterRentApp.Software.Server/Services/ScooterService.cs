using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScooterRent.Software.Server.Hubs;
using System.Data;

namespace ScooterRentApp.Software.Server.Services
{
    public class ScooterService : ScooterGRPCService.ScooterGRPCServiceBase
    {
        private readonly ILogger<ScooterService> _logger;
        ScooterListService _service;

        public ScooterService(ILogger<ScooterService> logger, ScooterListService listService) 
        {
            _service = listService;
            _logger = logger;
        }

        public override async Task<ScooterResponse> AddScooter(MacRequest request, ServerCallContext context)
        {
            _service.AddScooter(request);
            return new ScooterResponse() { LastId = _service.Scooters.Count().ToString() };
        }

        public override async Task<ScooterResponse> ChangeBatteryLevel(BatteryLevelRequest request, ServerCallContext context)
        {
            _service.ChangeBatteryLevel(request);
            return new ScooterResponse() { LastId = _service.Scooters.Count().ToString() };
        }

        public override async Task<ScooterResponse> ChangePosition(PositionRequest request, ServerCallContext context)
        {
            _service.ChangePosition(request);
            return new ScooterResponse() { LastId = _service.Scooters.Count().ToString() };
        }

        public override async Task<ScooterResponse> ChangeRentalTime(RentalTimeRequest request, ServerCallContext context)
        {
            _service.ChangeRentalTime(request);
            return new ScooterResponse() { LastId = _service.Scooters.Count().ToString() };
        }

        public override async Task<ScooterResponse> ChangeSpeed(SpeedRequest request, ServerCallContext context)
        {
            _service.ChangeSpeed(request);
            return new ScooterResponse() { LastId = _service.Scooters.Count().ToString() };
        }
    }
}
