import { useMutation } from 'react-query';
import { signIn } from '../api/authApi';
import { useAuthStore } from '../model/store';

export const useSignIn = () => {
    const { setAccessToken } = useAuthStore();

    return useMutation(signIn, {
        onSuccess: (data) => {
            if (data.succeed) {
                const responseBody = data.data;
                setAccessToken(responseBody.accessToken);
                localStorage.setItem('accessToken', responseBody.accessToken);
            }
        }
    });
};
