import { useAuth } from '../data/AuthContext';
import React, { createContext, useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';
import { useLocation } from 'react-router-dom';

type SignalRProviderProps = {
  children: React.ReactNode;
};

// Создайте контекст
export const SignalRContext = createContext<signalR.HubConnection | null>(null);

// Создайте компонент-поставщик, который будет управлять SignalR-подключением
export const SignalRProvider: React.FC<SignalRProviderProps> = ({ children }) => {
  console.log('SignalRContext render');
  const { tokens } = useAuth();
  const [hubConnection, setHubConnection] = useState<signalR.HubConnection>(()=>{ return  new signalR.HubConnectionBuilder()
    .withUrl("http://192.168.2.200:5272/ScooterHub", {
      accessTokenFactory: () => tokens.access_token,
    })
    .build();});

  const location = useLocation();

  useEffect(() => {
    // Создание объекта SignalR и подписка на события
      console.log('Инициализация');
    hubConnection.on("AddScooter", (obj: ScooterProperty) => {
      console.log(`AddScooter ${JSON.stringify(obj)}`);
    });
    hubConnection.on("UpdateScooter", (obj: ScooterProperty) => {
      console.log(`UpdateScooter ${JSON.stringify(obj)}`);
    });
    console.log('Подписка завершена');
    console.log(hubConnection?.state);
    setHubConnection(hubConnection);
    // Отключение SignalR-подключения при размонтировании компонента
    return () => {
      if (hubConnection.state === 'Connected') {
        hubConnection.stop().then(() => console.log('SignalR отключен при инициализации ремонтировании компонента'));
      }
    };
  }, []);

  useEffect(() => {
    console.log('Навигация');
    // Вызываем эту функцию при каждом изменении маршрута
    console.log('Адрес текущей страницы:', location.pathname);
    if(location.pathname == "/")
    {
      hubConnection?.stop();
      console.log('Останавливаю сигналР');
      console.log(hubConnection?.state);
    }
    else
    {
      console.log(hubConnection?.state);
      if(hubConnection?.state == 'Disconnected')
      {
        console.log('Запускаю сигналР');
      hubConnection
      .start()
      .then(() => {
        console.log('SignalR запущен при навигации');
        setHubConnection(hubConnection);
      })
      .catch(error => console.error('Ошиюка запуска сигналР при навигации:', error));
      }
    return () => {
      // Отключите SignalR-подключение при размонтировании компонента
      // if (hubConnection) {
      //   hubConnection.stop().then(() => console.log('СигналР остановлен при ремонтировании компонента'));
      // }
    };
    }
  }, [location.pathname]);
  return (
    <SignalRContext.Provider value={hubConnection}>
      {children}
    </SignalRContext.Provider>
  );
};

type ScooterProperty = {
  Mac : string,
  PropertyType : PropertyTypes,
  Value : any
}

enum PropertyTypes
{
  BateryLevel,
  RentalTime,
  Speed,
  Position,
  MAC
}