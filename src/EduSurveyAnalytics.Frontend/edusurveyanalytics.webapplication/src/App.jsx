import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import Navbar from "./components/MainNavbar/MainNavbar.jsx";
import AuthForm from "./components/AuthForm/AuthForm.jsx";
import FormAnswering from "./components/FormAnswering/FormAnswering.jsx";

export default function App() {

    const formData = JSON.parse(`{
    "id": "ce856fec-3386-494e-a835-0fad2a44d6d3",
    "ownerId": "cc34636b-269f-4691-9d69-7c81d608a653",
    "ownerName": "Admin Admin Admin",
    "ownerPost": "Admin",
    "createdAt": "0001-01-01T00:00:00+00:00",
    "updatedAt": "0001-01-01T00:00:00+00:00",
    "fields": [
      {
        "id": "bc34a4be-625b-4966-bc65-2d2a3876ea13",
        "title": "Do you like bananas?",
        "order": 0,
        "constraints": [
          "required"
        ]
      },
      {
        "id": "a9316715-2c05-456b-b659-710733be0ae1",
        "title": "How old are you",
        "order": 1,
        "constraints": [
          "required", "numbers_only"
        ]
      }
    ]
  }`)

    return (
        <>
            <Navbar/>

            <div className="container pt-5">
                <FormAnswering data={formData}/>
            </div>
        </>
    )
}
