import React, { useState } from 'react';
import './App.css';
import { useParams } from 'react-router-dom';

const ContactsItem = (props) => {

  const { id } = useParams();

  const[successAdd,setsuccessAdd]=useState(false);

  const addContactHandler = async (e) => {
    try {
      
      const contactid=e.target.getAttribute("data-id");

      console.log(`${id},${contactid}`);

      const responce= await fetch("https://localhost:5123/api/contacts/addcontact",{
        method:'POST',
        credentials:'include',
        headers: {
          "Content-Type": "application/json",
          'Accept': 'application/json', 
        },
      
        body:JSON.stringify(`${id},${contactid}`),
      });

      if(responce.ok){
        setsuccessAdd(true);
        console.log(`Contact with ID ${contactid} added to the contacts list.`);
      }
    } catch (error) {
      console.error('Error adding contact:', error);
    }
  };



  return (
    <div className='scroll-container' style={{ display: 'flex',flexDirection:'row',alignItems:'center',
     justifyItems:'center',overflowY: 'auto',height: '60vh'}}>
      <ul style={{listStyle:'none', padding:'10px', margin:'0 5px 0 0',padding:'10px'}}>
        {props.contacts&&props.contacts.map((contact) => (
          <li key={contact.id} style={{display: 'flex', fontFamily:'cursive',marginBottom:'10px',
          background:'#1a1a1a',borderRadius:'10px',padding:'10px'}}>
           <div style={{ marginRight: '10px' }}>
        {contact.photo && (
          <img
            src={`data:image/png;base64,${contact.photo}`}
            alt="User Photo"
            style={{ width: '50px', height: '50px', objectFit: 'cover', borderRadius: '50%' }}
          />
        )}
       <span style={{color:'#bdc3c7',fontSize:'8px'}}>{contact.isonline ? 'Online' : 'Offline'}</span>
       {contact.name &&(<span style={{display:'block'}}>{contact.name}</span>)}
      </div>
      <button data-id={contact.id} onClick={addContactHandler} style={{height:'50px', marginTop:'20px', marginLeft:'5px'}}>Додати</button >
      <button style={{background:'#e67e22', height:'50px',marginTop:'20px'}}>Повідомлення</button>
          </li>
        ))}
      </ul>
      {successAdd&&<p style={{border:'solid #00b894 0.5px',borderRadius:'10px',color:'#00b894 '}}>Контакт успішно додано</p>}
      </div>
  );
};

export default ContactsItem;
