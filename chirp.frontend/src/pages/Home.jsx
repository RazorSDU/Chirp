// src/pages/Home.jsx

import React from 'react';
import Feed from '../components/Feed/Feed';

export default function Home() {
    return (
        <div className="flex justify-center bg-gray-100 min-h-screen">
            <main className="w-full max-w-2xl p-4">
                <Feed />
            </main>
        </div>
    );
}
