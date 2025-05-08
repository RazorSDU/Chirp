import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { fetchFeed, fetchComments } from '../../services/api';
import Spinner from '../Shared/Spinner';
import { formatDate } from '../../utils/date';

const Feed = () => {
    const [posts, setPosts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        fetchFeed(1, 15)
            .then(data => setPosts(data))
            .catch(err => setError(err))
            .finally(() => setLoading(false));
    }, []);

    if (loading) {
        return (
            <div className="flex justify-center p-4">
                <Spinner />
            </div>
        );
    }

    if (error) {
        return <div className="text-red-500 p-4">Error loading feed: {error.message}</div>;
    }

    return (
        <div className="space-y-6">
            {posts.map(post => (
                <PostCard key={post.id} post={post} />
            ))}
        </div>
    );
};

const PostCard = ({ post }) => {
    const [comments, setComments] = useState([]);
    const [page, setPage] = useState(0);
    const [loadingComments, setLoadingComments] = useState(false);
    const [hasComments, setHasComments] = useState(false);

    // On mount, check if at least one comment exists
    useEffect(() => {
        fetchComments(post.id, 1, 1)
            .then(data => setHasComments(data.length > 0))
            .catch(() => setHasComments(false));
    }, [post.id]);

    const loadReplies = () => {
        const nextPage = page + 1;
        setLoadingComments(true);

        fetchComments(post.id, nextPage, 5)
            .then(data => setComments(prev => [...prev, ...data]))
            .catch(err => console.error(err))
            .finally(() => {
                setPage(nextPage);
                setLoadingComments(false);
            });
    };

    return (
        <div className="relative">
            {/* Post card */}
            <div className="bg-white border border-gray-200 rounded-t-lg shadow p-4">
                <div className="flex items-center justify-between">
                    <Link
                        to={`/user/${post.username}`}
                        className="font-bold text-blue-600 hover:underline"
                    >
                        {post.username}
                    </Link>
                    <span className="text-gray-500 text-sm">{formatDate(post.createdAt)}</span>
                </div>
                <p className="mt-2">{post.body}</p>
                {post.imageUrl && (
                    <img
                        src={post.imageUrl}
                        alt="Post"
                        className="mt-2 rounded max-h-60 object-cover"
                    />
                )}
            </div>

            {/* Comments wrapper */}
            {page > 0 && (
                <div className="bg-white border border-gray-200 rounded-b-lg -mt-px">
                    <div className="p-4 space-y-4">
                        {comments.map(comment => (
                            <div
                                key={comment.id}
                                className="bg-white border border-gray-200 rounded-lg p-3"
                            >
                                <Link
                                    to={`/user/${comment.username}`}
                                    className="text-sm font-semibold text-blue-600 hover:underline"
                                >
                                    {comment.username}
                                </Link>
                                <div className="text-sm">{comment.body}</div>
                            </div>
                        ))}
                    </div>
                </div>
            )}

            {/* Show replies button, only if there are comments */}
            {hasComments && (
                <div className="pt-4">
                    <button
                        onClick={loadReplies}
                        className="text-blue-600 hover:underline focus:outline-none"
                        disabled={loadingComments}
                    >
                        {loadingComments
                            ? 'Loading…'
                            : page === 0
                                ? 'Show replies'
                                : 'Show more replies'}
                    </button>
                </div>
            )}
        </div>
    );
};

export { PostCard };
export default Feed;
