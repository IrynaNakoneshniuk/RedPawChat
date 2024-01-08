import React from 'react';
import './App.css';
import { useParams } from 'react-router-dom';

import Conversation from './Conversation';


const UsersContacts = (props) => {
  const { id } = useParams();
  

const removeHandler= async (contactid)=>{

  try {
    
    console.log(`${id},${contactid}`);

    const responce= await fetch("https://localhost:5123/api/contacts/deletecontact",{
      method:'DELETE',
      credentials:'include',
      headers: {
        "Content-Type": "application/json",
        'Accept': 'application/json', 
      },
    
      body:JSON.stringify(`${id},${contactid}`),
    });
    
    if(responce.ok){
      const data =responce.json();
      navigate(<Conversation id ={data}/>)

    }
  } catch (error) {
    console.error('Error adding contact:', error);
  }

}

const createConversationHandler= async (contactid)=>{

  try {
    
    console.log(`${id},${contactid}`);

    const responce= await fetch("https://localhost:5123/api/conversations/createnewconversation",{
      method:'POST',
      credentials:'include',
      headers: {
        "Content-Type": "application/json",
        'Accept': 'application/json', 
      },
    
      body:JSON.stringify(`${id},${contactid}`),
    });
    
    if(responce.ok){
      const data= await responce.json();
      
    }
  } catch (error) {
    console.error('Error adding contact:', error);
  }

}

  return (
    <div className='scroll-container' style={{ display: 'flex',flexDirection:'row',alignItems:'center',
     justifyItems:'center',overflowY: 'scroll',maxHeight: '60vh'}}>
      <ul  style={{listStyle:'none', padding:'10px', margin:'0 5px 0 0',padding:'10px'}}>
        {props.contacts&&props.contacts.map((contact) => (
          <li key={contact.id} style={{display: 'flex', fontFamily:'cursive',marginBottom:'10px',
          background:'#1a1a1a',borderRadius:'10px',padding:'10px'}}>
           <div  style={{ marginRight: '10px' }}>
        {contact.photo && (
          <img 
            src={`data:image/png;base64,${contact.photo}`}
            alt="User Photo"
            style={{ width: '50px', height: '50px', objectFit: 'cover', borderRadius: '50%' }}
          />
        )}
       <span  style={{color:'#bdc3c7',fontSize:'8px'}}>{contact.isonline ? 'Online' : 'Offline'}</span>
       {contact.name &&(<span  style={{display:'block'}}>{contact.name}</span>)}
      </div>
        <button onClick={()=>{removeHandler(contact.id)}}  style={{height:'50px', marginTop:'20px', marginLeft:'5px'}}>Видалити</button>
        <button onClick={()=>{createConversationHandler(contact.id)}} style={{background:'#e67e22', height:'50px',marginTop:'20px'}}>Повідомлення</button>
          </li>
        ))}
      </ul>
      </div>
  );
};

export default UsersContacts;
