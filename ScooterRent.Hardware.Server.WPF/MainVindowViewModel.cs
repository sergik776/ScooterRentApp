using ScooterRent.Hardware.HAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Threading;

namespace ScooterRent.Hardware.Server.WPF
{
    class MainVindowViewModel : BaseViewModel
    {
        ScooterService scooterService;

        private ScooterMVVM _Scooter;
        /// <summary>
        /// Выбранный скутер
        /// </summary>
        public ScooterMVVM Scooter { get { return _Scooter; } set { _Scooter = value; OnPropertyChanged(nameof(Scooter)); } }

        /// <summary>
        /// Список скутеров подключенных
        /// </summary>
        private ObservableCollection<ScooterMVVM> _Scooters;
        public ObservableCollection<ScooterMVVM> Scooters { get { return _Scooters; } set { _Scooters = value; OnPropertyChanged(nameof(Scooters)); } }

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainVindowViewModel(ScooterService ss)
        {
            scooterService = ss;
            scooterService.PropertyChanged += ScooterService_PropertyChanged;
            Scooters = new ObservableCollection<ScooterMVVM>(scooterService.Scooters.Select(x=> (ScooterMVVM)x)); //Инициализация списка скутеров
            
        }

        private void ScooterService_PropertyChanged(System.Net.NetworkInformation.PhysicalAddress mac, Enums.RecieveProperty p)
        {
            var s = Scooters.FirstOrDefault(x => x.MAC == BitConverter.ToString(mac.GetAddressBytes()));
            Application.Current.Dispatcher.Invoke(() => { 
            if (s == null)
            {
                Scooters.Add(scooterService.GetById(mac));
            }
            else
            {
                // Обновляем свойства существующего скутера на основе scooterService
                var updatedScooter = scooterService.GetById(mac);
                switch(p)
                {
                    case Enums.RecieveProperty.BateryLevel: 
                        s.BatteryLevel = updatedScooter.BatteryLevel.ToString();
                        s.OnPropertyChanged(nameof(s.BatteryLevel));
                        break;
                    case Enums.RecieveProperty.State:
                        s.State = updatedScooter.State.ToString();
                        s.OnPropertyChanged(nameof(s.State));
                        break;
                    case Enums.RecieveProperty.Speed:
                        s.Speed = updatedScooter.Speed.ToString();
                        s.OnPropertyChanged(nameof(s.Speed));
                        break;
                    case Enums.RecieveProperty.Position:
                        s.Position.Latitude = updatedScooter.Position.Latitude;
                        s.OnPropertyChanged(nameof(s.Position.Latitude));
                        s.Position.Longitude = updatedScooter.Position.Longitude;
                        s.OnPropertyChanged(nameof(s.Position.Longitude));
                        s.OnPropertyChanged(nameof(s.Position));
                        break;
                    case Enums.RecieveProperty.MAC:
                        s.MAC = BitConverter.ToString(updatedScooter.MAC.GetAddressBytes());
                        s.OnPropertyChanged(nameof(s.MAC));
                        break;
                }
            }
            });
        }
    }

    public class BaseViewModel : INotifyPropertyChanged
    {
        protected Window currentWindow;

        public void BindingWindow(Window w)
        {
            currentWindow = w;
        }

        private RelayCommand _Close;
        public RelayCommand Close
        {
            get
            {
                return _Close ?? (_Close = new RelayCommand(async obj =>
                {
                    currentWindow.Close();
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
