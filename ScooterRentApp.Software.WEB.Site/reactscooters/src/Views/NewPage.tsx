import axios from 'axios';
import { useContext, useEffect, useState } from 'react';
import { useAuth } from '../data/AuthContext';
import { useNavigate } from 'react-router-dom';
import { SignalRContext } from '../data/SignalRContext';


function NewPage() {
    const [scooters, setScooters] = useState<Scooter[]>([]);
    const { tokens, refreshAccessToken } = useAuth();
    const navigate = useNavigate();
    const [selectedRow, setSelectedRow] = useState<number | null>(null);
    const [rentSeconds, setRentSeconds] = useState<number>(0);

    const hubConnection = useContext(SignalRContext);

    useEffect(()=>{
        hubConnection?.on("AddScooter", (obj: ScooterProperty) => {
            console.log(`AddScooter ${JSON.stringify(obj)}`);
          });
          hubConnection?.on("UpdateScooter", (obj: ScooterProperty) => {
            //console.log(`UpdateScooter ${JSON.stringify(obj)}`);
            const scooterIndex = scooters.findIndex((scooter) => scooter.mac == obj.mac);
            console.log(scooters.find(x=>x.mac === obj.mac));
            console.log(obj.mac + " intdex " + scooterIndex);
             if(scooterIndex !== -1) 
             {
                // Создаем копию массива scooters
                const updatedScooters = [...scooters];

                // Обновляем свойство в найденном элементе
                switch (obj.property) {
                case PropertyTypes.Position:
                    updatedScooters[scooterIndex].position.latitude = obj.value;
                    break;
                    case PropertyTypes.RentalTime:
                    updatedScooters[scooterIndex].rentalTime = obj.value;
                    break;
                    case PropertyTypes.BateryLevel:
                    updatedScooters[scooterIndex].batteryLevel = obj.value;
                    break;
                    case PropertyTypes.Speed:
                    updatedScooters[scooterIndex].speed = obj.value;
                    break;
                // Добавьте другие свойства для обновления по необходимости
                }

                // Обновляем состояние массива scooters
                setScooters(updatedScooters);
            }});
          return () => {
            
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
                        <th>Position N</th>
                        <th>Position E</th>
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
                            <td>{scooter.position.latitude}</td>
                            <td>{scooter.position.longitude}</td>
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