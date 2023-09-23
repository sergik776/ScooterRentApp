import React, { createContext, useContext, useEffect, useState, ReactNode } from 'react';
import * as signalR from '@microsoft/signalr';
import { useAuth } from '../data/AuthContext';

// Создайте контекст
export const SignalRContext = createContext<signalR.HubConnection | null>(null);

// Создайте компонент-поставщик, который будет управлять SignalR-подключением
export const SignalRProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [hubConnection, setHubConnection] = useState<signalR.HubConnection | null>(null);
  const { tokens } = useAuth();

  useEffect(() => {
    // Создайте и настройте SignalR-подключение
    const newHubConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://192.168.2.200:5272/ScooterHub", {
        accessTokenFactory: () => tokens.access_token,
      })
      .build();

    // Настройте обработчики событий

    newHubConnection.on("AddScooter", (message: string) => {
      console.log(`AddScooter ${message}`);
      // Добавьте обработку событий от сервера, как вам необходимо
    });

    newHubConnection.on("UpdateScooter", (message: string) => {
      console.log(`UpdateScooter ${message}`);
      // Добавьте обработку событий от сервера, как вам необходимо
    });

    // Запустите SignalR-подключение
    newHubConnection
      .start()
      .then(() => {
        console.log('SignalR connection started successfully.');
        setHubConnection(newHubConnection);
      })
      .catch(error => console.error('Error starting SignalR connection:', error));

    return () => {
      // Отключите SignalR-подключение при размонтировании компонента
      if (hubConnection) {
        hubConnection.stop().then(() => console.log('SignalR connection stopped.'));
      }
    };
  }, []);

  return (
    // Оберните дочерние компоненты в контекст провайдера
    <SignalRContext.Provider value={hubConnection}>
      {children}
    </SignalRContext.Provider>
  );
};
