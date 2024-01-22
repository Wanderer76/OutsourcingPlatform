import {useSearchParams} from "react-router-dom";
import {useEffect} from "react";
import axios from "axios";

export function Verification() {
    debugger
    const [searchParams, setSearchParams] = useSearchParams();
    const activationCode = searchParams.get("code")
    axios.get(process.env.REACT_APP_API_URL + "/api/verify" + activationCode, null,
        {
            headers:
                {
                    'Access-Control-Allow-Origin': '*',
                    'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS'
                },

        })
        .then((response) => {
                console.log(response);
                console.log(response.data.token);
                window.sessionStorage.setItem('token', response.data.token);
                window.sessionStorage.setItem('role', response.data.role);
                window.sessionStorage.setItem('username', response.data.username);
                window.sessionStorage.setItem('refresh-token', response.data.refreshToken);
                window.location.href = "/personal-area";
            }
        ).catch((error) => {
        console.log(error)
        window.location.href = "/signin";
    });

    return (<></>)
}