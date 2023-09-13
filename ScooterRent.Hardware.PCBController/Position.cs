namespace ScooterRent.Hardware.HAL
{
    /// <summary>
    /// Позиция самокана
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude { get;  set; }
        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="latitude">Широта</param>
        /// <param name="longtude">Долгота</param>
        public Position(double latitude, double longtude)
        {
            Latitude = latitude;
            Longitude = longtude;
        }

        public override string ToString()
        {
            return $"{Latitude}N {Longitude}E";
        }
    }
}
