// src/pages/Post.jsx
import React from 'react'
import { useParams } from 'react-router-dom'
import PostThread from '../components/PostThread/PostThread'

export default function Post() {
    const { id } = useParams()
    return (
        <div className="min-h-screen bg-gray-100 p-4">
            <PostThread postId={id} />
        </div>
    )
}
