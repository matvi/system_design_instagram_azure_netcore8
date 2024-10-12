import React, { useState } from 'react';
import axios from 'axios';

function PostModal({ isOpen, onRequestClose, userId }) {
  const [file, setFile] = useState(null);
  const [postTitle, setPostTitle] = useState('');
  const [postText, setPostText] = useState('');

  const handleFileChange = (event) => {
    setFile(event.target.files[0]);
  };

  const handleSubmit = async () => {
    try {
      if (!file || !postTitle || !postText) {
        alert("Please fill out all required fields.");
        return;
      }

      const formData = new FormData();
      formData.append('file', file);
      formData.append('postTitle', postTitle);
      formData.append('postText', postText);
      formData.append('userId', userId);

      await axios.post('http://localhost:5001/Post/Upload', formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      });

      // Close modal and clear form fields
      onRequestClose();
      setFile(null);
      setPostTitle('');
      setPostText('');
    } catch (error) {
      console.error('Error uploading post:', error);
    }
  };

  return (
    <div className="modal-dialog">
      <div className="modal-content">
        <div className="modal-header">
          <h5 className="modal-title">Add New Post</h5>
          <button type="button" className="close" onClick={onRequestClose}>
            <span>&times;</span>
          </button>
        </div>
        <div className="modal-body">
          <input type="file" className="form-control mb-3" onChange={handleFileChange} required />
          <input type="text" placeholder="Post Title" className="form-control mb-3" value={postTitle} onChange={(e) => setPostTitle(e.target.value)} required />
          <input type="text" placeholder="Post Text" className="form-control mb-3" value={postText} onChange={(e) => setPostText(e.target.value)} required />
        </div>
        <div className="modal-footer">
          <button type="button" className="btn btn-secondary" onClick={onRequestClose}>Close</button>
          <button type="button" className="btn btn-primary" onClick={handleSubmit}>Upload Post</button>
        </div>
      </div>
    </div>
  );
}

export default PostModal;
