import React, {useState} from "react";
import {Accordion, Button, Card, Form} from "react-bootstrap";
import useAnswersStore from "../../stores/useAnswersStore.js";
import FormAnswersList from "../../components/FormAnswersList/FormAnswersList.jsx";
import {getErrorMessage} from "../../shared/errorDescriptions.js";
import FormAnswersAnalytics from "../../components/FormAnswersAnalytics/FormAnswersAnalytics.jsx";

const FormAnswersPage = () => {

    const [formId, setFormId] = useState("");
    const answersStore = useAnswersStore();

    const handleSubmit = (e) => {
        e.preventDefault();
        answersStore.getFormAnswers(formId);
    };

    const isValidGuid = (guid) => {
        const regex = /^[{]?[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}[}]?$/;
        return regex.test(guid);
    }

    return (
        <>
            <h1>Form answers</h1>

            <Form onSubmit={handleSubmit}>
                <Form.Group controlId="formIdInput">
                    <Form.Label>Form ID</Form.Label>
                    <Form.Control
                        type="text"
                        placeholder="Enter Form ID"
                        value={formId}
                        onChange={(e) => setFormId(e.target.value)}
                        required
                    />
                </Form.Group>
                <Button variant="primary" type="submit" className="mt-3" disabled={!isValidGuid(formId)}>
                    Submit
                </Button>
            </Form>

            <hr className="my-4"/>

            {
                answersStore.errorCode !== null
                    ? <h3 className='text-danger'>{getErrorMessage(answersStore.errorCode)}</h3>
                    :
                    answersStore.currentFormTitle &&
                    <Accordion flush>
                        <h3 className='my-4'>{answersStore.currentFormTitle} ({answersStore.currentFormAnswersCount} answers)</h3>
                        <Accordion.Item eventKey="0">
                            <Accordion.Header>
                                <h4>Answers analytics</h4>
                            </Accordion.Header>
                            <Accordion.Body>
                                <FormAnswersAnalytics formData={answersStore.currentFormAnswers} />
                            </Accordion.Body>
                        </Accordion.Item>
                        <Accordion.Item eventKey="1">
                            <Accordion.Header>
                                <h4>All answers</h4>
                            </Accordion.Header>
                            <Accordion.Body>
                                <FormAnswersList formData={answersStore.currentFormAnswers}/>
                            </Accordion.Body>
                        </Accordion.Item>
                    </Accordion>

            }
        </>
    );
};

export default FormAnswersPage;