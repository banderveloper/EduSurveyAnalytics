import './App.css'
import useAuthStore from "./stores/useAuthStore.js";
import useUsersStore from "./stores/useUsersStore.js";
import useFormsStore from "./stores/useFormsStore.js";
import useAnswersStore from "./stores/useAnswersStore.js";

export default function App() {

    const authStore = useAuthStore();
    const userStore = useUsersStore();
    const formsStore = useFormsStore();
    const answersStore = useAnswersStore();

    console.log('Form data:')
    console.log(formsStore.currentForm);

    return (
        <div>
            <h1>{authStore.isLoading ? 'LOADING' : "HELLO WORLD"}</h1>

            <button onClick={() => {
                authStore.signIn('admin', 'admin', 'string');
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

            <button onClick={() => {
                userStore.createUser('damnboy', 'damnboy', 'damnboy', 'damnboy', '2004-04-04', 'damnboy');
            }}>Create user
            </button>

            <button onClick={() => {
                userStore.setPassword('Defaultpass1.')
            }}>Set password
            </button>

            <button onClick={() => {
                userStore.getUserPresentation('4fa68881-51f5-4bb0-88f5-071c236788e1');
            }}>Get presentation
            </button>

            <button onClick={() => {
                userStore.getUserFullData('4fa68881-51f5-4bb0-88f5-071c236788e1');
            }}>Get full
            </button>

            <button onClick={() => {
                answersStore.getFormAnswers('ce856fec-3386-494e-a835-0fad2a44d6d3')
            }}>Get answers
            </button>

        </div>
    )
}
