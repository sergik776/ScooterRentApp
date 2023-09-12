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
        private ScooterMVVM _Scooter;
        /// <summary>
        /// Выбранный скутер
        /// </summary>
        public ScooterMVVM Scooter { get { return _Scooter; } set { _Scooter = value; OnPropertyChanged(nameof(Scooter)); } }

        private ObservableCollection<ScooterMVVM> _Scooters;
        /// <summary>
        /// Список скутеров подключенных
        /// </summary>
        public ObservableCollection<ScooterMVVM> Scooters { get { return _Scooters; } set { _Scooters = value; OnPropertyChanged(nameof(Scooters)); } }

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainVindowViewModel()
        {
            Scooters = new ObservableCollection<ScooterMVVM>(); //Инициализация списка скутеров
            var tcpListener = new TcpListener(IPAddress.Any, 8888); //Инициализация ТСП сервера
            tcpListener.Start(); //Запуск сервера
            Task.Factory.StartNew(() => { //Запуск прослушивания в отдельном потоке что бы освободить конструктор
                
                try
                {
                    while (true)//Ожидание скутера
                    {
                        try
                        {
                            var tcpClient = tcpListener.AcceptTcpClientAsync().Result; //Подключаение скутера
                            Scooters.Add(new ScooterMVVM(tcpClient)); //Добавление скутера в список
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                finally
                {
                    tcpListener.Stop(); // останавливаем сервер
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
