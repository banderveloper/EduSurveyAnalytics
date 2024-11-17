import {useAuthStore} from "../model/store.js";
import {useMutation} from "react-query";
import {signOut} from "../api/authApi.js";

export const useSignOut = () => {

    const { setAccessToken } = useAuthStore();

    return useMutation(signOut, {
        onSuccess: (data) => {
            if(data.succeed){
                setAccessToken(null);
                localStorage.removeItem('accessToken');
            }
        }
    });
}
