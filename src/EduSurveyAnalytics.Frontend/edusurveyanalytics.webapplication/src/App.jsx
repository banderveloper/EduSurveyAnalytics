import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import Navbar from "./components/MainNavbar/MainNavbar.jsx";
import FormPickerPage from "./pages/FormPickerPage/FormPickerPage.jsx";
import {Container} from "react-bootstrap";
import {Route, Routes} from "react-router-dom";
import AuthPage from "./pages/AuthPage/AuthPage.jsx";
import HomePage from "./pages/HomePage/HomePage.jsx";
import SignOutPage from "./pages/SignOutPage/SignOutPage.jsx";

export default function App() {

    return (
        <>
            <Navbar />

            <Container className="mt-4">
                <Routes>
                    <Route path="/" element={<HomePage/>} />
                    <Route path="/auth" element={<AuthPage/>} />
                    <Route path="/form" element={<FormPickerPage/>} />
                    <Route path="/sign-out" element={<SignOutPage/>} />
                </Routes>
            </Container>
        </>
    )
}
