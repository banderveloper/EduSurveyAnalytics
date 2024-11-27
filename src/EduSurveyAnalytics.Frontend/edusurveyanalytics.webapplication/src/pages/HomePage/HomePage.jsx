import useAuthStore from "../../stores/useAuthStore.js";
import {useNavigate} from "react-router-dom";
import {useEffect} from "react";

const HomePage = () => {

    const authStore = useAuthStore();
    const navigate = useNavigate();

    useEffect(() => {
        if(!authStore.isAuthenticated()) {
            navigate("/auth");
        }
    }, [authStore]);

    return (
        <h1>Welcome</h1>
    );
};

export default HomePage;