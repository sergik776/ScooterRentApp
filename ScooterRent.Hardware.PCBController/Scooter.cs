using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static ScooterRent.Hardware.HAL.Enums;

namespace ScooterRent.Hardware.HAL
{
    /// <summary>
    /// Реализация скутера
    /// </summary>
    public class Scooter : AScooter
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="c">ТСП поток скутера</param>
        public Scooter(TcpClient c) : base(c)
        {
            base.Speed = 0;
            base.State = ScooterState.BlockAndStay;
            base.BatteryLevel = 100;
            base.Position = new Position(0, 0);
        }

        public override bool Lock()
        {
            throw new NotImplementedException();
        }

        public override bool Unlock()
        {
            throw new NotImplementedException();
        }
    }
}
