import React, {useEffect, useState} from 'react';
import useAuthStore from "../../stores/useAuthStore.js";
import {PERMISSION} from "../../shared/enums/permissions.js";
import {useLocation} from "react-router-dom";
import UserPresentationInfoBlock from "../../components/UserPresentationInfoBlock/UserPresentationInfoBlock.jsx";
import UserFullInfoBlock from "../../components/UserFullInfoBlock/UserFullInfoBlock.jsx";

const GetUserPage = () => {

    const authStore = useAuthStore();
    const location = useLocation();
    const queryParams = new URLSearchParams(location.search);

    const [canGetFullUserData, setCanGetFullUserData] = React.useState(false);

    useEffect(() => {
        setCanGetFullUserData(authStore.hasPermission(PERMISSION.GET_USERS_FULL_DATA) ||
            authStore.hasPermission(PERMISSION.EDIT_USERS));
    }, []);

    return (
        <>
            <h1 className='mb-4'>User info</h1>
            {
                canGetFullUserData
                    ? <UserFullInfoBlock userId={queryParams.get("id")}/>
                    : <UserPresentationInfoBlock userId={queryParams.get("id")}/>
            }
        </>
    )
};

export default GetUserPage;