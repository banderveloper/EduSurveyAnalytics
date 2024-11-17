import axios from 'axios';
import {refreshAccessToken} from "../../features/auth/api/authApi.js";

const apiClient = axios.create({
    baseURL: 'http://localhost:5035',
});

apiClient.interceptors.request.use((config) => {

    console.log('===== INTERCEPTOR REQUEST =====');
    console.log(config.data);

    const token = localStorage.getItem('accessToken');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

apiClient.interceptors.response.use(
    (response) => {
        console.log('===== INTERCEPTOR RESPONSE SUCCEED =====');
        console.log(response.data);

        return response;
    },
    async (error) => {
        console.log('===== INTERCEPTOR RESPONSE ERROR =====');
        if (error.response?.status === 401) {
            console.log('UNAUTHORIZED');
            const refreshResponse = await refreshAccessToken();

            if(!refreshResponse.succeed) {
                // send to auth page
                return Promise.reject(error);
            }

            localStorage.setItem('accessToken', refreshResponse.data.accessToken);
            error.config.headers.Authorization = `Bearer ${refreshResponse.data.accessToken}`;
            return apiClient.request(error.config);
        }
        return Promise.reject(error);
    }
);

export default apiClient;
