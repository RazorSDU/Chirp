import React, { useState, useEffect } from 'react';
import { fetchUserPosts } from '../../services/api';
import Spinner from '../Shared/Spinner';
import UserPostCard from './UserPostCard';

export default function UserFeed({ username }) {
    const [posts, setPosts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        setLoading(true);
        fetchUserPosts(username, 1, 15)
            .then(data => setPosts(data))
            .catch(err => setError(err))
            .finally(() => setLoading(false));
    }, [username]);

    if (loading) {
        return (
            <div className="flex justify-center p-4">
                <Spinner />
            </div>
        );
    }

    if (error) {
        return <div className="text-red-500 p-4">Error: {error.message}</div>;
    }

    return (
        <div className="space-y-6">
            <div className="flex items-center bg-white border border-gray-200 rounded p-4">
                <img
                    src={`https://ui-avatars.com/api/?name=${username}&background=ddd&color=555&size=64`}
                    alt={username}
                    className="w-16 h-16 rounded-full mr-4"
                />
                <h1 className="text-2xl font-bold">{username}</h1>
            </div>

            {posts.map(post => (
                <UserPostCard key={post.id} post={post} />
            ))}
        </div>
    );
}
