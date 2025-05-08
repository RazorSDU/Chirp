// vite.config.js
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
    plugins: [react()],
    server: {
        port: 55461,
        proxy: {
            '/api': {
                target: 'https://localhost:7061',
                changeOrigin: true,
                secure: false,    // if your dev cert is self-signed
            },
        },
    },
});
