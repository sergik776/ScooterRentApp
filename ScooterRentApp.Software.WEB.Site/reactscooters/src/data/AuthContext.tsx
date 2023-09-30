import React, { createContext, useContext, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

interface AuthContextType {
    tokens: Tokens;
    setTokens: (tokens: Tokens) => void;
    refreshAccessToken: () => Promise<void>;
}

interface Tokens {
    access_token: string;
    refresh_token: string;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
    console.log('AuthContextProvider rendered');
    const [tokens, setTokens] = useState<Tokens>(() => {
        const storedTokens = localStorage.getItem('tokens');
        return storedTokens ? JSON.parse(storedTokens) : { access_token: '', refresh_token: '' };
    });
    const navigate = useNavigate();
    const refreshAccessToken = async () => {
        try {
            const response = await fetch("http://localhost:8080/realms/TaogarSmartCloud/protocol/openid-connect/token", {
                method: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded",
                },
                body: `client_id=ScooterAuth&grant_type=refresh_token&refresh_token=${tokens.refresh_token}`,
            });
            if (response.status === 200) {
                console.log('got new tokens by refresh')
                const tokenData = await response.json();
                setTokens({
                    access_token: tokenData.access_token,
                    refresh_token: tokenData.refresh_token,
                });
                console.log('tokens setted');
            } else {
                throw new Error('Failed to refresh token');
            }
        } catch (error) {
            console.error('ne mogu obnpvit token:', error);
            navigate('/');
        }
    };

    useEffect(() => {
        localStorage.setItem('tokens', JSON.stringify(tokens));
    }, [tokens]);

    return (
        <AuthContext.Provider value={{ tokens, setTokens, refreshAccessToken }}>
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
}
