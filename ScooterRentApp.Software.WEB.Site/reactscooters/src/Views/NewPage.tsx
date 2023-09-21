import axios from 'axios';
import { useEffect, useState } from 'react';
import { useAuth } from '../data/AuthContext';
import { useNavigate } from 'react-router-dom';


function NewPage() {
    const [scooters, setScooters] = useState<Scooter[]>([]);
    const { tokens, refreshAccessToken } = useAuth();
    const navigate = useNavigate();
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
                        <tr key={index}>
                            <td>{scooter.mac}</td>
                            <td>{scooter.position}</td>
                            <td>{scooter.speed}</td>
                            <td>{scooter.rentalTime}</td>
                            <td>{scooter.batteryLevel}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

export default NewPage;