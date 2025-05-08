// src/services/api.js

const BASE_URL = '/api';

async function request(path, options = {}) {
    const res = await fetch(`${BASE_URL}${path}`, {
        headers: {
            'Content-Type': options.body instanceof FormData ? undefined : 'application/json',
        },
        ...options,
    });

    if (!res.ok) {
        if (res.status === 404) throw new Error('Resource not found');
        const errorBody = await res.json().catch(() => null);
        throw new Error(errorBody?.message || res.statusText);
    }

    if (res.status === 204) return null;
    return res.json();
}

// Posts
export const fetchFeed = (page = 1, pageSize = 20) =>
    request(`/posts/feed?page=${page}&pageSize=${pageSize}`);

export const fetchComments = (postId, page = 1, pageSize = 50) =>
    request(`/posts/${postId}/comments?page=${page}&pageSize=${pageSize}`);

export const fetchThread = (postId) =>
    request(`/posts/${postId}/thread`);

export const fetchUserPosts = (username, page = 1, pageSize = 20) =>
    request(`/posts/user/${username}?page=${page}&pageSize=${pageSize}`);

export const createPost = (userId, body) =>
    request('/posts', { method: 'POST', body: JSON.stringify({ userId, body }) });

export const editPost = (postId, body) =>
    request(`/posts/${postId}`, { method: 'PUT', body: JSON.stringify({ body }) });

export const deletePost = (postId) =>
    request(`/posts/${postId}`, { method: 'DELETE' });

export const deleteThread = (postId) =>
    request(`/posts/${postId}/thread`, { method: 'DELETE' });

export const searchPosts = ({ body, username, createdAfter, createdBefore, includeReplies = true }) => {
    const params = new URLSearchParams();
    if (body) params.append('body', body);
    if (username) params.append('username', username);
    if (createdAfter) params.append('createdAfter', createdAfter);
    if (createdBefore) params.append('createdBefore', createdBefore);
    params.append('includeReplies', includeReplies);
    return request(`/posts/search?${params.toString()}`);
};

// Image upload for posts
export const uploadPostImage = (postId, file) => {
    const form = new FormData();
    form.append('file', file);
    return request(`/posts/${postId}/image`, { method: 'POST', body: form });
};

export const fetchPostImage = (postId) =>
    fetch(`${BASE_URL}/posts/${postId}/image`).then(res => {
        if (!res.ok) throw new Error('Image not found');
        return res.blob();
    });

// Comments (alias of posts replies)
export const postComment = (postId, userId, body) =>
    request('/comments', { method: 'POST', body: JSON.stringify({ postId, userId, body }) });

export const fetchCommentImage = (commentId) =>
    fetch(`${BASE_URL}/comments/${commentId}/image`).then(res => {
        if (!res.ok) throw new Error('Image not found');
        return res.blob();
    });

// Users
export const fetchUsers = () =>
    request('/users');

export const createUser = userDto =>
    request('/users', { method: 'POST', body: JSON.stringify(userDto) });