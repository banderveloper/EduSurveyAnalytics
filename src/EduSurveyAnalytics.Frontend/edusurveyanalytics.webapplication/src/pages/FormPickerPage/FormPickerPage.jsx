import {useEffect} from 'react';
import useFormsStore from "../../stores/useFormsStore.js";
import FormAnswering from "../../components/FormAnswering/FormAnswering.jsx";

const FormPickerPage = () => {
    const formStore = useFormsStore();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);

        const formId = queryParams.get('id');

        if (formId) {
            formStore.getForm(formId);
        }
    }, []);

    if(formStore.isLoading) {
        return <p>Loading...</p>
    }

    return (
        <>
            <h1>Form answering</h1>
            {
                formStore.currentForm
                    ?
                        <div className="container pt-5">
                            <FormAnswering data={formStore.currentForm}/>
                        </div>
                    :
                        <p>Not found</p>
            }
        </>
    )
};

export default FormPickerPage;