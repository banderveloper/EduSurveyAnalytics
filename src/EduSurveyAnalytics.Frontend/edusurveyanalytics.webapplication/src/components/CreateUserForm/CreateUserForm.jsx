import React, {useEffect, useState} from 'react';
import {Form, Button} from 'react-bootstrap';
import DatePicker from 'react-datepicker';
import "react-datepicker/dist/react-datepicker.css";
import useUsersStore from "../../stores/useUsersStore.js";
import {useNavigate} from "react-router-dom";

const CreateUser = () => {
    const [formData, setFormData] = useState({
        accessCode: '',
        lastName: '',
        firstName: '',
        middleName: '',
        birthDate: new Date(),
        post: '',
    });

    const usersStore = useUsersStore()
    const navigate = useNavigate();

    // Handle input change for text fields
    const handleInputChange = (e) => {
        const {name, value} = e.target;
        setFormData((prevState) => ({
            ...prevState,
            [name]: value,
        }));
    };

    // Handle date picker change
    const handleDateChange = (date) => {
        setFormData((prevState) => ({
            ...prevState,
            birthDate: date,
        }));
    };

    // Handle form submission
    const handleSubmit = async (e) => {
        e.preventDefault();
        // Prepare the JSON data

        let createdUserId =
            await usersStore.createUser(formData.accessCode, formData.lastName, formData.firstName, formData.middleName, formData.birthDate.toISOString().split('T')[0], formData.post);

        navigate('/user?id=' + createdUserId);
    };


    return (
        <div>
            <Form onSubmit={handleSubmit} className='mt-4'>
                <Form.Group controlId="formAccessCode" className="mb-3">
                    <Form.Label>Access Code</Form.Label>
                    <Form.Control
                        type="text"
                        name="accessCode"
                        value={formData.accessCode}
                        onChange={handleInputChange}
                        required
                    />
                </Form.Group>

                <Form.Group controlId="formLastName" className="mb-3">
                    <Form.Label>Last Name</Form.Label>
                    <Form.Control
                        type="text"
                        name="lastName"
                        value={formData.lastName}
                        onChange={handleInputChange}
                        required
                    />
                </Form.Group>

                <Form.Group controlId="formFirstName" className="mb-3">
                    <Form.Label>First Name</Form.Label>
                    <Form.Control
                        type="text"
                        name="firstName"
                        value={formData.firstName}
                        onChange={handleInputChange}
                        required
                    />
                </Form.Group>

                <Form.Group controlId="formMiddleName" className="mb-3">
                    <Form.Label>Middle Name</Form.Label>
                    <Form.Control
                        type="text"
                        name="middleName"
                        value={formData.middleName}
                        onChange={handleInputChange}
                    />
                </Form.Group>

                {/* Birth Date Picker in a new line with more margin */}
                <Form.Group controlId="formBirthDate" className="mb-3">
                    <Form.Label>Birth Date</Form.Label>
                    <div className="mt-2"> {/* Adding margin-top to separate DatePicker from label */}
                        <DatePicker
                            selected={formData.birthDate}
                            onChange={handleDateChange}
                            dateFormat="yyyy-MM-dd"
                            className="form-control"
                            required
                        />
                    </div>
                </Form.Group>

                <Form.Group controlId="formPost" className="mb-3">
                    <Form.Label>Post</Form.Label>
                    <Form.Control
                        type="text"
                        name="post"
                        value={formData.post}
                        onChange={handleInputChange}
                        required
                    />
                </Form.Group>

                <Button variant="primary" type="submit">
                    Create User
                </Button>
            </Form>
        </div>
    );
};

export default CreateUser;
