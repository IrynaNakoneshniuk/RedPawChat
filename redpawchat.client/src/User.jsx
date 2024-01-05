import { useContext } from 'react';
import { UserContext } from './UserContext';
import pencilIcon from './assets/pensil.svg';

const User= ()=>{

  const currentUser= useContext(UserContext);
  console.log(currentUser);

    return(
       <div style={{display:'flex', margin:'20px', borderRadius:'10px', padding:'15px',
        background:'#1a1a1a', justifyContent:'space-between'}}>
          {currentUser && currentUser.photo && currentUser.name &&(
        <div 
          style={{
            width: '80px',
            height: '80px',
            borderRadius: '50%',
            overflow: 'hidden',
          }}
        >
          <img
            src={`data:image/png;base64,${currentUser.photo}`}
            alt="Фото"
            style={{ width: '100%', height: '100%', objectFit: 'cover'}}
          />
      
        </div>
      )}
      <div>
      {currentUser && currentUser.name && <p style={{fontFamily:'"Lucida Console", "Courier New", monospace'
       }}>{currentUser.name}</p>}
       <p style={{color:'#bdc3c7', fontFamily:'cursive'}}>online</p>
      </div>
      <button style={{width:'50px', height:'50px'}}>
            <img src={pencilIcon} alt="Олівець" style={{ width: '20px', height: '20px' }} />
          </button>
       </div>    
    );
};

export default User;