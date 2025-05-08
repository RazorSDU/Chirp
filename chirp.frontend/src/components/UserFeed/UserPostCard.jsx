import React, { useState, useEffect } from 'react';
import { fetchThread } from '../../services/api';
import { Link } from 'react-router-dom';
import Spinner from '../Shared/Spinner';
import { formatDate } from '../../utils/date';
import { PostCard } from '../Feed/Feed';

export default function UserPostCard({ post }) {
    const [parent, setParent] = useState(null);
    const [loadingParent, setLoadingParent] = useState(false);

    useEffect(() => {
        if (post.parentPostId) {
            setLoadingParent(true);
            fetchThread(post.parentPostId)
                .then(arr => setParent(arr[0]))
                .catch(err => console.error(err))
                .finally(() => setLoadingParent(false));
        }
    }, [post.parentPostId]);

    return (
        <div>
            {loadingParent && (
                <div className="flex justify-center p-2">
                    <Spinner />
                </div>
            )}

            {parent && (
                <div className="bg-gray-50 border border-gray-200 rounded p-2 mb-2 text-sm">
                    <div className="flex items-center justify-between">
                        <Link
                            to={`/user/${parent.username}`}
                            className="font-semibold text-blue-600 hover:underline"
                        >
                            {parent.username}
                        </Link>
                        <span className="text-gray-500 text-xs ml-2">
                            {formatDate(parent.createdAt)}
                        </span>
                    </div>
                    <p className="mt-1">{parent.body}</p>
                </div>
            )}

            <PostCard post={post} />
        </div>
    );
}
