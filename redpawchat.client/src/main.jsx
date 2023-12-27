import React from 'react'
import ReactDOM from 'react-dom/client'

import RegistrationPage from './RegistrationPage.jsx'
import './index.css'
import axios from 'axios';
import LoginPage from './LoginPage.jsx';
import ChangePasswordPage from './ChangePasswordPage.jsx';
import { BrowserRouter as Router } from 'react-router-dom';
import App from './App.jsx';
import Test from './Test.jsx'

ReactDOM.createRoot(document.getElementById('root')).render(
  <Router>
    <RegistrationPage/>
  </Router>,)

