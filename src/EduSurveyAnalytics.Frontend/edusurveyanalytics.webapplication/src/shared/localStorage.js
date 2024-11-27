const LS_TOKEN_NAME = 'accessToken';
const LS_PERMISSIONS_NAME = 'roles';

export const saveAccessToken = (accessToken) => {
    localStorage.setItem(LS_TOKEN_NAME, accessToken);
}

export const getAccessToken = () => {
    return localStorage.getItem(LS_TOKEN_NAME);
}

export const removeAccessToken = () => {
    localStorage.removeItem(LS_TOKEN_NAME);
}

export const savePermissions = (permissions) => {
    localStorage.setItem(LS_PERMISSIONS_NAME, JSON.stringify(permissions));
}

export const getPermissions = () => {
    const permissionsStr = localStorage.getItem(LS_PERMISSIONS_NAME);
    if(!permissionsStr) return [];

    return JSON.parse(permissionsStr);
}

export const removePermissions = () => {
    localStorage.removeItem(LS_PERMISSIONS_NAME);
}