// import apiClient from '~/shared/api/apiClient';

import apiClient from "../../../shared/api/apiClient.js";

export const signIn = async ({accessCode, password, fingerprint}) => {
    console.log(`SIGN IN DATA ${accessCode}: ${password} : ${fingerprint}`);
    const response = await apiClient.post('/auth/sign-in', { accessCode, password, fingerprint }, {withCredentials: true});
    return response.data;
};

export const refreshAccessToken = async () => {
    const response = await apiClient.post('/auth/refresh', {}, { withCredentials: true });
    return response.data;
};

export const signOut = async () => {
    const response = await apiClient.post('/auth/sign-out', {}, { withCredentials: true });
    return response.data;
}
