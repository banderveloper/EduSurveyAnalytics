import AuthForm from "../../components/AuthForm/AuthForm.jsx";
import useAuthStore from "../../stores/useAuthStore.js";
import HomePage from "../HomePage/HomePage.jsx";

const AuthPage = () => {

    const authStore = useAuthStore();

    if(authStore.isAuthenticated()) {
        return <HomePage/>
    }

    return (
        <>
          <h1>Authorization</h1>
          <AuthForm/>
        </>
    );
};

export default AuthPage;