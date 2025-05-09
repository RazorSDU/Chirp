import React from 'react';
import LoginForm from '../components/LoginForm/LoginFormOld'


export default function Login() {
    return (
        <div className="flex justify-center bg-gray-100 min-h-screen">
            <main className="w-full max-w-2xl p-4">
                <LoginForm />
            </main>
        </div>
    )
}