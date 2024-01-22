import { checkAllData } from "./constructor-validation-funcs";
import axios from "axios";

export function sendData(status="executor") {
    let data = checkAllData()
    if (data === null)
        console.log("Data wasn't send");
    else {
       // data['permission'] = ["localhost:5228"]
        console.log(data);
        axios.post(process.env.REACT_APP_API_URL+"/api/Auth/"+status+"/register",data,
            {
                headers:
                    {'Access-Control-Allow-Origin' : '*',
                     'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS'},

            })
            .then((response => {
                console.log(response);
                axios.post(process.env.REACT_APP_API_URL+"/api/Auth/authenticate",
                    {
                        'userName': data['username'],
                        'password': data['password']
                    },
                    {
                        headers:
                            {
                                'Access-Control-Allow-Origin': '*',
                                'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS'
                            },

                    })
                    .then((response) => {
                        console.log(response.data.token);
                        window.sessionStorage.setItem('token', response.data.token);
                        window.sessionStorage.setItem('role', response.data.role);
                        window.sessionStorage.setItem('username', data['username']);
                        window.sessionStorage.setItem('refresh-token', response.data.refreshToken);
                        window.location.href = "/personal-area";
                    })
                    .catch((error) => {
                        console.log(error)
                        window.location.href = "/signin";
                    });
                // window.location.href = "/signin";
            })).catch((error) => {
                console.log(error);
                alert("что-то полшло не так...:( возможно пользователи с такими данными уже зарегистрированы на нашем сайте")
            });
        console.log("Data was send. Congratulate you, thumbs!");

    }
}