// LoginPage.js
import React, { useState } from 'react';
import {Link} from 'react-router-dom';
import registrationImage from './assets/logo.svg';
import "./App.css";
import ErrorComponent from './ErrorComponent.jsx';
import { useNavigate } from 'react-router-dom';;

const LoginPage = () => {

  const [formData, setFormData] = useState({
    email: '',
    password: '',
  });

  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setFormData({
      ...formData,
      [name]:value,
    });
  };

  const handleLogin = async () => {
    try{
    const response = await fetch("https://localhost:5123/api/account/login", {
      method: "POST",
      credentials:'include',
      headers: {
        "Content-Type": "application/json",
        'Accept': 'application/json', 
      },
      body: JSON.stringify(formData),
    });

    if (response.ok) {

      var data=await response.json();

      console.log(data);
      navigate(`/getconversations/${data}`);
    }
    else{
      const errorText = await response.json();
      setError(`${errorText.error}`);

      console.error("Error :", errorText);
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
      </div>
      <p style={{ marginTop: '10px', textAlign: 'center', color: '#fff' }}>
       <span><Link to="/registration">Зареєструйтесь тут</Link>.</span> 
      </p>
      <p style={{ textAlign: 'center', color: '#fff' }}>
        Забули пароль? <Link to="/change-password">Скинути пароль</Link>.
        
      </p>
      <ErrorComponent error={error}/>
    </div>
  );
};

export default LoginPage;
