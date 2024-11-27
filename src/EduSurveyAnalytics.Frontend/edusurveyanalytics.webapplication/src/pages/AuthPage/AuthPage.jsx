import AuthForm from "../../components/AuthForm/AuthForm.jsx";
import useAuthStore from "../../stores/useAuthStore.js";
import {useNavigate} from "react-router-dom";
import {useEffect} from "react";

const AuthPage = () => {

    const authStore = useAuthStore();
    const navigate = useNavigate();

    useEffect(() => {
        if(authStore.isAuthenticated()) {
            navigate("/");
        }
    }, [authStore]);


    return (
        <>
          <h1>Authorization</h1>
          <AuthForm/>
        </>
    );
};

export default AuthPage;