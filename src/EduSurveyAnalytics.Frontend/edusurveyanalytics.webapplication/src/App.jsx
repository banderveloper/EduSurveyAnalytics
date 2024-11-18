import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import Navbar from "./components/MainNavbar/MainNavbar.jsx";
import AuthForm from "./components/AuthForm/AuthForm.jsx";

export default function App() {

    return (
        <>
            <Navbar/>

            <div className="container pt-5">
                <AuthForm/>
            </div>
        </>
    )
}
