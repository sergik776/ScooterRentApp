using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ScooterRent.Hardware.HAL.Enums;

namespace ScooterRent.Hardware.HAL
{
    /// <summary>
    /// Интерфейс управления самокатом
    /// </summary>
    public interface IScooterManager : IBaseScooter
    {
        void SetRentalTime(ushort seconds);
    }
}
