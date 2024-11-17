import {useState} from "react";
import {useSignIn} from "../../features/auth/hooks/useSignIn.js";
import {useSignOut} from "../../features/auth/hooks/useSignOut.js";

const AuthPage = () => {

    const [accessCode, setAccessCode] = useState('');
    const [password, setPassword] = useState('');
    const fingerprint = 'string';
    const signInMutation = useSignIn();
    const signOutMutation = useSignOut();

    const handleSubmit = async (e) => {
        e.preventDefault();
        await signInMutation.mutateAsync({accessCode, password, fingerprint});
    };

    const signOut = async(e) => {
        e.preventDefault();
        await signOutMutation.mutateAsync();
    }

    return (

        <form onSubmit={handleSubmit}>
            <input
                type="text"
                placeholder="Username"
                value={accessCode}
                onChange={(e) => setAccessCode(e.target.value)}
            />
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <button type="submit" disabled={signInMutation.isLoading}>
                Sign In
            </button>
            <button onClick={signOut}>Sign out</button>
        </form>
    );
};

export default AuthPage;