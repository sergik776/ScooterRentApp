namespace ScooterRent.Hardware.HAL
{
    /// <summary>
    /// Класс хранящий перечисления
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Состояния самоката
        /// </summary>
        public enum ScooterState : byte
        {
            /// <summary>
            /// Не определенный (возможно взломан)
            /// </summary>
            Undefined,
            /// <summary>
            /// Заблокирован и стоит
            /// </summary>
            BlockAndStay,
            /// <summary>
            /// Разблокирован и едет
            /// </summary>
            UnlockAndRide,
            /// <summary>
            /// Разблокирован и стот
            /// </summary>
            UnlockAndStay
        }

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
            /// Зпрос айди
            /// </summary>
            GetId,
            /// <summary>
            /// Запрос уровня заряда батареи
            /// </summary>
            GetBateryLevel,
            /// <summary>
            /// Запрос состояния скутера
            /// </summary>
            GetState,
            /// <summary>
            /// Запрос текущей скорости
            /// </summary>
            GetSpeed,
            /// <summary>
            /// Запрос текущей позиции
            /// </summary>
            GetPosition,
            /// <summary>
            /// Блокировка самоката
            /// </summary>
            ToLock,
            /// <summary>
            /// Разблокировка самоката
            /// </summary>
            ToUnlock
        }
    }
}
