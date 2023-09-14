// See https://aka.ms/new-console-template for more information
using ScooterRent.Hardware.HAL;
using ScooterRent.Hardware.TCPClient;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using static ScooterRent.Hardware.HAL.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

FakeScooter F = new FakeScooter();

Console.ReadKey();