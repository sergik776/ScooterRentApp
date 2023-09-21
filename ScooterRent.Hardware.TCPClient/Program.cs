// See https://aka.ms/new-console-template for more information
using ScooterRent.Hardware.HAL;
using ScooterRent.Hardware.TCPClient;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using static ScooterRent.Hardware.HAL.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

List<FakeScooter> scooters = new List<FakeScooter>();
Thread.Sleep(5000);
for(int i = 0; i < 10; i++)
{
    FakeScooter F = new FakeScooter();
    Thread.Sleep(2000);
    scooters.Add(F);
}

Console.ReadKey();