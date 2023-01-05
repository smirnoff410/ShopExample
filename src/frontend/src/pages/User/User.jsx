import React from 'react';
import { useState, useEffect } from 'react';
import '../../App.css';
import UserCard from './UserCard';

const API_URL = 'http://localhost:5000/api/user';

const User = () => {
    const [users, setUsers] = useState([]);

    const getUsers = async () => {
        const response = await fetch(`${API_URL + '/user?token=super_hard_token'}`);
        const data = await response.json();
        setUsers(data)
        console.log(data)
    }

    useEffect(() => {
        getUsers()
    }, [])

    return (
        <div>
            {
                users?.length > 0 ? (
                    <div className = 'content'>
                        {users.map((item) => (
                            <UserCard user={item}/>
                        ))}
                        
                    </div>
                ) :
                (
                    <div className="empty">
                        <h2>No user found</h2>
                    </div>
                )
            }
        </div>
    )
}

export default User;