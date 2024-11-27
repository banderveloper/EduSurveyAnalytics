import React, {useEffect} from 'react';
import useUsersStore from "../../stores/useUsersStore.js";
import {Card, Container, ListGroup} from "react-bootstrap";

// eslint-disable-next-line react/prop-types
const UserPresentationInfoBlock = ({userId}) => {

    const usersStore = useUsersStore();

    useEffect(() => {
        usersStore.getUserPresentation(userId)
    }, [])

    if (!usersStore.currentUserPresentation) {
        return <h2 className='text-danger'>User not found</h2>
    }

    return (
        <Container className="mt-4">
            <Card>
                <Card.Body>
                    <Card.Title>User Information</Card.Title>
                    <ListGroup variant="flush">
                        <ListGroup.Item><strong>Last
                            Name:</strong> {usersStore.currentUserPresentation.lastName}</ListGroup.Item>
                        <ListGroup.Item><strong>First
                            Name:</strong> {usersStore.currentUserPresentation.firstName}</ListGroup.Item>
                        <ListGroup.Item><strong>Middle
                            Name:</strong> {usersStore.currentUserPresentation.middleName}</ListGroup.Item>
                        <ListGroup.Item><strong>Birth
                            Date:</strong> {new Date(usersStore.currentUserPresentation.birthDate).toLocaleDateString()}
                        </ListGroup.Item>
                        <ListGroup.Item><strong>Post:</strong> {usersStore.currentUserPresentation.post}
                        </ListGroup.Item>
                    </ListGroup>
                </Card.Body>
            </Card>
        </Container>
    );
};

export default UserPresentationInfoBlock;