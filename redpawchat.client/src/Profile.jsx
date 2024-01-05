import React, { useState, useEffect } from 'react';
import Conversation from './Conversation';
import Contacts from './Contacts';
import User from './User';
import {UserContext} from './UserContext';
import { useParams } from 'react-router-dom';
import Message from './Message';


const Profile = () => {
  const [conversations, setConversations] = useState([]);
  const [selectedConversation, setSelectedConversation] = useState(false);
  const[context,setContext]=useState(null);
  const { id } = useParams();

  async function getConversations() {
    try {
      const response = await fetch(`https://localhost:5123/api/profile/getconversations/${id}`, {
        credentials: 'include',
        headers: {
          "Content-Type": "application/json",
          'Accept': 'application/json',
        }
      });

      if (response.ok) {
        
        const data = await response.json();

        setContext(data);
        setConversations(data.conversation);
        
      } else {
        console.error("Error fetching conversations:", response.statusText);
        console.log(response);
      }
    } catch (error) {
      console.error("Error fetching conversations:", error.message);
    }
  }

  useEffect(() => {
    getConversations();
  }, []);

  const handleConversationClick = async () => {
    try {
      const response = await fetch(`https://localhost:5123/api/profile/getconversations/${id}`, {
        credentials: 'include',
        headers: {
          "Content-Type": "application/json",
          'Accept': 'application/json',
        }
      });

      if (response.ok) {
        
        const data = await response.json();

        setContext(data);
        setConversations(data.conversation);
        setSelectedConversation(true);
      } else {
        console.error("Error fetching conversations:", response.statusText);
        console.log(response);
      }
    } catch (error) {
      console.error("Error fetching conversations:", error.message);
    }
  };


  return (
    <UserContext.Provider value={context}>
    <div style={{ display: 'flex', background: '#333', borderRadius: '20px', height: '95vh',
     width:`${window.innerWidth-100}px`, height:`${window.innerHeight-30}px`, margin:'40px'  }}>
     
      <div style={{padding:'15px'}}>
      <User/>
     <Contacts/>
      </div>
      <div style={{ flex: 1, padding: '10px', border:'solid 1px' }}>
        <ul style={{ listStyle: 'none' }}>
          {conversations.map((conversation) => (
            <li key={conversation.id} onClick={() => handleConversationClick()}>
              <Conversation conversation={conversation} click={selectedConversation}/>
              <Message photo={conversation.messages[0].photo} userName={conversation.messages[0].userName} 
              text={conversation.messages[0].text} createdAt={conversation.messages[0].createdAt} />
            </li>
          ))}
        </ul>
      </div>
    </div>
    </UserContext.Provider>
    );
};

export default Profile;
