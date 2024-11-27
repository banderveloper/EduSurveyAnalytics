import useAuthStore from "../../stores/useAuthStore.js";
import AuthPage from "../AuthPage/AuthPage.jsx";
import {useEffect} from "react";

const SignOutPage = () => {

    const authStore = useAuthStore();

    useEffect(() => {
        authStore.signOut();
        authStore.clear()
    }, []);

    return <AuthPage/>
};

export default SignOutPage;