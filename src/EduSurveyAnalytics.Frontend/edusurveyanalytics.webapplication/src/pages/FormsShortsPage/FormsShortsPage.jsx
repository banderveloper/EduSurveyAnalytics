import React, {useEffect, useState} from 'react';
import { Container, Row, Col, Card } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import useFormsStore from "../../stores/useFormsStore.js";

const FormsShortsPage = () => {

    const formsStore = useFormsStore();
    const [shorts, setShorts] = useState([]);

    useEffect(() => {

        const fetch = async () => {
            setShorts(await formsStore.getShorts())
        }

        fetch()

    }, [])

    return (
        <Container>
            <Row>
                {shorts.map((form) => (
                    <Col key={form.id} xs={12} md={6} lg={4} className="mb-4">
                        <Card>
                            <Card.Body>
                                <Card.Title>
                                    <Link to={`/form?id=${form.id}`} className="text-decoration-none">
                                        {form.title}
                                    </Link>
                                </Card.Title>
                                <Card.Subtitle className="mb-2 text-muted">{form.ownerName}</Card.Subtitle>
                                <Card.Text>
                                    <strong>Last Updated:</strong>{' '}
                                    {new Date(form.updatedAt).toLocaleString()}
                                </Card.Text>
                            </Card.Body>
                        </Card>
                    </Col>
                ))}
            </Row>
        </Container>
    );
};

export default FormsShortsPage;
