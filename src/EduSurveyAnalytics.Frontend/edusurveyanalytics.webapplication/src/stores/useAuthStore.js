import {create} from 'zustand';
import apiClient from "../shared/axios.js";
import {ENDPOINTS} from "../shared/endpoints.js";
import {
    getAccessToken, getPermissions,
    removeAccessToken,
    removePermissions,
    saveAccessToken,
    savePermissions
} from "../shared/localStorage.js";
import {PERMISSION} from "../shared/enums/permissions.js";

const useAuthStore = create((set,get) => ({

    accessToken: null,  // Store access token
    errorCode: null,
    isLoading: false,
    permissions: [],          // Store roles array

    signIn: async (accessCode, password, fingerprint) => {

        set({isLoading: true});

        const response = await apiClient.post(ENDPOINTS.AUTH.SIGN_IN, {accessCode, password, fingerprint});
        const responseData = response.data;

        if (responseData.succeed) {
            saveAccessToken(responseData.data.accessToken);
            savePermissions(response.data.data.permissions);

            set({accessToken: response.data.data.accessToken});
            set({permissions: response.data.data.permissions});
        }

        set({errorCode: responseData.errorCode});
        set({isLoading: false});
    },

    refresh: async () => {
        set({isLoading: true});

        const response = await apiClient.post(ENDPOINTS.AUTH.REFRESH);
        const responseData = response.data;

        if (responseData.succeed) {
            saveAccessToken(responseData.data.accessToken);
            set({accessToken: response.data.data.accessToken});
        }

        set({errorCode: responseData.errorCode});
        set({isLoading: false});
    },

    signOut: async () => {

        set({isLoading: true});

        const response = await apiClient.post(ENDPOINTS.AUTH.SIGN_OUT);
        const responseData = response.data;

        set({accessToken: null});
        removeAccessToken();

        set({errorCode: responseData.errorCode});
        set({isLoading: false});
    },

    clear: () => {
        set({accessToken: null})
        set({permissions: []})
        removeAccessToken();
        removePermissions();
    },

    isAuthenticated: () => {
        return get().accessToken || getAccessToken();
    },

    getPermissions: () => {
        const inMemoryPermissions = get().permissions;

        if(inMemoryPermissions > 0) {
            return inMemoryPermissions;
        }

        return getPermissions();
    },

    hasPermission: (permission) => {
        const userPermissions = get().getPermissions();
        return userPermissions.includes(permission) || userPermissions.includes(PERMISSION.ALL);
    }
}));

export default useAuthStore;