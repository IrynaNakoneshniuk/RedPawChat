import React from 'react'
import ReactDOM from 'react-dom/client'

import RegistrationPage from './RegistrationPage.jsx'
import './index.css'
import axios from 'axios';
import LoginPage from './LoginPage.jsx';
import ChangePasswordPage from './ChangePasswordPage.jsx';
import App from './App.jsx';
import Test from './Test.jsx'
import Profile from './Profile.jsx'
import { Provider } from 'react-redux'
import { UserProvider } from './UserContext';

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
     <UserProvider>
     <App/>
     </UserProvider>
  </React.StrictMode>
);



