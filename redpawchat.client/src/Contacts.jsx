import React, { useState, useEffect } from 'react';
import './App.css';
import ContactsItem from './ContactsItem';
import UsersContacts from './UsersContacts';
import { useParams } from 'react-router-dom';

const Contacts = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const [error, setError] = useState('');
  const [searchResults, setSearchResults] = useState([]);
  const [loading, setLoading] = useState(false);
  const [contacts, setContacts]=useState(null);
 
  const { id } = useParams();


 const handleEnterPress= async ()=>{
  try{
    setLoading(true);
    const response= await fetch("https://localhost:5123/api/contacts/search",{
      method:'POST',
      credentials:'include',
      headers: {
        "Content-Type": "application/json",
        'Accept': 'application/json', 
      },

      body:JSON.stringify(searchTerm),
    })

    if (!response.ok) {
      throw new Error('Failed to fetch data');
    }

    const data = await response.json();
    setSearchResults(data);
  } catch (error) {
    setError(error.message || 'Something went wrong');
  } finally {
    setLoading(false);
  }
}

  const getContactsInfo= async ()=>{

    try{
      setLoading(true);
      console.log('getContactsInfo');
      const response= await fetch(`https://localhost:5123/api/contacts/getcontacts/${id}`,{
        method:'GET',
        credentials:'include',
        headers: {
          "Content-Type": "application/json",
          'Accept': 'application/json', 
        },
      })
  
      if (response.ok) {
        const data = await response.json();
        setContacts(data);
        console.log(data);
      }
  
    } catch (error) {
      setError(error.message || 'Something went wrong');
    } finally {
      setLoading(false);
    }

 };

    
     useEffect(()=>{
     getContactsInfo();
     }, [searchTerm]);
  

  const handleEdit = () => {
    console.log('Edit clicked');
  };

  

  return (
    <div>
      <input style={{padding:'15px', marginLeft:'25px'}}
        type="text"
        placeholder="Search users"
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
        onKeyDown={handleEnterPress}
      />
      <div style={{ margin: '0 0 0 20px' }}>
      {loading && <p>Loading...</p>}
      {searchTerm&&searchResults&&<ContactsItem contacts={searchResults}/>}
      {contacts&&<UsersContacts contacts={contacts}/>}
    </div>
    </div>
  );
};

export default Contacts;
