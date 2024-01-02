import React, { useState } from 'react';
import Message from './Message';

const Conversation = (props) => {
  const [newMessage, setNewMessage] = useState('');

  console.log(props);

  const containerStyle = {
    // Рамка для зони списку
    height: '700px',          // Фіксована висота
    overflowY: 'auto',        
  };

  const openSocket= ()=>{

    const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

     async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }

    connection.onclose(async () => {
      await start();
  });
  } 

      start();
  }

  //openSocket();

  const sendMessages=async ()=>{
    
    const message={
      text:{newMessage},
      conversationId:props.id,
    }

    try {
      await connection.invoke("SendMessage", user, message);
  } catch (err) {
      console.error(err);
  }

    try{

      const response= await fetch("https://localhost:5123/conversation/sendmessage",{
        method:'POST',
        credentials:'include',
        headers: {
          "Content-Type": "application/json",
          'Accept': 'application/json', 
        },

        body:JSON.stringify(message),
      })

      console.log(response.body);

    }catch(error){
      console.log(error);
    }
  }

  const handleSendMessage = () => {
    //sendMessages();
    setNewMessage('');
  };

  return (
    <div className='scroll-container' style={containerStyle}>
       <ul style={{listStyleType:'none'}}>
      <div>
        {props.conversation.messages.map((message) => (
          <li key={message.id} style={{listStyleType:'none'}}>
          <Message photo={message.photo} userName={message.userName} 
          text={message.text} createdAt={message.createdAt} />
          </li>
        ))}
      </div>
      </ul>
      <input style={{borderRadius:'20px'}}
        type="text"
        placeholder="Type your message"
        value={newMessage}
        onChange={(e) => setNewMessage(e.target.value)}
      />
      <button onClick={handleSendMessage}>Відправити</button>
    </div>
  );
};

export default Conversation;

