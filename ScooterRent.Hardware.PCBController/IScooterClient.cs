using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRent.Hardware.HAL
{
    /// <summary>
    /// Адресный интерфейс скутера
    /// </summary>
    internal interface IScooterClient
    {
        /// <summary>
        /// Айпи скутера
        /// </summary>
        EndPoint? IP { get; }
        /// <summary>
        /// Мак адрес скутера
        /// </summary>
        PhysicalAddress MAC { get; }
    }
}
