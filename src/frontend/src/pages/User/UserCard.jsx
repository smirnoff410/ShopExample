import React from 'react';

const UserCard = ({user}) => {
    return(
            <div className="profile">
                <div className="profile-image">
                    <img className="profile-icon" src="https://ps.w.org/users-profile-picture/assets/icon-256x256.jpg" />
                </div>
                <h2 className="profile-username">{user.name}</h2>
                <small className="profile-user-handle">{user.birthday}</small>
                    <a className="profile-btn" onClick={() => {
                localStorage.setItem('user-id', user.id)}}>Use this user</a>
        </div>
    )
}

export default UserCard;