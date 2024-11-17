import axios from 'axios';
import useAuthStore from "../stores/useAuthStore.js";
import {API_URL} from "./endpoints.js";
import {getAccessToken} from "./localStorage.js";

const apiClient = axios.create({
    baseURL: API_URL,
    withCredentials: true
});

apiClient.interceptors.request.use((config) => {

    console.log('===== INTERCEPTOR REQUEST =====');
    console.log(config.data);

    const token = getAccessToken();
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

            useAuthStore.getState().refresh().then(result => {

                let response = result.data;
                console.log('CHEEEL')
                console.log(response);

                if(!response.succeed) {
                    // send to auth page
                    return Promise.reject(error);
                }

                localStorage.setItem('accessToken', response.data.accessToken);
                error.config.headers.Authorization = `Bearer ${response.data.accessToken}`;
                return apiClient.request(error.config);
            });
        }
        return Promise.reject(error);
    }
);

export default apiClient;