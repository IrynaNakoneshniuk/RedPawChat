import React from 'react'
import ReactDOM from 'react-dom/client'

import AuthForm from './AuthForm.jsx'
import './index.css'
import axios from 'axios';

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <AuthForm/>
  </React.StrictMode>,
)

