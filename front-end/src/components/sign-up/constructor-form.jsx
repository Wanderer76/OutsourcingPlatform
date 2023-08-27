import { showPassword } from "./constructor-validation-funcs";
import { sendData } from "./send-data";
import {initializeMasks} from "./masks-checker";

const ConstructorForm = () => {
    document.onchange = () => {
        initializeMasks();
    }

    document.onreadystatechange = () => {
        initializeMasks();
    }
    return (
        <form className="sign-up__form">
            <h2>Регистрация <strong className="blue">исполнителя</strong></h2>
            <div className="form-blocks contractor">
                <div className="form-block form-block-1">
                    <div className="auth__field">
                        <p>Фамилия</p>
                        <input className="auth_input" type="text" placeholder="Фамилия" name="surname" id="surname" />
                        <label className="sign-in__label visually-hidden" htmlFor="surname">Фамилия</label>
                        <p className="input-error__text visually-hidden" id="surname-error">Заполните поле</p>
                    </div>

                    <div className="auth__field">
                        <p>Имя</p>
                        <input className="auth_input" type="text" placeholder="Имя" name="name" id="name" />
                        <label className="sign-in__label visually-hidden" htmlFor="name">Имя</label>
                        <p className="input-error__text visually-hidden" id="name-error">Заполните поле</p>
                    </div>

                    <div className="auth__field">
                        <p>Отчество</p>
                        <input className="auth_input" type="text" placeholder="Отчество" name="second-name" id="second-name" />
                        <label className="sign-in__label visually-hidden" htmlFor="second-name">Отчество</label>
                        <p className="input-error__text visually-hidden" id="second-name-error">Заполните поле или укажите что отчества нет</p>
                    </div>

                    <div className="auth__field field-check-box">
                        <input className="auth__field sign-in__check-box" type="checkbox" name="no-second-name" id="no-second-name" />
                        <label className="sign-in__label" htmlFor="no-second-name">Нет отчества</label>
                    </div>

                    <div className="auth__field">
                        <p>Дата рождения</p>
                        <input className="auth_input" type="date" name="birth-date" id="birth-date" />
                        <label className="sign-in__label visually-hidden" htmlFor="birth-date">Дата рождения</label>
                        <p className="input-error__text visually-hidden" id="birth-date-error">Заполните поле</p>
                    </div>

                    <div className="auth__field">
                        <p>Город</p>
                        <input className="auth_input" type="text" placeholder="Город" name="city" id="city"/>
                        <label className="sign-in__label visually-hidden" htmlFor="city">Город</label>
                        <p className="input-error__text visually-hidden" id="city-error">Заполните поле</p>
                    </div>

                    <div className="auth__field">
                        <p>Телефон</p>
                        <input className="auth_input" type="tel" placeholder="+7-XXX-XXX-XX-XX" name="phone" id="phone" />
                        <label className="sign-in__label visually-hidden" htmlFor="phone">Телефон</label>
                        <p className="input-error__text visually-hidden" id="phone-error">Error text (Текст ошибки)!</p>
                    </div>
                </div>

                <div className="form-block form-block-2">
                    <div className="auth__field">
                        <p>Username</p>
                        <input className="auth_input" type="text" placeholder="@username" name="username" id="username" />
                        <label className="sign-in__label visually-hidden" htmlFor="username">Username</label>
                        <p className="input-error__text visually-hidden" id="username-error">Error text (Текст ошибки)!</p>
                    </div>

                    <div className="auth__field">
                        <p>Email</p>
                        <input className="auth_input" type="email" placeholder="your@email.com" name="email" id="email"/>
                        <label className="sign-in__label visually-hidden" htmlFor="email">Email</label>
                        <p className="input-error__text visually-hidden" id="email-error">Error text (Текст ошибки)!</p>
                    </div>

                    <div className="password-block">
                        <div className="auth__field password" id="password-div">
                            <p className="error">Пароль</p>
                            <div className="password-with-control">
                                <input className="auth_input error" type="password" placeholder="password"
                                       name="user-password" id="user-password"/>
                                    <a href="#" className="password-control"
                                       onClick={() =>
                                       {
                                           let inp = document.getElementById('user-password');
                                           inp.type = inp.type === "text" ? "password" : "text"
                                       }}></a>
                            </div>
                            {/*<input className="auth_input error" type="password" placeholder="password" name="user-password" id="user-password" />*/}
                            <label className="sign-in__label visually-hidden" htmlFor="user-password">Пароль</label>
                            <p className="input-error__text visually-hidden" id="password-error">Длина пароля должна быть от 6 символов</p>
                        </div>

                        <div className="auth__field password-repeat" id="password-repeat-div">
                            <p className="error">Повторите пароль</p>
                            <div className="password-with-control">
                                <input className="auth_input error" type="password" placeholder="password"
                                       name="user-password-repeat" id="user-password-repeat"/>
                                    <a href="#" className="password-control"
                                       onClick={() =>
                                       {
                                           let inp = document.getElementById('user-password-repeat');
                                           inp.type = inp.type === "text" ? "password" : "text"
                                       }}></a>
                            </div>
                            <label className="sign-in__label visually-hidden" htmlFor="user-password-repeat">Повторите пароль</label>
                            <p className="input-error__text visually-hidden" id="password-repeat-error">Пароли не совпадают</p>
                        </div>

                    </div>

                    <button className="auth__btn" type="button" onClick={() => {sendData("executor")}}>Зарегистрироваться</button>
                </div>
            </div>
        </form>
    )
}

export default ConstructorForm;