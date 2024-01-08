import React, { useState, useEffect } from 'react';
import Contacts from './Contacts';
import User from './User';
import {UserContext} from './UserContext';
import { useParams } from 'react-router-dom';
import { useDispatch } from 'react-redux'
import './App.css'
import { updateValue } from './conversationSlice';
import Conversation from './Conversation';
import removeIcon from './assets/delete.svg';

const Profile = () => {
  const [conversations, setConversations] = useState([]);
  const[context,setContext]=useState(null);
  const { id } = useParams();
  const dispatch = useDispatch();
  const [selectedConversation,setConversation]=useState(false);

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

  const handleConversationClick = async (e,conversation) => {
    
    if (e.target.tagName !=="IMG"|| e.taget.tagName!=="BUTTON") {
      console.log('Клік на елементі <li>:', conversation.id);
      console.log(conversation);
      console.log(e.target.tagName);
       if(conversation!=null){

        dispatch(updateValue(conversation));
        setConversation(true);
    }
    }
    
  };


  const removeConversationHandler= async(conversationid)=>{

    try {

      console.log(conversationid);

      const response = await fetch(`https://localhost:5123/api/conversations/removeconversation`, {
        method:'DELETE',
        credentials: 'include',
        headers: {
          "Content-Type": "application/json",
          'Accept': 'application/json',
        },

        body:JSON.stringify(`${conversationid}`),
      });

      if(response.ok){

        console.log(`Conversation ${conversationid} delete`);
        getConversations();
      }

    } catch (error) {
      console.error("Error fetching conversations:", error.message);
    }
  }

  
  return (
<UserContext.Provider value={context}>
  <div style={{ display: 'flex', margin: '40px'}}>
    <div style={{ background: '#333', borderRadius: '20px', height: '95vh' }}>
      <User />
      <Contacts />
    </div>
    <div style={{ flex: 1, padding: '10px', borderRadius: '15px', background: '#333', borderRadius: '20px', height: '92vh', marginLeft: '10px', paddingInlineEnd:'40px'}}>
      <ul style={{ listStyle: 'none',display:'flex', flexDirection:'column', alignContent:'center',width:'400px' }}>
        {conversations.map((conversation) => (
          <li key={conversation.id} onClick={(e) => handleConversationClick(e,conversation)} style={{ background: '#1a1a1a', borderRadius: '10px', padding: '20px', height: '70px', margin: '30px',position:'relative' }}>
            {conversation.messages[0] && (
              <div style={{ display: 'flex', alignItems: 'center' }}>
                <div style={{ display: 'flex', justifyContent: 'center' }}>
                  {conversation.messages[0] && (
                    <div style={{ width: '50px', height: '50px', borderRadius: '50%', overflow: 'hidden' }}>
                      <img
                        src={`data:image/png;base64,${conversation.messages[0].photo}`}
                        alt="Фото"
                        style={{ width: '100%', height: '100%', objectFit: 'cover' }}
                      />
                    </div>
                  )}
                  {conversation.messages[0].userName && <p style={{ marginLeft: '10px' }}>{conversation.messages[0].userName}</p>}
                  <div style={{ textAlign: 'center', padding: '20px', marginTop: '-20px' }}>
                    <p>{conversation.messages[0].text}</p>
                  </div>
                </div>
                
              </div>
            )}
            <div style={{ position :'absolute' ,top: 0, right: 0 }}>
                  <button style={{ width: '50px', height: '50px' }} onClick={()=>{removeConversationHandler (conversation.id)}}>
                    <img src={removeIcon} alt="Видалити" style={{ width: '20px', height: '20px' }} />
                  </button>
                </div>
          </li>
        ))}
      </ul>
    </div>
   {selectedConversation&&<Conversation/>} 
  </div>
</UserContext.Provider>);
};

export default Profile;



