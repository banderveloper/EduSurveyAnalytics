export const API_URL = 'http://195.234.5.107';

export const ENDPOINTS = {
    AUTH: {
        SIGN_IN: `${API_URL}/auth/sign-in`,
        REFRESH: `${API_URL}/auth/refresh`,
        SIGN_OUT: `${API_URL}/auth/sign-out`,
    },
    USER: {
        CREATE: `${API_URL}/user/create`,
        SET_PASSWORD: `${API_URL}/user/set-password`,
        UPDATE: `${API_URL}/user/update`,
        DELETE: `${API_URL}/user/delete`,
        GET_USER_PRESENTATION: `${API_URL}/user/presentation/`,
        GET_USER_FULL_DATA: `${API_URL}/user/full/`,
    },
    ANSWERS: {
        CREATE: `${API_URL}/answer/create`,
        GET_ANSWERS: `${API_URL}/answer/get/`,
    },
    FORMS: {
        GET: `${API_URL}/form/get/`,
        CREATE: `${API_URL}/form/create`,
        GET_SHORTS: `${API_URL}/form/get/shorts`,
    }
};