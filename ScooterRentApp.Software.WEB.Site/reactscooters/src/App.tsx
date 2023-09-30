import './App.css'
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import NewPage from './Views/NewPage';
import LoginPage from './Views/LoginPage';
import { AuthProvider } from './data/AuthContext';
import { SignalRProvider } from './data/SignalRContext'; // Импортируйте SignalRProvider

function App() {
    return (
        <Router>
            <AuthProvider>
                <SignalRProvider>
                    <Routes>
                        <Route path="/" element={<LoginPage />} />
                        <Route path="/newpage" element={<NewPage />} />
                    </Routes>
                </SignalRProvider>
            </AuthProvider>
        </Router>
    );
}

export default App;
