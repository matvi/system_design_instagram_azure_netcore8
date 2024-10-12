import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Modal from 'react-modal';
import './App.css'; // Import CSS file for styling
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faHeart } from '@fortawesome/free-solid-svg-icons';
import { faPlus, faSignOutAlt } from '@fortawesome/free-solid-svg-icons'; 
import PostModal from './PostModal';
import Login from './Login';

function App() {
  const [userId, setUserId] = useState('');
  const [posts, setPosts] = useState([]);
  const [modalIsOpen, setModalIsOpen] = useState(false);

  

  useEffect(() => {
    if (userId) {
      axios.get(`http://localhost:5003/Feed/${userId}`)
        .then(response => {
          if (response.data.isSuccess) {
            setPosts(response.data.posts);
          } else {
            console.error('Error fetching posts:', response.data.errorMessage);
          }
        })
        .catch(error => {
          console.error('Error fetching posts:', error);
        });
    }
  }, [userId]);

  const handleUserIdChange = (event) => {
    setUserId(event.target.value);
  };

  const openModal = () => {
    setModalIsOpen(true);
  };

  const closeModal = () => {
    setModalIsOpen(false);
  };

  const handleLogin = (userId) => {
    setUserId(userId);
  };

  const handleLogout = () => {
    setUserId(''); // Clear the userId to log out
    setPosts([]); // Clear the posts when logging out
  };

  const handleLike = (postId) => {
    const likeData = {
      userId: userId,
      postId: postId
    };

    axios.post('http://localhost:5005/Like', likeData)
    .then(response => {
      if (response.data.isSuccess) {
        // Increase likes count by one
        setPosts(prevPosts => prevPosts.map(post => {
          if (post.postId === postId) {
            return { ...post, likes: post.likes + 1 };
          }
          return post;
        }));
      } else {
        console.error('Error liking post:', response.data.errorMessage);
      }
    })
    .catch(error => {
      console.error('Error liking post:', error);
    });
};

  

return (
  <div className="App">
    {userId && (
      <div className="header">
        <div className="user-indicator">
          Logged in as: {userId}
        </div>
        <div className="logout">
          <button className="btn btn-danger" onClick={handleLogout}>
            <FontAwesomeIcon icon={faSignOutAlt} /> Logout
          </button>
        </div>
      </div>
    )}

    {!userId && <Login onLogin={handleLogin} />}
      <div className="posts">
        {posts.map(post => (
          <div key={post.postId} className="post-box">
            <div className="user-profile">
              <img src={post.user.userProfileImageUrl} alt="User Profile" />
              <p>{post.user.userName}</p>
            </div>
            <img src={post.imageUrl} alt={post.imageName} className="post-image" />
            <div className="post-details">
              <h3>{post.title}</h3>
              <p>{post.postText}</p>
              <div className="likes" onClick={() => handleLike(post.postId)}>
                <FontAwesomeIcon icon={faHeart} />
                <span>{post.likes}</span>
              </div>
            </div>
          </div>
        ))}
      </div>

      <Modal isOpen={modalIsOpen} onRequestClose={closeModal}>
        <PostModal isOpen={modalIsOpen} onRequestClose={closeModal} userId={userId} />
      </Modal>
      {userId && (
        <button className="add-post-button" onClick={() => setModalIsOpen(true)}>
          <FontAwesomeIcon icon={faPlus} />
        </button>
      )}
    </div>
  );
}

export default App;
