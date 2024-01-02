import React, { useState, useEffect } from 'react';
import Conversation from './Conversation';
import Contacts from './Contacts';
import { useUserContext } from './UserContext';
import { useParams } from 'react-router-dom';

const Profile = () => {
   // const [chats, setChats] = useState([]);
    //const [selectedChat, setSelectedChat] = useState(null);
    const [conversation, setConversations] = useState([]);
    const { id } = useParams();
    const { updateUserData } = useUserContext();

    async function GetConversation(){
      const response = await fetch(`https://localhost:5123/api/profile/getconversations/${id}`, {
      credentials: 'include',
       headers: {
      "Content-Type": "application/json",
      'Accept': 'application/json',
       }})

       if (response.ok) {
        const data = await response.json();
        updateUserData(data);
        setConversations(data.conversation);
        console.log(data);

      } else {
        console.error("Error fetching conversations:", response.statusText);
        сonsole.log(response);
      }
  };

    useEffect(()=>{
      GetConversation();
    },[]);
    

return (
  <div>
    <h1>Profile</h1>
    <ul>
      {conversation&&conversation.map((conversation) => (
        <li key={conversation.id}>
          
          <Conversation conversation={conversation} />
        </li>
      ))}
    </ul>
  </div>
);
  };
  
  export default Profile;
  