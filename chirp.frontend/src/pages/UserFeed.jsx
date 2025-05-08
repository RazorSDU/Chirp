import React from 'react';
import { useParams } from 'react-router-dom';
import UserFeed from '../components/UserFeed/UserFeed';

export default function UserFeedPage() {
    const { username } = useParams();

    return (
        <div className="flex justify-center bg-gray-100 min-h-screen p-4">
            <main className="w-full max-w-2xl">
                <UserFeed username={username} />
            </main>
        </div>
    );
}
