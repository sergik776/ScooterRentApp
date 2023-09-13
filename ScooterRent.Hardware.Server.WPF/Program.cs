using ScooterRent.Hardware.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRent.Hardware.Server.WPF
{
    internal class Program
    {
        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.9.0")]
        public static void Main()
        {
            ScooterRent.Hardware.Server.WPF.App app = new ScooterRent.Hardware.Server.WPF.App();
            
            ScooterService scooterService = new ScooterService();

            MainWindow MW = new MainWindow();
            MainVindowViewModel MWVM = new MainVindowViewModel(scooterService);
            MW.InitDataContext(MWVM);

            app.Run(MW);
        }
    }
}
