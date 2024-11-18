import {create} from "zustand";
import apiClient from "../shared/axios.js";
import {ENDPOINTS} from "../shared/endpoints.js";

const useAnswersStore = create((set) => ({

    errorCode: null,
    isLoading: false,
    currentFormAnswers: [],
    currentFormTitle: null,

    createForm: async (formId, fieldAnswers) => {

        set({isLoading: true});

        const response = await apiClient.post(ENDPOINTS.ANSWERS.CREATE, {formId, fieldAnswers});
        const responseData = response.data;

        set({errorCode: responseData.errorCode});
        set({isLoading: false});
    },

    getFormAnswers: async (formId) => {

        set({isLoading: true});

        const response = await apiClient.get(ENDPOINTS.ANSWERS.GET_ANSWERS + formId);
        const responseData = response.data;

        if(responseData.succeed) {
            set({currentFormTitle: responseData.data.formTitle});
            set({currentFormAnswers: responseData.data.answers});
        }

        set({errorCode: responseData.errorCode});
        set({isLoading: false});
    }
}));

export default useAnswersStore;