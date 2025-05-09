import React from "react";
import { useState } from 'react';

export default function LoginForm() {
    const [username, setUsername] = useState('Username');
    const [password, setPassword] = useState('Password');

    function handleUsernameChange(uname) {
        setUsername(uname.target.value);
    }

    function handlePasswordChange(pword) {
        setPassword(pword.target.value);
    }

    function handleSubmit() {
        console.log({ username });
        console.log({ password });
    }

    return (
        <>
            <form onSubmit={handleSubmit}>
                <input
                    placeholder="Username"
                    type="text"
                    id="username"
                    value={username}
                    onChange={handleUsernameChange}
                />

                <input
                    placeholder="Password"
                    type="password"
                    id="password"
                    value={password}
                    onChange={handlePasswordChange}
                />
                <input type="submit" value="submit" />
            </form>
        </>
    );
}