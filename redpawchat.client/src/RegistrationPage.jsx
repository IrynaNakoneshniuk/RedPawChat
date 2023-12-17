// RegistrationPage.js
import React, { useState } from 'react';
import './App.css';
import registrationImage from './assets/logo.svg'

const RegistrationPage = () => {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [middleName, setMiddleName] = useState('');
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [email, setEmail] = useState('');
  const [nickname, setNickname] = useState('');

  const handleRegister = async () => {
    // Викликати API для реєстрації користувача зі введеними даними
    console.log('Реєстрація: ', { firstName, lastName, middleName, username, password, email, nickname });
  };

  return (
    <div className="container">
     <h2 style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', textAlign: 'center' }}>
        <img src={registrationImage} alt="Іконка реєстрації" style={{ width: '80px', height: 'auto', marginRight: '8px' }} />
        Реєстрація
      </h2>
      <div  style={{ margin: 'auto',width: '80%', padding: '10px', boxSizing: 'border-box', marginBottom: '10px'}}>
      <input type="text" placeholder="Ім'я" value={firstName} onChange={(e) => setFirstName(e.target.value)} />
      <input type="text" placeholder="Прізвище" value={lastName} onChange={(e) => setLastName(e.target.value)} />
      <input type="text" placeholder="По батькові" value={middleName} onChange={(e) => setMiddleName(e.target.value)} />
      <input type="text" placeholder="Логін" value={username} onChange={(e) => setUsername(e.target.value)} />
      <input type="password" placeholder="Пароль" value={password} onChange={(e) => setPassword(e.target.value)} />
      <input type="email" placeholder="Емейл" value={email} onChange={(e) => setEmail(e.target.value)} />
      <input type="text" placeholder="Нік" value={nickname} onChange={(e) => setNickname(e.target.value)} />
      <button onClick={handleRegister}>Зареєструватися</button>
      </div>
    </div>
  );
};

export default RegistrationPage;
