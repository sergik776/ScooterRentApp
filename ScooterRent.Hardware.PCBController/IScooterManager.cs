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
    internal interface IScooterManager : IBaseScooter
    {
        /// <summary>
        /// Метод блокировки самоката
        /// </summary>
        /// <returns></returns>
        abstract bool Lock();
        /// <summary>
        /// Метод разблокировки самоката
        /// </summary>
        /// <returns></returns>
        abstract bool Unlock();
    }
}
