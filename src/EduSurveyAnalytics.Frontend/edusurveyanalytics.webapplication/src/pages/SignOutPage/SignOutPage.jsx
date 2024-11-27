import useAuthStore from "../../stores/useAuthStore.js";
import {useEffect} from "react";
import {useNavigate} from "react-router-dom";

const SignOutPage = () => {

    const authStore = useAuthStore();
    const navigate = useNavigate();

    useEffect(() => {
        authStore.signOut();
        authStore.clear()
        navigate("/auth");
    }, []);
};

export default SignOutPage;