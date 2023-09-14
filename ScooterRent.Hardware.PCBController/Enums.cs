namespace ScooterRent.Hardware.HAL
{
    /// <summary>
    /// Класс хранящий перечисления
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Тип полученного свойства самоката
        /// </summary>
        public enum RecieveProperty : byte
        {
            /// <summary>
            /// Заряд батареи
            /// </summary>
            BateryLevel,
            /// <summary>
            /// Состояние скутера
            /// </summary>
            RentalTime,
            /// <summary>
            /// Текущая скорость
            /// </summary>
            Speed,
            /// <summary>
            /// Текущая позиция
            /// </summary>
            Position,
            /// <summary>
            /// MAC адрес
            /// </summary>
            MAC
        }

        /// <summary>
        /// Команды самоката
        /// </summary>
        public enum SendCommand : byte
        {
            /// <summary>
            /// Устанавливает время аренды и разблокирует самокат
            /// </summary>
            SetRentTime = 36
        }
    }
}
