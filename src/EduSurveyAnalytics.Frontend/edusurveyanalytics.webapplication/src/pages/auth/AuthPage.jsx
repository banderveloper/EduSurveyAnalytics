import {useState} from "react";
import {useSignIn} from "../../features/auth/hooks/useSignIn.js";
import {useSignOut} from "../../features/auth/hooks/useSignOut.js";
import {getUserPresentation} from "../../features/users/api/usersApi.js";

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

    const createUser = async(e) => {
        e.preventDefault();

        let a = await getUserPresentation({userId: '93b1cda6-61d4-4791-ad8f-c2f11f5d1d49'})
        console.log(a.data);
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
            <button onClick={createUser}>Get user</button>
        </form>
    );
};

export default AuthPage;