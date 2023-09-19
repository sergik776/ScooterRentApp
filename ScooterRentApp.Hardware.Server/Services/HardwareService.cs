using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRentApp.Hardware.Server.Services
{
    internal class HardwareService : HardwareProtocolService.HardwareProtocolServiceBase
    {
        Action<string, ushort> SetRentalTime;

        public HardwareService(Action<string, ushort> setRentalTime)
        {
            SetRentalTime = setRentalTime;
        }

        public override Task<ScooterResponse1> SetRentalTime1(SetRentalTimeRequest1 request, ServerCallContext context)
        {
            ushort sec;
            ushort.TryParse(request.Seconds, out sec);
            SetRentalTime.Invoke(request.Mac, sec);
            return new Task<ScooterResponse1>(() => { return new ScooterResponse1() { LastId = "1" }; });
        }
    }
}
