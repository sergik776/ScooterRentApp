using Grpc.Core;

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

        public override Task<ScooterResponse> SetScooter(ScooterRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"{request.Mac} {request.Position}");
            _service.Add(request);
            return new Task<ScooterResponse>(() => { return new ScooterResponse() { LastId = _service.Scooters.Count().ToString() }; });
        }

        public override Task<ScooterResponse> SetPropertyScooter(ScooterPropertyUpdateRequest request, ServerCallContext context)
        {
            _service.Update(request.Mac, request.PropertyName, request.PropertyValue);
            return new Task<ScooterResponse>(() => { return new ScooterResponse() { LastId = _service.Scooters.Count().ToString() }; });
        }
    }
}
