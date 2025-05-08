// src/main.jsx
import React from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import './main.css';
import AppRoutes from './routes';

createRoot(document.getElementById('root')).render(
    <React.StrictMode>
        <BrowserRouter>
            <AppRoutes />
        </BrowserRouter>
    </React.StrictMode>
);
