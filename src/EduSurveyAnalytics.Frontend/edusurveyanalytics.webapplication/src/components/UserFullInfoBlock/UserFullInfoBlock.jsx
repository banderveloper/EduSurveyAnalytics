import React, { useEffect, useState } from 'react';
import { Form, Button, Col, Row, Container } from 'react-bootstrap';
import useUsersStore from "../../stores/useUsersStore.js";
import {useNavigate} from "react-router-dom";

// eslint-disable-next-line react/prop-types
const UserFullInfoBlock = ({ userId }) => {
    const usersStore = useUsersStore();
    const navigate = useNavigate();

    const [canEditUsers, setCanEditUsers] = useState(false);

    useEffect(() => {
        usersStore.getUserFullData(userId);
    }, []);

    const handleChange = (e) => {
        const { name, value } = e.target;
        usersStore.updateCurrentUserFullData({ [name]: value });
    };

    const handlePermissionChange = (e) => {
        const { value, checked } = e.target;
        const currentPermissions = usersStore.currentUserFullData.permissions || [];
        const updatedPermissions = checked
            ? [...currentPermissions, value]
            : currentPermissions.filter((permission) => permission !== value);

        usersStore.updateCurrentUserFullData({ permissions: updatedPermissions });
    };

    const handleSubmit = async () => {
        const { currentUserFullData } = usersStore;
        if (!currentUserFullData) return;

        const {
            userId,
            accessCode,
            lastName,
            firstName,
            middleName,
            birthDate,
            post,
            permissions,
        } = currentUserFullData;

        await usersStore.updateUser(
            userId,
            accessCode,
            lastName,
            firstName,
            middleName,
            birthDate,
            post,
            permissions
        );

        console.log(JSON.stringify(currentUserFullData, null, 2));
    };

    const handleDeleteUser = async () => {
        const { currentUserFullData } = usersStore;
        if (currentUserFullData?.userId) {
            await usersStore.deleteUser(currentUserFullData.userId);
            navigate("/");
        }
    };

    const permissionDisplay = (permission) =>
        permission
            .replace(/_/g, " ")
            .replace(/\b\w/g, (char) => char.toUpperCase());

    const { currentUserFullData } = usersStore;

    if (!currentUserFullData) {
        return <h2 className="text-danger">User not found</h2>;
    }

    return (
        <Container>
            <Form>
                {/* User ID - Non-editable */}
                <Row className="mb-3">
                    <Col sm={3}>
                        <strong>User ID:</strong>
                    </Col>
                    <Col>
                        <span>{currentUserFullData.userId}</span>
                    </Col>
                </Row>

                {canEditUsers ? (
                    <>
                        <Row className="mb-3">
                            <Col sm={3}>
                                <strong>Access Code:</strong>
                            </Col>
                            <Col>
                                <Form.Control
                                    type="text"
                                    name="accessCode"
                                    value={currentUserFullData.accessCode || ""}
                                    onChange={handleChange}
                                />
                            </Col>
                        </Row>
                        <Row className="mb-3">
                            <Col sm={3}>
                                <strong>Last Name:</strong>
                            </Col>
                            <Col>
                                <Form.Control
                                    type="text"
                                    name="lastName"
                                    value={currentUserFullData.lastName || ""}
                                    onChange={handleChange}
                                />
                            </Col>
                        </Row>
                        <Row className="mb-3">
                            <Col sm={3}>
                                <strong>First Name:</strong>
                            </Col>
                            <Col>
                                <Form.Control
                                    type="text"
                                    name="firstName"
                                    value={currentUserFullData.firstName || ""}
                                    onChange={handleChange}
                                />
                            </Col>
                        </Row>
                        <Row className="mb-3">
                            <Col sm={3}>
                                <strong>Middle Name:</strong>
                            </Col>
                            <Col>
                                <Form.Control
                                    type="text"
                                    name="middleName"
                                    value={currentUserFullData.middleName || ""}
                                    onChange={handleChange}
                                />
                            </Col>
                        </Row>
                        <Row className="mb-3">
                            <Col sm={3}>
                                <strong>Birth Date:</strong>
                            </Col>
                            <Col>
                                <Form.Control
                                    type="date"
                                    name="birthDate"
                                    value={currentUserFullData.birthDate || ""}
                                    onChange={handleChange}
                                />
                            </Col>
                        </Row>
                        <Row className="mb-3">
                            <Col sm={3}>
                                <strong>Post:</strong>
                            </Col>
                            <Col>
                                <Form.Control
                                    type="text"
                                    name="post"
                                    value={currentUserFullData.post || ""}
                                    onChange={handleChange}
                                />
                            </Col>
                        </Row>
                        <Row className="mb-3">
                            <Col sm={3}>
                                <strong>Permissions:</strong>
                            </Col>
                            <Col>
                                {["editusers", "editpermissions", "editforms", "getformsresponses", "getusersfulldata"].map(
                                    (permission) => (
                                        <Form.Check
                                            key={permission}
                                            type="checkbox"
                                            label={permissionDisplay(permission)}
                                            value={permission}
                                            checked={currentUserFullData.permissions.includes(permission)}
                                            onChange={handlePermissionChange}
                                        />
                                    )
                                )}
                            </Col>
                        </Row>
                    </>
                ) : (
                    <>
                        <Row className="mb-3">
                            <Col sm={3}>
                                <strong>Access Code:</strong>
                            </Col>
                            <Col>
                                <span>{currentUserFullData.accessCode}</span>
                            </Col>
                        </Row>
                        <Row className="mb-3">
                            <Col sm={3}>
                                <strong>Last Name:</strong>
                            </Col>
                            <Col>
                                <span>{currentUserFullData.lastName}</span>
                            </Col>
                        </Row>
                        <Row className="mb-3">
                            <Col sm={3}>
                                <strong>First Name:</strong>
                            </Col>
                            <Col>
                                <span>{currentUserFullData.firstName}</span>
                            </Col>
                        </Row>
                        <Row className="mb-3">
                            <Col sm={3}>
                                <strong>Middle Name:</strong>
                            </Col>
                            <Col>
                                <span>{currentUserFullData.middleName}</span>
                            </Col>
                        </Row>
                        <Row className="mb-3">
                            <Col sm={3}>
                                <strong>Birth Date:</strong>
                            </Col>
                            <Col>
                                <span>{currentUserFullData.birthDate}</span>
                            </Col>
                        </Row>
                        <Row className="mb-3">
                            <Col sm={3}>
                                <strong>Post:</strong>
                            </Col>
                            <Col>
                                <span>{currentUserFullData.post}</span>
                            </Col>
                        </Row>
                        <Row className="mb-3">
                            <Col sm={3}>
                                <strong>Permissions:</strong>
                            </Col>
                            <Col>
                <span>
                  {currentUserFullData.permissions
                      .map((permission) => permissionDisplay(permission))
                      .join(", ")}
                </span>
                            </Col>
                        </Row>
                    </>
                )}

                {canEditUsers && (
                    <>
                        <Button variant="primary" onClick={handleSubmit} className="ms-2">
                            Submit
                        </Button>
                        <Button variant="danger" onClick={handleDeleteUser} className="ms-2">
                            Delete User
                        </Button>
                    </>
                )}

                <Button
                    variant={canEditUsers ? "secondary" : "primary"}
                    onClick={() => setCanEditUsers(!canEditUsers)}
                    className="ms-2"
                >
                    {canEditUsers ? "Cancel Edit" : "Enable Edit"}
                </Button>
            </Form>
        </Container>
    );
};

export default UserFullInfoBlock;
