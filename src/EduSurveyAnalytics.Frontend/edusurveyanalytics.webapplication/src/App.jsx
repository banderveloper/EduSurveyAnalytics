import './App.css'
import useAuthStore from "./stores/useAuthStore.js";

export default function App() {

    const authStore = useAuthStore();

    return (
        <div>
            <h1>{authStore.isLoading ? 'LOADING' : "HELLO WORLD"}</h1>
            b
            <button onClick={() => {
                authStore.signIn('string', 'string', 'string');
            }}>Sign in
            </button>

            <button onClick={() => {
                authStore.refresh();
            }}>Refresh
            </button>

            <button onClick={() => {
                authStore.signOut();
            }}>Sign out
            </button>

        </div>
    )
}
