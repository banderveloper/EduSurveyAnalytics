import {create} from "zustand";
import apiClient from "../shared/axios.js";
import {ENDPOINTS} from "../shared/endpoints.js";


const useFormsStore = create((set) => ({

    errorCode: null,
    isLoading: false,
    currentForm: null,
    createdFormId: null,

    getForm: async(formId) => {

        set({isLoading: true});

        const response = await apiClient.get(ENDPOINTS.FORMS.GET + formId);
        const responseData = response.data;

        set({currentForm: responseData.data});
        set({errorCode: responseData.errorCode});
        set({isLoading: false});
    },

    createForm: async(formTitle, formFields) => {

        set({isLoading: true});

        const response = await apiClient.post(ENDPOINTS.FORMS.CREATE, {formFields, formTitle});
        const responseData = response.data;

        set({errorCode: responseData.errorCode});
        set({isLoading: false});

        if(responseData.succeed){
            set({createdFormId: responseData.data.formId});
        }
        else{
            set({createdFormId: null})
        }
    }
}));

export default useFormsStore;