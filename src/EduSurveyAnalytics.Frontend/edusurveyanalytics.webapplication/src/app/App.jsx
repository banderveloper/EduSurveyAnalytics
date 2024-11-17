import './styles/App.css'
import {ReactQueryProvider} from './providers/ReactQueryProvider.jsx'
import AuthPage from "../pages/auth/AuthPage.jsx";

export default function App() {

    return (
        <ReactQueryProvider>
            <h1>Hello world</h1>
            <AuthPage/>
        </ReactQueryProvider>
    )
}
