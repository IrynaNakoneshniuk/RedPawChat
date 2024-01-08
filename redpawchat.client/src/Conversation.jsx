import React, { useState,useEffect } from 'react';
import Message from './Message';
import banIcon from './assets/ban.svg';
import addUser from './assets/add-user.svg';
import { useSelector} from 'react-redux';
import { useParams } from 'react-router-dom';
import './App.css';
import connection from './connection.js';
import { UserContext } from './UserContext';
import { useContext } from 'react';
import { v4 as uuidv4 } from 'uuid';

const Conversation = (props) => {
  const [newMessage, setNewMessage] = useState('');
  const [showDropdown, setShowDropdown] = useState(false); 
  const [currentUser, setSelectedMember] = useState(null); 
  const [hovered, setHovered] = useState('#1a1a1a');
  const { id } = useParams();
  const user= useContext(UserContext);
  const currentConversation=useSelector((state) => state.conversation.value);
  console.log(currentConversation);
  const [conversation,setCurrentConversation]=useState(currentConversation);


  const addMessageToDb= async (message)=>{

    const messageDto={
      id:message.id,
      userName:message.userName,
      photo:message.photo,
      conversationId:message.conversationId,
      userId:message.userId,
      text:message.text,
      createdAt: Date.now(),
      deletedAt:null,
    }


    try{
      console.log('addMessageToDb');
      const response= await fetch(`https://localhost:5123/api/conversations/addmessage`,{
        method:'POST',
        credentials:'include',
        headers: {
          "Content-Type": "application/json",
          'Accept': 'application/json', 
        },
        body:JSON.stringify({messageDto}),
      })
  
      if (response.ok) {
        
        console.log("Message add to db");
      }
  
    } catch (error) {
      setError(error.message || 'Something went wrong');
    } finally {
    }
  }

  useEffect(() => {

    connection.start().then(() => {
      console.log('Connected to SignalR');
    });

     
    connection.on('SendMessage', (conversationId, userName, message,userId) => {
      if (conversationId.toString() === conversation.id) {
        console.log('SendMessage:', userName, message);

        const member=conversation.members.find(m=>m.id==userId);

        const text= {
          id:uuidv4().toString(),
          userName: userName,
          photo:member.photo,
          conversationId:conversationId,
          userId:userId,
          text: message,
          createdAt: Date.now(),
          deletedAt:null,
        };

        if(text!=null){
          addMessageToDb(text);
        }
        
        console.log(" addMessageToDb");

        setCurrentConversation((conversation) => ({
          ...conversation,
          messages: [
            ...conversation.messages,
            {
              id:uuidv4(),
              photo:member.photo,
              userName: userName,
              text: message,
              createdAt: Date.now(),
              userid:userId,
              deleteAt:null,
            },
          ],
        }));
    
           console.log(conversation);
      }
    });


    connection.on('UserJoined', (user) => {
      console.log('User joined:', user);
      
    });

    connection.on('UserLeft', (userName) => {
      console.log('User left:', userName);
      
    });

    return () => {
    
      connection.stop()
  .then(() => {
    console.log('Disconnected from SignalR');
  })
  .catch((err) => {
    console.error('Error disconnecting from SignalR:', err);
  });
    };
  }, []);
  

  
  const containerStyle = {   
    backgraund:'#333',
    overflowY: 'auto',
    height: '75vh',
  };

  const sendMessageHandler = () => {
    
    if (newMessage.trim() !== '') {

      if (connection.state === 'Connected') {
        connection.invoke('SendMessage', conversation.id, user.name, newMessage,user.id)
          .catch((error) => {
            console.error('Error sending message:', error);
          });
      } else {
        console.error('Connection is not in the Connected state');
      }
  
      setNewMessage('');
    }
  }

  const addMembers =async (contactid)=>{
    try{
      console.log('getContactsInfo');
      const response= await fetch(`https://localhost:5123/api/conversations/addmembers`,{
        method:'POST',
        credentials:'include',
        headers: {
          "Content-Type": "application/json",
          'Accept': 'application/json', 
        },
        body:JSON.stringify(`${conversation.id},${contactid}`)
      })
  
      if (response.ok) {
        updateConversation();
      }
  
    } catch (error) {
      setError(error.message || 'Something went wrong');
    } finally {
    }
  }

  const addUserHandler =async ()=>{
    

    try{
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
        setSelectedMember(data);
        setShowDropdown((!showDropdown));
        console.log(data);
      }
  
    } catch (error) {
      setError(error.message || 'Something went wrong');
    } finally {
    }

  };

  const updateConversation = async () => {
    try {
      const response = await fetch(`https://localhost:5123/api/conversations/updateconversation/${id}/${currentConversation.id}`, {
        method: 'GET',
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
        },
      });
  
      if (response.ok) {
        const data = await response.json();
        setCurrentConversation(data);
      }
    } catch (error) {
      setError(error.message || 'Something went wrong');
    }
  };
    

  return (
    <div style={{background:'#333', width:'400px',borderRadius:'15px',marginLeft:'10px'}}> 
      <div style={{ display: 'flex', justifyContent: 'center',marginTop:'20px',position: 'relative'}}>
      <button style={{width:'40px', height:'30px',position: 'relative', marginRight:'10px'}} onClick={addUserHandler}>
            <img src={banIcon} alt="Заблокувати" style={{
      width: '20px',
      height: '20px',
      position: 'absolute',  
      top: '50%',  
      left: '50%',  
      transform: 'translate(-50%, -50%)',  
      }}/>
          </button>
          <button style={{width:'40px', height:'30px',position: 'relative', marginRight:'10px'}} onClick={addUserHandler}>
            <img src={addUser} alt="Додати" style={{
      width: '20px',
      height: '20px',
      position: 'absolute',  
      top: '50%',  
      left: '50%',  
      transform: 'translate(-50%, -50%)',  
    }}/>
          </button>
          {showDropdown && (
  <div className='scroll-container contacts' style={{
    position: 'absolute',
    top: '80%', 
    right: '20px',
    background: '#1a1a1a',
    borderRadius: '10px',
    padding: '5px',
    zIndex: '1000', 
  }}>
    <ul>
      {currentUser&&
        currentUser.map((contact) => (
          <li
            key={contact.id}
            style={{
              display: 'flex',
              fontFamily: 'cursive',
              marginBottom: '10px',
              background: {hovered},
              borderRadius: '10px',
              padding: '10px',
            
            }}
            onMouseLeave={()=>{setHovered('#1a1a1a')}}
            onMouseEnter={()=>{setHovered('#333')}}
            onClick={()=>{addMembers(contact.id)}}
          >
            <div style={{ marginRight: '10px'}}>
              <span style={{ color: '#bdc3c7', fontSize: '8px' }}>{contact.isonline ? 'Online' : 'Offline'}</span>
              {contact.name && <span style={{ display: 'block' }}>{contact.name}</span>}
            </div>
            <hr/>
          </li>
        ))}
    </ul>
  </div>
)}
          <div style={{height:'40px'}}>
          <ul style={{listStyle:'none',width:'100px',height:'40px', margin:'0', padding:'0',
          position:'absolute', top:'0', right:'0',display:'flex'}} >
          {conversation.members && conversation.members.map((member) => (
       <li key={member.id} style={{ listStyle: 'none',width:'35px'}}>
    {member.photo && (
      <div
        style={{
          width: '30px',
          height: '30px',
          borderRadius: '50%',
          overflow: 'hidden',
        }}
      >
        <img
          src={`data:image/png;base64,${member.photo}`}
          alt="Фото"
          style={{ width: '100%', height: '100%', objectFit: 'cover' }}
        />
      </div>
    )}
  </li>
    ))}
           </ul>
       </div>
          </div>
      <ul style={{ listStyleType: 'none', paddingInlineStart:'40px'}}>
        <div className='scroll-container' style={containerStyle}>
          {conversation&&(
            conversation.messages.map((message) => (
              <li key={message.id} style={{ listStyleType: 'none'}}>
                <Message
                  photo={message.photo}
                  userName={message.userName}
                  text={message.text}
                  createdAt={message.createdAt}
                />
              </li>
            ))
          )}
        </div>
      </ul>
      <input
        style={{ borderRadius: '20px', marginLeft:'20px'}}
        type="text"
        placeholder="Type your message"
        value={newMessage}
        onChange={(e) => setNewMessage(e.target.value)}
        onKeyDown={(e) => e.key === 'Enter' && sendMessageHandler()}
      />
    </div>
  );
};

export default Conversation;

