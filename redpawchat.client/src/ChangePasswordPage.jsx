import React, { useState } from 'react';

const ChangePasswordPage = () => {
  const [oldPassword, setOldPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');

  const handleChangePassword = async () => {
    // Викликати API для зміни паролю користувача
    console.log('Зміна паролю: ', { oldPassword, newPassword });
  };

  return (
   
    <div>
      <h2>Зміна паролю</h2>
      <input
        type="password"
        placeholder="Поточний пароль"
        value={oldPassword}
        onChange={(e) => setOldPassword(e.target.value)}
      />
      <input
        type="password"
        placeholder="Новий пароль"
        value={newPassword}
        onChange={(e) => setNewPassword(e.target.value)}
      />
      <button onClick={handleChangePassword}>Змінити пароль</button>
    </div>
  );
};

export default ChangePasswordPage;
