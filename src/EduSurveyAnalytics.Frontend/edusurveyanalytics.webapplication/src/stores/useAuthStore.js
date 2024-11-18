import {create} from 'zustand';
import apiClient from "../shared/axios.js";
import {ENDPOINTS} from "../shared/endpoints.js";
import {removeAccessToken, saveAccessToken} from "../shared/localStorage.js";

const useAuthStore = create((set) => ({

    accessToken: null,  // Store access token
    errorCode: null,
    isAuthenticated: () => (state) => state.accessToken !== null,
    isLoading: false,
    roles: [],          // Store roles array

    signIn: async (accessCode, password, fingerprint) => {

        set({isLoading: true});

        const response = await apiClient.post(ENDPOINTS.AUTH.SIGN_IN, {accessCode, password, fingerprint});
        const responseData = response.data;

        if (responseData.succeed) {
            saveAccessToken(responseData.data.accessToken);
            set({accessToken: response.data.data.accessToken});
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
        set({roles: []})
        removeAccessToken();
    }
}));

export default useAuthStore;