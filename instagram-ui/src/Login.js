// Login.js

import React, { useState } from 'react';

function Login({ onLogin }) {
  const [userId, setUserId] = useState('');

  const handleLogin = () => {
    onLogin(userId);
  };

  return (
    <div className="container">
      <h2>Login</h2>
      <div className="form-group">
        <label htmlFor="userId">User ID:</label>
        <input
          type="text"
          id="userId"
          className="form-control"
          value={userId}
          onChange={(e) => setUserId(e.target.value)}
          placeholder="Enter User ID"
        />
      </div>
      <button className="btn btn-primary" onClick={handleLogin}>Login</button>
    </div>
  );
}

export default Login;
