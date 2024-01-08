import React from 'react';

const Message = (props) => {

  const currentDate=new Date(props.createdAt).toLocaleString();

   return (
    <div style={{display:'flex', alignItems:'center',alignContent:'center'}}>
      <div style={{display:'flex', justifyContent:'center'}}>
      {props.photo && (
        <div
          style={{
            width: '50px',
            height: '50px',
            borderRadius: '50%',
            overflow: 'hidden',
            marginLeft:'50px'
          }}
        >
          <img
            src={`data:image/png;base64,${props.photo}`}
            alt="Фото"
            style={{ width: '100%', height: '100%', objectFit: 'cover'}}
          />
              </div>)}
              <div style={{background:'#636e72',textAlign:'center',borderRadius:'20px',margin:'15px',padding:'20px'}}>
            
            <p>{props.text}</p>
            <p style={{fontSize:'8px'}}>{currentDate}</p>
            </div>
            
      </div>
            
              
    </div>)};

export default Message;
