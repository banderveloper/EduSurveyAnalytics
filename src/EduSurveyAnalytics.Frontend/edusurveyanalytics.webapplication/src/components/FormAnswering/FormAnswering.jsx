import {useEffect, useState} from 'react';
import {Button, Container, Form} from "react-bootstrap";
import useAnswersStore from "../../stores/useAnswersStore.js";
import useAuthStore from "../../stores/useAuthStore.js";
import {FORM_FIELD_CONSTRAINTS} from "../../shared/enums/formFieldConstraints.js";

const FormAnswering = (data) => {

    data = data.data;

    const authStore = useAuthStore();
    const answersStore = useAnswersStore();

    // todo remove
    useEffect(() => {
        authStore.signIn('admin',  'admin', 'web')
    }, []);

    const [answers, setAnswers] = useState(
        data.fields.map((field) => ({
            formFieldId: field.id,
            value: "", // Initialize with empty value
        }))
    );

    const handleFieldChange = (index, value) => {
        const updatedAnswers = [...answers];
        updatedAnswers[index].value = value; // Update the answer for the respective field
        setAnswers(updatedAnswers);
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        const submissionData = {
            formId: data.id,
            fieldAnswers: answers,
        };
        console.log("Submitted Data:", submissionData);
        // Here you can send `submissionData` to an API or process it further
        answersStore.createForm(data.id, answers);
    };

    const getInputType = (constraints) => {
        // Check for 'numbers_only' constraint and return 'number' type for that field
        if (constraints.includes(FORM_FIELD_CONSTRAINTS.NUMBERS_ONLY)) {
            return "number";
        }
        return "text"; // Default to text input type
    };

    const isRequired = (constraints) => {
        // Check for 'required' constraint and return true if present
        return constraints.includes(FORM_FIELD_CONSTRAINTS.REQUIRED);
    };

    return (
        <Container className="mt-4">
            <Form onSubmit={handleSubmit}>
                {/* Display labels for ownerName, ownerPost, createdAt, and updatedAt */}
                <Form.Group className="mb-3">
                    <Form.Label><strong>Owner Name:</strong> {data.ownerName}</Form.Label>
                </Form.Group>
                <Form.Group className="mb-3">
                    <Form.Label><strong>Owner Post:</strong> {data.ownerPost}</Form.Label>
                </Form.Group>
                <Form.Group className="mb-3">
                    <Form.Label><strong>Created At:</strong> {data.createdAt}</Form.Label>
                </Form.Group>
                <Form.Group className="mb-3">
                    <Form.Label><strong>Updated At:</strong> {data.updatedAt}</Form.Label>
                </Form.Group>

                {/* Render fields with title as label and input for user answer */}
                <h5>Fields</h5>
                {data.fields.map((field, index) => (
                    <Form.Group className="mb-3" key={field.id}>
                        <Form.Label>{field.title}</Form.Label>
                        <Form.Control
                            type={getInputType(field.constraints)} // Determine input type based on constraints
                            placeholder={`Enter your answer for "${field.title}"`}
                            value={answers[index].value}
                            onChange={(e) => handleFieldChange(index, e.target.value)}
                            required={isRequired(field.constraints)} // Apply required constraint
                        />
                    </Form.Group>
                ))}

                <Button variant="primary" type="submit" disabled={answersStore.isLoading}>
                    Submit
                </Button>
            </Form>
        </Container>
    );
};


export default FormAnswering;