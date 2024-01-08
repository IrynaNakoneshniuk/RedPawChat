import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Profile from './Profile.jsx';
import LoginPage from './LoginPage.jsx';
import RegistrationPage from './RegistrationPage.jsx';
import Conversation from './Conversation.jsx';


const App = () => {
    return (
        <Router>
          <Routes>
            <Route  element={<LoginPage />} path="/"/>
            <Route  element={<Profile />} path="/getconversations/:id" />
            <Route element={<RegistrationPage/>} path="/registration"/>
            <Route element={<Conversation/>} path="/conversation"/>
          </Routes>
        </Router>
      );
};

export default App;


