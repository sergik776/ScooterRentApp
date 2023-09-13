using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScooterRent.Hardware.Server.WPF
{
    internal static class Extentions
    {
        public static void InitDataContext(this Window w, BaseViewModel context)
        {
            w.DataContext = context;
            context.BindingWindow(w);
        }
    }
}
