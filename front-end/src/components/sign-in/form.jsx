import React, {useState} from "react";
import axios from "axios";
import {validateSigninData} from "./validate-signin-data";


const Form = () => {
    const [email, setEmail] = useState('')
    const [pass, setPass] = useState('');
    console.log(window.sessionStorage.getItem('token'))

    function sendData() {
        if (validateSigninData()) {
            console.log(email);
            console.log(pass);
            axios.post(process.env.REACT_APP_API_URL + "/api/Auth/authenticate",
                {
                    'userName': email,
                    'password': pass
                },
                {
                    headers:
                        {
                            'Access-Control-Allow-Origin': '*',
                            'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS'
                        },

                })
                .then((response) => {
                    console.log(response.data);
                    window.sessionStorage.setItem('token', response.data.token);
                    window.sessionStorage.setItem('role', response.data.role);
                    window.sessionStorage.setItem('username', email);
                    window.sessionStorage.setItem('refresh-token', response.data.refreshToken);
                    if (response.data.role === "admin_role")
                        window.location.href = "/admin"
                    else
                        window.location.href = "/personal-area";
                })
                .catch((error) => {
                    console.log(error)
                    alert("что пошло не так... все правильно ввели?")
                });
        }
    }

    return (
        <form className="sign-in__form">
            <h2>Login</h2>
            <p>У вас нет аккаунта? <a href="/signup" className="sign-in__link">Зарегистрируйтесь</a></p>
            <div className="auth__field login" id="login-field">
                <p>Логин</p>
                <input className="auth_input" type="text" placeholder="username" name="user-email"
                       id="user-email" onChange={(e) => {
                    setEmail(e.target.value);
                    e.target.parentElement.classList.remove("error");
                }}/>
                <label className="sign-in__label visually-hidden" htmlFor="user-email">Username</label>
                <p className="input-error__text visually-hidden" id="email-error">Неверное имя пользователя</p>
            </div>
            <div className="auth__field password" id="password-field">
                <p>Пароль</p>
                <input className="auth_input" type="password" placeholder="password" name="user-password"
                       id="user-password" onChange={(e) => {
                    setPass(e.target.value);
                    e.target.parentElement.classList.remove("error");
                }}/>
                <label className="sign-in__label visually-hidden" htmlFor="user-password">Password</label>
                <p className="input-error__text" id="password-error">Неверный пароль</p>
            </div>

            <p>Забыли логин или пароль? <a href="/404" className="sign-in__link">Восстановить</a></p>
            <button className="auth__btn" type="button" onClick={
                sendData}>Войти</button>
        </form>
    );
}

export default Form;
