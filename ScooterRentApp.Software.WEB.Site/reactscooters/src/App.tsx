import './App.css'
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import NewPage from './Views/NewPage';
import LoginPage from './Views/LoginPage';
import { AuthProvider } from './data/AuthContext';

function App() {
    return (
        <Router>
            <AuthProvider>
            <Routes>
                <Route path="/" element={<LoginPage />} />
                <Route path="/newpage" element={<NewPage />} />
                </Routes>
            </AuthProvider>
        </Router>
    );
}

export default App;
