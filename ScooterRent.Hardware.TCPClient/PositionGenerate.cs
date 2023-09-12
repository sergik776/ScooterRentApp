using ScooterRent.Hardware.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRent.Hardware.TCPClient
{
    /// <summary>
    /// Класс генерации шагов позиции, унаследован от бащового
    /// </summary>
    internal class PositionGenerate : Position
    {
        Random random; //Рандомайзер
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="latitude">Стартовая широта</param>
        /// <param name="longtude">Стартовая долгота</param>
        public PositionGenerate(double latitude, double longtude) : base(latitude, longtude)
        {
            random = new Random();//Инициализация
        }

        /// <summary>
        /// Метод передвижения по точкам
        /// </summary>
        /// <param name="client">ТСП поток от самоката</param>
        public void MoveToPoint(TcpClient client)
        {
            var lastPoint = MyStaticGenerators.GenerateRandomPosition();//Генерируем конечную точку
            //Генерируем список точкет для достижения последней
            var list = GenerateCoordinates(this.Latitude, this.Longitude, lastPoint.Latitude, lastPoint.Longitude);
            //Перемещаемся по каждой точке из списка и сообщаем об этом серверу
            foreach (var a in list)
            {
                this.Latitude = a.Item1;
                this.Longitude = a.Item2;
                client.Client.Send(PropertyGenerator.NewPositionPacket(new Position(a.Item1, a.Item2)));
                Thread.Sleep(200);//Ждем чуть чуть
            }
            MoveToPoint(client);//Когда пришли к последней точке, запускаем процесс заново
        }

        public void MoveRandomRecursive(TcpClient client)
        {
            // Генерируем случайное направление движения в радианах от 0 до 2π
            double direction = random.NextDouble() * 2 * Math.PI;

            // Вычисляем новые координаты на основе направления и длины шага (1 единица)
            double newLatitude = Latitude + Math.Sin(direction);
            double newLongitude = Longitude + Math.Cos(direction);

            // Применяем новые координаты
            base.Latitude = newLatitude;
            base.Longitude = newLongitude;
            client.Client.Send(PropertyGenerator.NewPositionPacket(this));
            Thread.Sleep(250);
            // Рекурсивно вызываем функцию для следующего шага
            MoveRandomRecursive(client);
        }

        /// <summary>
        /// Метод генерации точек от позиции А до позиции Б
        /// </summary>
        /// <param name="startX">Стартовая широта</param>
        /// <param name="startY">Стартовая долгота</param>
        /// <param name="endX">Конечная широта</param>
        /// <param name="endY">Конечная долгота</param>
        /// <returns></returns>
        List<Tuple<double, double>> GenerateCoordinates(double startX, double startY, double endX, double endY)
        {
            List<Tuple<double, double>> coordinates = new List<Tuple<double, double>>();//Список точек

            double currentX = startX;
            double currentY = startY;

            double stepX = Math.Sign(endX - startX); // Определяем направление по X
            double stepY = Math.Sign(endY - startY); // Определяем направление по Y

            while (currentX != endX || currentY != endY)
            {
                coordinates.Add(new Tuple<double, double>(currentX, currentY));

                // Перемещаемся на следующий шаг в соответствии с направлением
                if (currentX != endX)
                {
                    currentX += stepX;
                }

                if (currentY != endY)
                {
                    currentY += stepY;
                }
            }

            coordinates.Add(new Tuple<double, double>(endX, endY)); // Добавляем конечную точку

            return coordinates;
        }
    }
}
