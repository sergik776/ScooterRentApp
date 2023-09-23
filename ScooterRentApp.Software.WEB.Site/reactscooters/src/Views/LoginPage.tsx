import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../data/AuthContext';

function LoginPage() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const navigate = useNavigate();
    const { setTokens } = useAuth();
    async function sendData() {
        const response = await fetch("http://localhost:8080/realms/TaogarSmartCloud/protocol/openid-connect/token", {
            method: "POST",
            headers: {
                "Content-Type": "application/x-www-form-urlencoded",
            },
            body: `client_id=ScooterAuth&grant_type=password&username=${username}&password=${password}`,
        });
        console.log(response);
        const tokenData = await response.json();
        console.log(tokenData);
        setTokens({
            access_token: tokenData.access_token,
            refresh_token: tokenData.refresh_token
        });
        
        navigate('/newpage');
    }

    return (
        <>
            <h1>Taogar Scooter Rent Service</h1>
            <h2>Login page</h2>
            <div>
                <a>Username</a>
                <input
                    className='custom-input'
                    type='text'
                    value={username}
                    onChange={e => setUsername(e.target.value)}
                />
            </div>
            <div>
                <a>Password</a>
                <input
                    className='custom-input'
                    type='password'
                    value={password}
                    onChange={e => setPassword(e.target.value)}
                />
            </div>
            <div>
                <button onClick={sendData}>Enter</button>
            </div>
        </>
    );
}

export default LoginPage;
