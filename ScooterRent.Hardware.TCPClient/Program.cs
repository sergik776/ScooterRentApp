// See https://aka.ms/new-console-template for more information
using ScooterRent.Hardware.HAL;
using ScooterRent.Hardware.TCPClient;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using static ScooterRent.Hardware.HAL.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

List<(PhysicalAddress, TcpClient)> scooters = new List<(PhysicalAddress, TcpClient)>();//Список самокатов (пока не реализован)

try
{
    TcpClient tcpClient = new TcpClient();//создаем ТСП клиента
    Console.WriteLine("Connect to 127.0.0.1:8888");//выводим
    await tcpClient.ConnectAsync("127.0.0.1", 8888);//Подлючаемся к серверу
    var mac = MyStaticGenerators.GenerateRandomMacAddress();//генерим мак
    scooters.Add((mac, tcpClient));//добавляем самокат в список
    Console.WriteLine("Подключение установлено");//выводим
    tcpClient.Client.Send(PropertyGenerator.NewMACPacket(mac));//отправляем на сервак свой мак
    var p = MyStaticGenerators.GenerateRandomPosition();//генерим начальную позицию
    PositionGenerate PG = new PositionGenerate(p.Latitude, p.Longitude);//создаем генератор пути
    //Заупскаем задачу с имитацией движения самоката
    Task.Factory.StartNew(() => { PG.MoveToPoint(scooters[MyStaticGenerators.R.Next(0, scooters.Count - 1)].Item2); });
    //Запускаем задачу с имитацией изменения параметров самоката
    while(true)
    {
        GetTypeEnum(MyStaticGenerators.GetRandomEnumValue<RecieveProperty>(), scooters[MyStaticGenerators.R.Next(0, scooters.Count-1)].Item2);
        Thread.Sleep(500);
    }
}
catch (SocketException ex)
{
    Console.WriteLine(ex.Message);
}


Console.ReadKey();



void GetTypeEnum(RecieveProperty pq, TcpClient c)
{
    switch (pq)
    {
        //case RecieveProperty.MAC: c.Client.Send(PropertyGenerator.NewMACPacket(scooters[MyStaticGenerators.R.Next(0, scooters.Count)])); break;

        //case RecieveProperty.Position: var p = MyStaticGenerators.GenerateRandomPosition(); 
          //  Console.WriteLine("Send Position"); c.Client.Send(PropertyGenerator.NewPositionPacket(p)); break;

        case RecieveProperty.BateryLevel: var p1 = PropertyGenerator.NewBateryLevelPacket((byte)MyStaticGenerators.R.Next(0, 100)); 
            Console.WriteLine("Send BateryLevel"); c.Client.Send(p1); break;

        case RecieveProperty.Speed: var p2 = PropertyGenerator.NewSpeedPacket((sbyte)MyStaticGenerators.R.Next(-3, 30));
            Console.WriteLine("Send Speed"); c.Client.Send(p2); break;

        case RecieveProperty.RentalTime: var p3 = PropertyGenerator.NewRentaltimePacket((ushort)MyStaticGenerators.R.Next(0, ushort.MaxValue));
            Console.WriteLine("Send State"); c.Client.Send(p3); break;

        default: break;
    }
}