import apiClient from "../../../shared/api/apiClient.js";

export const createUser = async ({accessCode, lastName, firstName, middleName, birthDate, post}) => {

    const response = await apiClient.post('/user/create', { accessCode, lastName, firstName, middleName, birthDate, post }, {withCredentials: true});
    return response.data;
};

export const getUserPresentation = async({userId}) => {

    const response = await apiClient.get(`/user/presentation/${userId}`, {withCredentials: true});
    return response.data;
}