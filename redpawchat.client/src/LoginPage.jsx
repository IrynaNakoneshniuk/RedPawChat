// LoginPage.js
import React, { useState } from 'react';
import {Link} from 'react-router-dom';
import registrationImage from './assets/logo.svg';

import './App.css';

const LoginPage = () => {

  const [formData, setFormData] = useState({
    email: '',
    password: '',
    rememberMe: false,

  });

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setFormData({
      ...formData,
      [name]:value,
    });
  };

  const handleLogin = async () => {
    try{
    const response = await fetch("http://localhost:5123/api/account/Login", {
      method: "POST", 
      mode: "cors", 
      cache: "no-cache", 
      credentials: "same-origin", 
      headers: {
        "Content-Type": "application/json",
        'Accept': 'application/json', 
      },
      redirect: "follow", 
      referrerPolicy: "no-referrer", 
      body: JSON.stringify(formData), 
    });
    console.log(response.statusText);
    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }
  } catch(error) {

    console.error("Error parsing JSON:", error);
  }
  };

  return (
    <div className="container">
      <h2 style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', textAlign: 'center' }}>
        <img src={registrationImage} alt="Іконка реєстрації" style={{ width: '80px', height: 'auto', marginRight: '8px' }} />
        Вхід
      </h2>
      <input
        type="text"
        name='email'
        placeholder=" Логін"
        value={formData.email}
        onChange={handleInputChange}
      />
      <input
        type="password"
        name="password"
        placeholder="Пароль"
        value={formData.password}
        onChange={handleInputChange}
      />
      <button onClick={handleLogin} formMethod='POST'>Увійти</button>
      <div style={{ display: 'grid', gridTemplateColumns: 'auto auto', gap: '8px'}}>
      <label>Запам'ятати мене</label>
      <input 
          type="checkbox"
          name="rememberMe"
          checked={formData.rememberMe}
          onChange={handleInputChange}
      />
      </div>
      <p style={{ marginTop: '10px', textAlign: 'center', color: '#fff' }}>
       <span><Link to="/registration">Зареєструйтесь тут</Link>.</span> 
      </p>
      <p style={{ textAlign: 'center', color: '#fff' }}>
        Забули пароль? <Link to="/change-password">Скинути пароль</Link>.
      </p>
      
    </div>
  );
};

export default LoginPage;
