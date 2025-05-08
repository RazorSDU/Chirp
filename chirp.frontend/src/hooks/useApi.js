// src/hooks/useApi.js

import { useState, useEffect } from 'react';

/**
 * Generic hook for API calls.
 * @param {Function} apiFunc - A function returning a promise (e.g., a service call)
 * @param {...any} params - Parameters to pass to apiFunc
 */
export function useApi(apiFunc, ...params) {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        let cancelled = false;
        setLoading(true);
        setError(null);

        apiFunc(...params)
            .then(response => {
                if (!cancelled) setData(response);
            })
            .catch(err => {
                if (!cancelled) setError(err);
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [apiFunc, ...params]);

    return { data, loading, error };
}