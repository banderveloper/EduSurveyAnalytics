import useAuthStore from "../../stores/useAuthStore.js";
import AuthPage from "../AuthPage/AuthPage.jsx";

const HomePage = () => {

    const authStore = useAuthStore();

    if(!authStore.isAuthenticated()){
        return <AuthPage/>
    }

    return (
        <h1>Welcome</h1>
    );
};

export default HomePage;