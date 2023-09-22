import axios from 'axios';
import { useEffect, useState } from 'react';
import { useAuth } from '../data/AuthContext';
import { useNavigate } from 'react-router-dom';
import * as signalR from '@microsoft/signalr';


function NewPage() {
    const [scooters, setScooters] = useState<Scooter[]>([]);
    const { tokens, refreshAccessToken } = useAuth();
    const navigate = useNavigate();
    const [selectedRow, setSelectedRow] = useState<number | null>(null);
    const [rentSeconds, setRentSeconds] = useState<number>(0);

    const [hubConnection, setHubConnection] = useState<signalR.HubConnection | null>(null);

    useEffect(() => {
        // Создаем SignalR подключение при монтировании компонента
        const newHubConnection = new signalR.HubConnectionBuilder()
            .withUrl("http://192.168.2.200:5272/ScooterHub",
            {
                accessTokenFactory: () => tokens.access_token
            }) // Замените на свой URL хаба
            .build();

        // Настраиваем обработчики событий от сервера SignalR
        newHubConnection.on("ScooterPropercyChanged", (message: string) => {
            console.log(`ScooterPropercyChanged ${message}`);
            // Добавьте обработку событий от сервера, как вам необходимо
        });

        newHubConnection.on("ScooterConnected", (message: string) => {
            console.log(`ScooterConnected ${message}`);
            // Добавьте обработку событий от сервера, как вам необходимо
        });

        // Запускаем SignalR подключение
        newHubConnection.start()
            .then(() => {
                console.log('SignalR connection started successfully.');
                setHubConnection(newHubConnection);
            })
            .catch(error => console.error('Error starting SignalR connection:', error));

        return () => {
            // Отключаем SignalR подключение при размонтировании компонента
            if (hubConnection) {
                hubConnection.stop().then(() => console.log('SignalR connection stopped.'));
            }
        };
    }, []);

    const handleRowClick = (index: number) => {
        if (index === selectedRow) {
            // Если да, снимаем выделение
            setSelectedRow(null);
        } else {
            // Если нет, устанавливаем данную строку как выбранную
            setSelectedRow(index);
        }
        console.log(index);
    };

    const handleRentClick = async () => {
        if (selectedRow !== null && rentSeconds > 0) {
            try {
                const response = await axios.post(
                    'https://localhost:7018/Scooter',
                    {
                        mac: scooters[selectedRow].mac,
                        seconds: rentSeconds,
                    },
                    {
                        headers: {
                            Accept: 'application/json',
                            Authorization: `Bearer ${tokens.access_token}`,
                        },
                    }
                );

                if (response.status === 200) {
                    // Аренда успешно создана, можно добавить дополнительную логику или обновление данных
                    console.log('Аренда успешно создана.');
                } else {
                    console.log('Не удалось создать аренду.');
                }
            } catch (error: any) {
                if (error.response.status === 401) {
                    refreshAccessToken();
                    handleRentClick();
                    return;
                } else {
                    navigate('/');
                }
            }
        }
    };

    useEffect(() => {
        async function fetchData() {
            try {
                const response = await axios.get('https://localhost:7018/Scooter', {
                    headers: {
                        Accept: 'application/json',
                        Authorization: `Bearer ${tokens.access_token}`,
                    },
                });

                if (response.status === 200) {
                    const data = response.data as Scooter[];
                    setScooters(data);
                } else {
                    console.log('Scooter response != 200');
                }
            } catch (error: any) {
                if (error.response.status === 401) {
                    refreshAccessToken();
                    window.location.reload();
                    return;
                }
                else {
                    navigate('/');
                }
            }
        }
        fetchData();
    }, [tokens.access_token]);

    return (
        <div>
            <h1>Scooters</h1>
            <table>
                <thead>
                    <tr>
                        <th>Mac</th>
                        <th>Position</th>
                        <th>Speed</th>
                        <th>Rental Time</th>
                        <th>Battery Level</th>
                    </tr>
                </thead>
                <tbody>
                    {scooters.map((scooter, index) => (
                        <tr
                            key={index}
                            onClick={() => handleRowClick(index)}
                            className={`
                                ${index === selectedRow ? 'selected-row' : ''}
                                ${index === selectedRow && selectedRow !== null ? 'clicked-row' : ''}
                            `}
                        >
                            <td>{scooter.mac}</td>
                            <td>{scooter.position}</td>
                            <td>{scooter.speed}</td>
                            <td>{scooter.rentalTime}</td>
                            <td>{scooter.batteryLevel}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
            <div>
                <input
                    type="number"
                    placeholder="Секунды аренды"
                    value={rentSeconds}
                    onChange={(e) => setRentSeconds(Number(e.target.value))}
                />
                <button onClick={handleRentClick}>Аренда</button>
            </div>
        </div>
    );
}

export default NewPage;