import React, { useState } from 'react';

const ErrorComponent = ({ error }) => {
  return error ? <div style={{border:'solid #c0392b',width:'90%', margin:'10px  0 0 0',borderRadius:'10px'}}><p style={{ color: '#c0392b', textAlign:'center'}}>{error}</p></div> : null;
};

export default ErrorComponent;
