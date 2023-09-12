using static ScooterRent.Hardware.HAL.Enums;

namespace ScooterRent.Hardware.HAL.HardwareProtocol
{
    /// <summary>
    /// Пакет на отправку команды
    /// </summary>
    public class SendDataPacket
    {
        /// <summary>
        /// Айди скутера
        /// </summary>
        public Guid ScooterId { get; private set; }
        /// <summary>
        /// Команда
        /// </summary>
        public SendCommand Command { get; private set; }
        /// <summary>
        /// Пакет данных
        /// </summary>
        public byte[] Data { get { return new byte[] { (byte)Command }; } }

        /// <summary>
        /// Конструктор пакета на отправку команды
        /// </summary>
        /// <param name="id">Айди скутера</param>
        /// <param name="cmd">Команда</param>
        public SendDataPacket(Guid id, SendCommand cmd)
        {
            this.ScooterId = id;
            this.Command = cmd;
        }
    }
}
