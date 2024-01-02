import React, { useState, useEffect } from 'react';

const Contacts = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const [searchResults, setSearchResults] = useState([]);

  useEffect(() => {
    // Запит до сервера за списком користувачів
    const fetchData = async () => {
      const users = await searchUsers(searchTerm);
      setSearchResults(users);
    };
    const simulateDelay = (milliseconds) => new Promise(resolve => setTimeout(resolve, milliseconds));

    // Список усіх користувачів (симулюємо базу даних)
    const allUsers = [
      { id: 1, username: 'user1' },
      { id: 2, username: 'user2' },
      { id: 3, username: 'user3' },
      // Додайте більше користувачів за потребою
    ];
    
    // Симулювання функції пошуку користувачів
    const searchUsers = async (searchTerm) => {
      await simulateDelay(500); // Симулювати затримку
      const filteredUsers = allUsers.filter(user =>
        user.username.toLowerCase().includes(searchTerm.toLowerCase())
      );
      return filteredUsers;
    };
    
    // Симулювання функції додавання контакту
    const addContact = async (userId) => {
      await simulateDelay(500); // Симулювати затримку
      // Логіка для додавання контакту на сервері (не симульовано в цьому прикладі)
      console.log(`Added user with ID ${userId} to contacts.`);
    };
    
    // Симулювання функції блокування користувача
    const blockUser = async (userId) => {
      await simulateDelay(500); // Симулювати затримку
      // Логіка для блокування користувача на сервері (не симульовано в цьому прикладі)
      console.log(`Blocked user with ID ${userId}.`);
    };
    
    fetchData();
  }, [searchTerm]); // Запускати ефект при зміні searchTerm

  const handleAddContact = (userId) => {
    addContact(userId);
    // Оновити список контактів
  };

  const handleBlockUser = (userId) => {
    blockUser(userId);
    // Оновити список контактів
  };

  return (
    <div>
      <h2>Contacts</h2>
      <input
        type="text"
        placeholder="Search users"
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
      />
      <ul>
        {searchResults.map((user) => (
          <li key={user.id}>
            {user.username}
            <button onClick={() => handleAddContact(user.id)}>Add Contact</button>
            <button onClick={() => handleBlockUser(user.id)}>Block User</button>
          </li>
        ))}
      </ul>
      {/* Додайте список існуючих контактів користувача */}
    </div>
  );
};

export default Contacts;
