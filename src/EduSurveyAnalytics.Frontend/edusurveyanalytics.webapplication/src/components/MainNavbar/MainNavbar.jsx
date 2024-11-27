import {Container, Nav, Navbar} from "react-bootstrap";
import {Link} from "react-router-dom";
import useAuthStore from "../../stores/useAuthStore.js";
import {PERMISSION} from "../../shared/enums/permissions.js";

const MainNavbar = () => {

    const authStore = useAuthStore();

    return (
        <Navbar collapseOnSelect expand="lg" className="bg-body-tertiary">
            <Container>
                <Link className='navbar-brand' to='/'>EduSurveyAnalytics</Link>
                <Navbar.Toggle aria-controls="responsive-navbar-nav"/>
                <Navbar.Collapse id="responsive-navbar-nav">
                    <Nav className="me-auto">
                        {
                            !authStore.isAuthenticated()
                                ? <Link className='nav-link navbar-nav' to='/auth'>Sign in</Link>
                                :
                                <>
                                    <Link className='nav-link navbar-nav' to='/sign-out'>Sign out</Link>
                                    <Link className='nav-link navbar-nav' to='/form'>Forms</Link>
                                </>
                        }
                        {
                            authStore.hasPermission(PERMISSION.EDIT_FORMS)
                            && <Link className='nav-link navbar-nav' to='/form/create'>Create form</Link>
                        }
                        {
                            authStore.hasPermission(PERMISSION.GET_FORMS_RESPONSES)
                            && <Link className='nav-link navbar-nav' to='/form/answers'>Answers</Link>
                        }
                        {
                            authStore.hasPermission(PERMISSION.EDIT_USERS)
                            && <Link className='nav-link navbar-nav' to='/user/create'>Create user</Link>
                        }
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
};

export default MainNavbar;