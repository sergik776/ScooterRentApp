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
        ScooterEventHub ScooterEventHub;

        public ScooterService(ILogger<ScooterService> logger, ScooterListService listService, ScooterEventHub hub) 
        {
            _service = listService;
            _logger = logger;
            ScooterEventHub = hub;
        }

        public override Task<ScooterResponse> SetScooter(ScooterRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"{request.Mac} {request.Position}");
            _service.Add(request);
            ScooterEventHub.SendEventConnect(request);
            return new Task<ScooterResponse>(() => { return new ScooterResponse() { LastId = _service.Scooters.Count().ToString() }; });
        }

        public override Task<ScooterResponse> SetPropertyScooter(ScooterPropertyUpdateRequest request, ServerCallContext context)
        {
            _service.Update(request.Mac, request.PropertyName, request.PropertyValue);
            ScooterEventHub.SendEventProperty(request);
            return new Task<ScooterResponse>(() => { return new ScooterResponse() { LastId = _service.Scooters.Count().ToString() }; });
        }
    }
}
