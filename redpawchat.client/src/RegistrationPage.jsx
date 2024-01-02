// RegistrationPage.js
import React, { useState, useEffect } from 'react';
import './App.css';
import registrationImage from './assets/logo.svg'
import ErrorComponent from './ErrorComponent';

const RegistrationPage = () => {
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    confirmPassword: '',
    firstName: '',
    lastName: '',
    middlename:'',
    nickname: '',
    photo: null,
  });

  const [error, setError] = useState('');

  const [emailValid, setEmailValid] = useState(false);
  const [passwordValid, setPasswordValid] = useState(false);
  const [confirmPasswordValid, setConfirmPasswordValid] = useState(false);

  useEffect(() => {
    // Валідація електронної адреси
    setEmailValid(/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email));

    // Валідація паролю 
    setPasswordValid(formData.password.length >= 6);

    // Перевірка на співпадіння паролів
    setConfirmPasswordValid(formData.password === formData.confirmPassword);
  }, [formData]);

  const handleInputChange = (e) => {
    const { name, value, type } = e.target;

    // Валідація фото
    if (name === 'photo' && type === 'file') {
      const file = e.target.files[0];
      setFormData((prevData) => ({
        ...prevData,
        [name]: file,
      }));
    } else {
      setFormData((prevData) => ({
        ...prevData,
        [name]: value,
      }));
    }

    if(!setEmailValid(/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email))){
      setError("Uncorrect format of email!")
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const formDataToSend = new FormData();
    formDataToSend.append('email', formData.email);
    formDataToSend.append('password', formData.password);
    formDataToSend.append('confirmPassword', formData.confirmPassword);
    formDataToSend.append('firstName', formData.firstName);
    formDataToSend.append('lastName', formData.lastName);
    formDataToSend.append('middlename', formData.middlename);
    formDataToSend.append('nickname', formData.nickname);
    formDataToSend.append('photo', formData.photo);
 

    try {
      const response = await fetch('https://localhost:5123/api/account/registration', {
        method: 'POST',
        credentials: 'include',
        body: formDataToSend,
      });

      console.log(response.statusText);
      if (!response.ok) {
        const errorText = JSON.parse(await response.text());
        setError(`${errorText.error[0].description}`);
      }
    } catch (error) {
      console.error('Error uploading photo:', error);
    }
  };
    
  return (
    <div className="container">
     <h3 style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', textAlign: 'center'}}>
        <img src={registrationImage} alt="Іконка реєстрації" style={{ width: '80px', height: 'auto', marginRight: '8px' }} />
        Реєстрація
      </h3>
      <p></p>
      <div  style={{ margin: 'auto',width: '80%', boxSizing: 'border-box', marginBottom: '30px'}}>
      <form onSubmit={handleSubmit}>
                    
                      <input
                        type="email"
                        name="email"
                        value={formData.email}
                        onChange={handleInputChange}
                        className={emailValid ? 'valid' : 'invalid'}
                        required
                        placeholder='Електронна пошта'
                      />
                      <input
                        type="password"
                        name="password"
                        value={formData.password}
                        onChange={handleInputChange}
                        className={passwordValid ? 'valid' : 'invalid'}
                        required
                        placeholder='Пароль'
                      />         
                      <input
                        type="password"
                        name="confirmPassword"
                        value={formData.confirmPassword}
                        onChange={handleInputChange}
                        className={confirmPasswordValid ? 'valid' : 'invalid'}
                        required
                        placeholder='Повторіть пароль'
                      />

                      <input
                        type="text"
                        name="firstName"
                        value={formData.firstName}
                        onChange={handleInputChange}
                        required
                        placeholder='Імя'
                      />
                      <input
                        type="text"
                        name="lastName"
                        value={formData.lastName}
                        onChange={handleInputChange}
                        required
                        placeholder='Прізвище'
                      />
                      <input
                        type="text"
                        name="middlename"
                        value={formData.middlename}
                        onChange={handleInputChange}
                        required
                        placeholder='По батькові'
                      />
                      <input
                        type="text"
                        name="nickname"
                        value={formData.nickname}
                        onChange={handleInputChange}
                        required
                        placeholder='Нік'
                      />
                      <input
                        type="file"
                        accept="image/*"
                        name="photo"
                        onChange={handleInputChange}
                        required
                        placeholder='Фото'
                      />
                    <button type="submit" disabled={!emailValid || !passwordValid || !confirmPasswordValid}>
                      Зареєструватися
                    </button>
                    
                  </form>
                  <ErrorComponent error={error}/>
      </div>
    </div>
  );
};

export default RegistrationPage;
