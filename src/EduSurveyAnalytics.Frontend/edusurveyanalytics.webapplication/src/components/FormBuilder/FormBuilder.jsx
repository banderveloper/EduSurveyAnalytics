import { useState } from 'react';
import { Form, Button, Row, Col } from 'react-bootstrap';
import useFormsStore from "../../stores/useFormsStore.js";
import {useNavigate} from "react-router-dom";

const FormBuilder = () => {

    const formsStore = useFormsStore();
    const navigate = useNavigate();

    const [formTitle, setFormTitle] = useState('');
    const [formFields, setFormFields] = useState([]);

    const addField = () => {
        setFormFields([
            ...formFields,
            { title: '', order: formFields.length, constraints: [] }
        ]);
    };

    const deleteField = (index) => {
        setFormFields(formFields.filter((_, i) => i !== index));
    };

    const updateField = (index, key, value) => {
        const updatedFields = [...formFields];
        updatedFields[index][key] = value;
        setFormFields(updatedFields);
    };

    const toggleConstraint = (index, constraint) => {
        const updatedFields = [...formFields];
        const constraints = updatedFields[index].constraints;
        if (constraints.includes(constraint)) {
            updatedFields[index].constraints = constraints.filter(c => c !== constraint);
        } else {
            updatedFields[index].constraints.push(constraint);
        }
        setFormFields(updatedFields);
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        formsStore.createForm(formTitle, formFields);
    };

    const handleVisitForm = () => {
        navigate('/form?id=' + formsStore.createdFormId)
    }

    return (
        <div className="container mt-4">
            <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3">
                    <Form.Label>Form Title</Form.Label>
                    <Form.Control
                        type="text"
                        value={formTitle}
                        onChange={(e) => setFormTitle(e.target.value)}
                        placeholder="Enter form title"
                    />
                </Form.Group>

                {formFields.map((field, index) => (
                    <div key={index} className="mb-3 border p-3 position-relative">
                        <Row>
                            <Col>
                                <Form.Group>
                                    <Form.Label>Field Title</Form.Label>
                                    <Form.Control
                                        type="text"
                                        value={field.title}
                                        onChange={(e) => updateField(index, 'title', e.target.value)}
                                        placeholder="Enter field title"
                                    />
                                </Form.Group>
                            </Col>
                            <Col>
                                <Form.Group>
                                    <Form.Label>Order</Form.Label>
                                    <Form.Control
                                        type="number"
                                        value={field.order}
                                        onChange={(e) => updateField(index, 'order', Number(e.target.value))}
                                        placeholder="Field order"
                                        min="0"
                                    />
                                </Form.Group>
                            </Col>
                        </Row>
                        <Form.Group className="mt-2">
                            <Form.Check
                                type="checkbox"
                                label="Required"
                                checked={field.constraints.includes('required')}
                                onChange={() => toggleConstraint(index, 'required')}
                            />
                            <Form.Check
                                type="checkbox"
                                label="Numbers Only"
                                checked={field.constraints.includes('numbersonly')}
                                onChange={() => toggleConstraint(index, 'numbersonly')}
                            />
                        </Form.Group>
                        <Button
                            variant="danger"
                            size="sm"
                            className="position-absolute top-0 end-0 m-2"
                            onClick={() => deleteField(index)}
                        >
                            Delete Field
                        </Button>
                    </div>
                ))}

                <Row>
                    <Col className="d-flex justify-content-start">
                        <Button variant="secondary" onClick={addField} className="me-2">
                            Add Field
                        </Button>
                        <Button variant="primary" type="submit" className="me-2">
                            Submit Form
                        </Button>
                        {
                            formsStore.createdFormId &&
                            <Button variant="success" onClick={handleVisitForm} className="me-2">
                                Visit created form
                            </Button>
                        }
                    </Col>
                </Row>

            </Form>
        </div>
    );
};

export default FormBuilder;
