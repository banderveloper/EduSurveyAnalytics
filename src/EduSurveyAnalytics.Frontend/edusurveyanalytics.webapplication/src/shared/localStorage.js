const LS_TOKEN_NAME = 'accessToken';

export const saveAccessToken = (accessToken) => {
    localStorage.setItem(LS_TOKEN_NAME, accessToken);
}

export const getAccessToken = () => {
    return localStorage.getItem(LS_TOKEN_NAME);
}

export const removeAccessToken = () => {
    localStorage.removeItem(LS_TOKEN_NAME);
}