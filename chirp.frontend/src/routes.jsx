// src/routes.jsx
import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import Post from './pages/Post';
import UserFeedPage from './pages/UserFeed';
import Login from './pages/Login';

export default function AppRoutes() {
    return (
        <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/post/:id" element={<Post />} />
            <Route path="/user/:username" element={<UserFeedPage />} />
            <Route path="/login" element={<Login />} />
        </Routes>
    );
}
