import {useState} from "react";
import {useSignIn} from "../../features/auth/hooks/useSignIn.js";

const AuthPage = () => {

    const [accessCode, setAccessCode] = useState('');
    const [password, setPassword] = useState('');
    const fingerprint = 'string';
    const mutation = useSignIn();

    const handleSubmit = async (e) => {
        e.preventDefault();
        await mutation.mutateAsync({accessCode, password, fingerprint});
    };

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
            <button type="submit" disabled={mutation.isLoading}>
                Sign In
            </button>
        </form>
    );
};

export default AuthPage;