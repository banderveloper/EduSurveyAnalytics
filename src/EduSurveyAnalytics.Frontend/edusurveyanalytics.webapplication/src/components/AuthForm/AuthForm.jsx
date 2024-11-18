import {Button, Form} from "react-bootstrap";
import useAuthStore from "../../stores/useAuthStore.js";
import {getErrorMessage} from "../../shared/errorDescriptions.js";
import {useState} from "react";

const AuthForm = () => {

    const authStore = useAuthStore();

    const [accessCode, setAccessCode] = useState("");
    const [password, setPassword] = useState("");

    const handleSubmit = (e) => {
        e.preventDefault();
        authStore.signIn(accessCode, password, 'web');
    }

    return (
        <Form onSubmit={handleSubmit}>
            <Form.Group className="mb-3" controlId="formBasicEmail">
                <Form.Label>Access code</Form.Label>
                <Form.Control type="text" placeholder="Enter access code" value={accessCode} onChange={(e) => setAccessCode(e.target.value)} />
                <Form.Text className="text-muted">
                    Well never share your access code with anyone else.
                </Form.Text>
            </Form.Group>

            <Form.Group className="mb-3" controlId="formBasicPassword">
                <Form.Label>Password</Form.Label>
                <Form.Control type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} />
            </Form.Group>

            {
                authStore.errorCode &&
                <Form.Text className="text-danger d-block mb-3">
                    {getErrorMessage(authStore.errorCode)}
                </Form.Text>
            }

            <Button variant="primary" type="submit" disabled={authStore.isLoading}>
                Submit
            </Button>
        </Form>
    );
};

export default AuthForm;