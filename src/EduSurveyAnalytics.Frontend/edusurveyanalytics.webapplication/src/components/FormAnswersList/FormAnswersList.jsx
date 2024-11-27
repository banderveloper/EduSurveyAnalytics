import {Card, ListGroup} from "react-bootstrap";

// eslint-disable-next-line react/prop-types
const FormAnswersList = ({formData}) => {

    return (
        <div className="form-answers">
            {/* eslint-disable-next-line react/prop-types */}
            {formData.map((item, index) => (
                <Card key={index} className="mb-4">
                    <Card.Header>{item.title}</Card.Header>
                    <ListGroup variant="flush">
                        {item.answers.map((answer, ansIndex) => (
                            <ListGroup.Item key={ansIndex} className={ansIndex % 2 === 1 ? 'bg-light' : ''}>
                                <div><strong>User:</strong> {answer.userName}</div>
                                <div><strong>Answer:</strong> {answer.value || "[No answer provided]"}</div>
                                <div><strong>Answered At:</strong> {new Date(answer.answeredAt).toLocaleString()}</div>
                            </ListGroup.Item>
                        ))}
                    </ListGroup>
                </Card>
            ))}
        </div>
    );
};

export default FormAnswersList;