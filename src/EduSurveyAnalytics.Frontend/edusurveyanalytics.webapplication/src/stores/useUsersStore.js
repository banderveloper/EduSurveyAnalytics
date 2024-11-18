import apiClient from "../shared/axios.js";
import {ENDPOINTS} from "../shared/endpoints.js";
import {create} from "zustand";


const useUsersStore = create((set) => ({

    errorCode: null,
    isLoading: false,
    currentUserPresentation: null,
    currentUserFullData: null,

    createUser: async (accessCode, lastName, firstName, middleName, birthDate, post) => {

        set({isLoading: true});

        const response = await apiClient.post(ENDPOINTS.USER.CREATE, {
            accessCode,
            lastName,
            firstName,
            middleName,
            birthDate,
            post
        });
        const responseData = response.data;

        if (responseData.succeed) {
            set({currentUserPresentation: responseData.data});
        }

        set({errorCode: responseData.errorCode});
        set({isLoading: false});
    },

    setPassword: async (password) => {
        set({isLoading: true});

        const response = await apiClient.post(ENDPOINTS.USER.SET_PASSWORD, {password});
        const responseData = response.data;

        set({errorCode: responseData.errorCode});
        set({isLoading: false});
    },

    updateUser: async (userId, accessCode, lastName, firstName, middleName, birthDate, post, permissions) => {
        set({isLoading: true});

        const response = await apiClient.post(ENDPOINTS.USER.UPDATE,
            {userId, accessCode, lastName, firstName, middleName, birthDate, post, permissions});
        const responseData = response.data;

        set({errorCode: responseData.errorCode});
        set({isLoading: false});
    },

    deleteUser: async (userId) => {

        set({isLoading: true});

        const response = await apiClient.post(ENDPOINTS.USER.DELETE, {userId});
        const responseData = response.data;

        set({errorCode: responseData.errorCode});
        set({isLoading: false});
    },

    getUserPresentation: async (userId) => {
        set({isLoading: true});

        const response = await apiClient.get(ENDPOINTS.USER.GET_USER_PRESENTATION + userId);
        const responseData = response.data;

        set({currentUserPresentation: responseData.data.user});
        set({errorCode: responseData.errorCode});
        set({isLoading: false});
    },

    getUserFullData: async(userId) => {
        set({isLoading: true});

        const response = await apiClient.get(ENDPOINTS.USER.GET_USER_FULL_DATA + userId);
        const responseData = response.data;

        set({currentUserFullData: responseData.data.user});
        set({errorCode: responseData.errorCode});
        set({isLoading: false});
    }
}));

export default useUsersStore;