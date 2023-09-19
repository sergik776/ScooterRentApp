using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRentApp.Hardware.Server.HardwareProtocol.Commands
{
    internal class SetRentalTime
    {
        public byte[] MAC { get; set; }
        public ushort Seconds { get; set; }

        public SetRentalTime(byte[] MAC, ushort Seconds)
        {
            this.MAC = MAC;
            this.Seconds = Seconds;
        }

        public static SetRentalTime GetTime(byte[] data)
        {
            if (data[0] == 0xFF)
            {
                return new SetRentalTime(data.Skip(1).SkipLast(2).ToArray(), BitConverter.ToUInt16(data.Skip(7).ToArray()));
            }
            throw new Exception("Не могу конвертировать");
        }

        public byte[] GetDataBytes()
        {
            return new byte[] { 0xFF }.Concat(MAC).Concat(BitConverter.GetBytes(Seconds)).ToArray();
        }
    }
}
