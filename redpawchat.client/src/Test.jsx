import React, { useState, useEffect } from 'react';
import registrationImage from './assets/logo.svg';

import './App.css';

const Test = () => {
    const [formData, setFormData] = useState({
      email: '',
      password: '',
      confirmPassword: '',
      firstName: '',
      lastName: '',
      nickname: '',
      photo: null,
    });
  
    const [emailValid, setEmailValid] = useState(false);
    const [passwordValid, setPasswordValid] = useState(false);
    const [confirmPasswordValid, setConfirmPasswordValid] = useState(false);
  
    useEffect(() => {
      // Валідація електронної адреси
      setEmailValid(/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email));
  
      // Валідація паролю (припустимо, що він повинен мати мінімум 6 символів)
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
    };
  
    const handleSubmit = async (e) => {
      e.preventDefault();
  
      const formDataToSend = new FormData();
      formDataToSend.append('email', formData.email);
      formDataToSend.append('password', formData.password);
      formDataToSend.append('confirmPassword', formData.confirmPassword);
      formDataToSend.append('firstName', formData.firstName);
      formDataToSend.append('lastName', formData.lastName);
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
          throw new Error(`HTTP error! Status: ${response.status}`);
        }
      } catch (error) {
        console.error('Error uploading photo:', error);
      }
    };
      
        return (
                <div className="registration-container">
                  <h1>Реєстрація</h1>
                  <form onSubmit={handleSubmit}>
                    <label>
                      Електронна пошта:
                      <input
                        type="email"
                        name="email"
                        value={formData.email}
                        onChange={handleInputChange}
                        className={emailValid ? 'valid' : 'invalid'}
                        required
                      />
                    </label>
            
                    <label>
                      Пароль:
                      <input
                        type="password"
                        name="password"
                        value={formData.password}
                        onChange={handleInputChange}
                        className={passwordValid ? 'valid' : 'invalid'}
                        required
                      />
                    </label>
            
                    <label>
                      Підтвердження паролю:
                      <input
                        type="password"
                        name="confirmPassword"
                        value={formData.confirmPassword}
                        onChange={handleInputChange}
                        className={confirmPasswordValid ? 'valid' : 'invalid'}
                        required
                      />
                    </label>
            
                    <label>
                      Ім'я:
                      <input
                        type="text"
                        name="firstName"
                        value={formData.firstName}
                        onChange={handleInputChange}
                        required
                      />
                    </label>
            
                    <label>
                      Прізвище:
                      <input
                        type="text"
                        name="lastName"
                        value={formData.lastName}
                        onChange={handleInputChange}
                        required
                      />
                    </label>
            
                    <label>
                      Нік:
                      <input
                        type="text"
                        name="nickname"
                        value={formData.nickname}
                        onChange={handleInputChange}
                        required
                      />
                    </label>
            
                    <label>
                      Фото користувача:
                      <input
                        type="file"
                        accept="image/*"
                        name="photo"
                        onChange={handleInputChange}
                        required
                      />
                    </label>
            
                    <button type="submit" disabled={!emailValid || !passwordValid || !confirmPasswordValid}>
                      Зареєструватися
                    </button>
                  </form>
                </div>
              );
            };
            
  
  export default Test;
  