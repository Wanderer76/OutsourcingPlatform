import {initializeMasks} from "../../sign-up/masks-checker";
import React, {useEffect, useState} from "react";
import axios from "axios";
import {fillCustomerDataForEdit} from "./fill-customer-data-for-edit";

const EditCustomerData = () => {
    const [res, setRes] = useState({});
    const [links, setLinks] = useState({});

    useEffect(() => {
        document.onchange = () => {
            initializeMasks();
        }

        document.onreadystatechange = () => {
            initializeMasks();
        }

        axios.get(process.env.REACT_APP_API_URL + "/api/PersonalArea/customer/area",
            {
                headers:
                    {'Access-Control-Allow-Origin' : '*',
                        'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization' : "Bearer " + window.sessionStorage.getItem('token')
                    },

            })
            .then((response) => {
                console.log(response.data);
                setRes({
                    surname: response.data.surname,
                    name : response.data.name,
                    secondName: response.data.secondName,
                    phone: response.data.phone,
                    email : response.data.email,
                    inn : response.data.customer.inn,
                    companyName : response.data.customer.companyName,
                    address: response.data.customer.address,
                    password: ""
                });
                if (response.data.contacts !== null)
                    setLinks({
                        about: response.data.contacts.about,
                        vkNickname: response.data.contacts.vkNickname,
                        githubUrl: response.data.contacts.githubUrl,
                        messager: response.data.contacts.messager
                    })
                else
                    setLinks({
                        about: null,
                        vkNickname: null,
                        githubUrl: null,
                        messager: null
                    })
                fillCustomerDataForEdit(response.data);
                console.log(res);
                console.log(links);
            })
            .catch((error) => {
                console.log('здесь должна быть ошибка')
                console.log(error);
                alert("что-то пошло не так :(")
            });


    }, [])

    function addLink() {
        let type = document.getElementById("link-new-field").value;
        let value = document.getElementById("current-link").value;
        let copy = {...links};
        copy[type] = value
        setLinks(copy);
        console.log(links);
    }
    function removeLink(linkField) {
        let copy= {...links}
        copy[linkField] = null;
        setLinks(copy);
    }

    function change(fieldName, value, setState, state) {
        let copy_res = state;
        copy_res[fieldName] = value;
        setState(copy_res);
        console.log(res);
        console.log(links);
    }

    function sendData(url, data) {
        console.log("send data: ");
        console.log(data);
        axios.post(url, data,
            {
                headers:
                    {'Access-Control-Allow-Origin' : '*',
                        'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization' : "Bearer " + window.sessionStorage.getItem('token')
                    },

            })
            .then((response) => {
                console.log("info send");
            })
            .catch((error) => {console.log(error)});
    }

    function changePassword() {

        let pass = document.getElementById("user-password").value;
        let repeatPass = document.getElementById("user-password-repeat").value;
        if (pass.length < 6) {
            document.getElementById("password-error").classList.remove("visually-hidden");
            document.getElementById("password-error").textContent = "Слишком короткий пароль"
            change("password", "", setRes, res)
        }
        else if (pass === repeatPass) {
            change("password", pass, setRes, res)
            document.getElementById("password-error").classList.add("visually-hidden");
        }
        else {
            change("password", "", setRes, res)
            document.getElementById("password-error").classList.remove("visually-hidden");
            document.getElementById("password-error").textContent = "Пароли не совпадают!"
        }
    }


    return(
        <div className="main">
            <section className="new-project">
                <h1 className="page-title">Редактирование пользователя</h1>
                <p className="navigation-after-title"><a href="#">личный кабинет</a>/<a href="#">форма
                    редактирования</a></p>
                <form className="new-project" name="new-project">

                    <div className="user-personal-data block">
                        <div className="user-personal-data column-1">
                            <div className="user-data-edit__field">
                                <p>Фамилия</p>
                                <input className="user-data-edit_input" type="text" placeholder="Фамилия" name="surname"
                                       id="surname" onChange={event => change("surname", event.target.value, setRes, res)}/>
                                <label className="user-data__label visually-hidden" htmlFor="surname">Фамилия</label>
                                <p className="input-error__text visually-hidden" id="surname-error">Error text (Текст
                                    ошибки)!</p>
                            </div>

                            <div className="user-data-edit__field">
                                <p>Имя</p>
                                <input className="user-data-edit_input" type="text" placeholder="Имя" name="name"
                                       id="name" onChange={event => change("name", event.target.value, setRes, res)}/>
                                <label className="user-data__label visually-hidden" htmlFor="name" >Имя</label>
                                <p className="input-error__text visually-hidden" id="name-error">Error text (Текст
                                    ошибки)!</p>
                            </div>

                            <div className="user-data-edit__field" id={"surnameInput"}>
                                <p>Отчество</p>
                                <input className="user-data-edit_input" type="text" placeholder="Отчество"
                                       name="second-name" id="second-name" onChange={event => change("secondName", event.target.value, setRes, res)}/>
                                <label className="user-data__label visually-hidden"
                                       htmlFor="second-name" >Отчество</label>
                                <p className="input-error__text visually-hidden" id="second-name-error">Error text
                                    (Текст ошибки)!</p>
                            </div>

                            <div className="user-data-edit__field field-check-box">
                                <input className="user-data-edit__field user-data__check-box" type="checkbox"
                                       name="no-second-name" id="no-second-name"
                                       onChange={(event) => {
                                           change("secondName", "", setRes, res)
                                           document.getElementById("surnameInput").hidden = event.target.checked;
                                       }
                                }/>
                                <label className="user-data__label" htmlFor="no-second-name">Нет отчества</label>
                            </div>
                        </div>
                        <div className="user-personal-data column-2">
                            <div className="user-data-edit__field">
                                <p>Название организации</p>
                                <input className="user-data-edit_input" type="text" placeholder="Название организации"
                                       name="company-name" id="company-name" onChange={event => change("companyName", event.target.value, setRes, res)}/>
                                <label className="user-data__label visually-hidden" htmlFor="company-name">Название
                                    организации</label>
                                <p className="input-error__text visually-hidden" id="company-name-error">Error text
                                    (Текст ошибки)!</p>
                            </div>

                            <div className="user-data-edit__field">
                                <p>Адрес</p>
                                <input className="user-data-edit_input" type="text" placeholder="Адрес" name="address"
                                       id="address" onChange={event => change("address", event.target.value, setRes, res)}/>
                                <label className="user-data__label visually-hidden" htmlFor="address">Адрес</label>
                                <p className="input-error__text visually-hidden" id="address-error">Error text (Текст
                                    ошибки)!</p>
                            </div>

                            <div className="user-data-edit__field">
                                <p>ИНН</p>
                                <input className="user-data-edit_input" type="text" placeholder="XXX-XXX-XXXX"
                                       name="inn" id="inn" onChange={event => change("inn", event.target.value.replaceAll("-", ""), setRes, res)}/>
                                <label className="user-data__label visually-hidden" htmlFor="inn">Индивидуальный
                                    налоговый номер</label>
                                <p className="input-error__text visually-hidden" id="inn-error">Error text (Текст
                                    ошибки)!</p>
                            </div>

                            <div className="user-data-edit__field field-check-box">
                                <input className="user-data-edit__field user-data__check-box" type="checkbox"
                                       name="own-business-check" id="own-business-check"/>
                                <label className="user-data__label" htmlFor="own-business-check">У меня ИП</label>
                            </div>

                        </div>
                        <div className="user-personal-data column-3">
                            <div className="user-data-edit__field">
                                <p>Телефон</p>
                                <input className="user-data-edit_input" type="tel" placeholder="+7-XXX-XXX-XX-XX"
                                       name="phone" id="phone" onChange={event => change("phone", event.target.value, setRes, res)}/>
                                <label className="user-data__label visually-hidden" htmlFor="phone">Телефон</label>
                                <p className="input-error__text visually-hidden" id="phone-error">Error text (Текст
                                    ошибки)!</p>
                            </div>

                            <div className="user-data-edit__field">
                                <p>Email</p>
                                <input className="user-data-edit_input" type="email" placeholder="your@email.com"
                                       name="email" id="email" onChange={event => change("email", event.target.value, setRes, res)}/>
                                <label className="user-data__label visually-hidden" htmlFor="email">Email</label>
                                <p className="input-error__text visually-hidden" id="email-error">Error text (Текст ошибки)!</p>
                            </div>

                            <div className="password-block">
                                <div className="user-data-edit__field password">
                                    <p className="error">Пароль</p>
                                    <div className="password-with-control">
                                        <input type="password" id="user-password"
                                               name="password" className="user-data-edit_input error"
                                               onChange={() => changePassword()}
                                        />
                                        <a href="#" className="password-control"
                                           onClick={() =>
                                           {
                                               let inp = document.getElementById('user-password');
                                               inp.type = inp.type === "text" ? "password" : "text"
                                           }}>
                                        </a>
                                    </div>

                                    {/*<input className="user-data-edit_input error" type="password"*/}
                                    {/*       name="user-password" id="user-password"/>*/}
                                    <label className="user-data__label visually-hidden"
                                           htmlFor="user-password">Пароль</label>
                                </div>

                                <div className="user-data-edit__field password-repeat">
                                    <p className="error">Повторите пароль</p>
                                    <div className="password-with-control">
                                        <input type="password" name="user-password-repeat" id="user-password-repeat" className="user-data-edit_input error" onChange={() => changePassword()}/>
                                        <a href="#" className="password-control"
                                           onClick={() =>
                                           {
                                               let inp = document.getElementById('user-password-repeat');
                                               inp.type = inp.type === "text" ? "password" : "text"
                                           }}></a>
                                    </div>
                                    {/*<input className="user-data-edit_input error" type="password"*/}
                                    {/*       name="user-password-repeat" id="user-password-repeat"/>*/}
                                    <label className="user-data__label visually-hidden" htmlFor="user-password-repeat">Повторите
                                        пароль</label>
                                    <p className="input-error__text visually-hidden"  id="password-error">Пароли не совпадают!</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="inputs-row">
                        <div className="new-project__field project-new-link">
                            <div className="add-new-link">
                                <div className="new-project__field project-link-type">
                                    <p>Добавить ссылку</p>
                                    <select className="new-project-select" name="link-new-field" id="link-new-field">
                                        <option value="">-- Ресурс --</option>
                                        <option value="vkNickname">Вконтакте</option>
                                        <option value="messager">Telegram</option>
                                        <option value="githubUrl">GitHub</option>
                                    </select>
                                    <label className="visually-hidden" htmlFor="link-new-field">Ссылка
                                        пользователя</label>
                                    <p className="new-project__text visually-hidden" id="link-type-error">Error text
                                        (Текст ошибки)!</p>
                                </div>

                                <div className="new-project__field project-current-link">
                                    <p>Адрес ссылки</p>
                                    <input className="new-project__field" name="current-link" id="current-link"
                                           autoComplete="off"/>
                                    <label className="visually-hidden" htmlFor="current-link">Введите ссылку</label>
                                    <p className="new-project__text visually-hidden" id="current-link-error">Error
                                        text (Текст ошибки)!</p>
                                </div>
                            </div>

                        </div>

                        <input type="button" className="new-project-btn" value="Добавить" id="add-new-link" onClick={addLink}/>
                    </div>

                    <div className="new-project-links">
                        <h2>Мои ссылки</h2>
                        <div className="new-project-links-tags">
                            {links.vkNickname !== null &&
                                <div className="new-project-field-tag">
                                    <span>{links.vkNickname}</span>
                                    <button type="button" className="delete-tag-btn" onClick={()=> {
                                        removeLink('vkNickname')
                                    }}>x</button>
                                </div>}
                            {links.messager !== null &&
                                <div className="new-project-field-tag">
                                    <span>{links.messager}</span>
                                    <button type="button" className="delete-tag-btn" onClick={()=> {
                                        removeLink('messager')
                                    }}>x</button>
                                </div>}
                            {links.githubUrl !== null &&
                                <div className="new-project-field-tag">
                                    <span>{links.githubUrl}</span>
                                    <button type="button" className="delete-tag-btn" onClick={()=> {
                                        removeLink('githubUrl')
                                    }}>x</button>
                                </div>}
                        </div>
                        <p className="new-project__text visually-hidden" id="project-new-links-error">Error text (Текст
                            ошибки)!</p>
                    </div>

                    <div className="new-project__field person-description">
                        <p>О себе</p>
                        <textarea className="new-project__field person-description" placeholder="Напишите о себе"
                                  name="person-description" id="person-description" rows="8"
                                  autoComplete="off" onInput={(event) => change("about", event.target.value, setLinks, links)}></textarea>
                        <label className="visually-hidden" htmlFor="person-description">О себе</label>
                        <p className="new-project__text visually-hidden" id="person-description-error">Error text (Текст
                            ошибки)!</p>
                    </div>



                    <div className="new-project__btns">
                        <button type="button" className="new-project-btn new-project-submit-btn" value="Сохранить"
                                id="new-project-submit" onClick={() => {
                                    console.log(res);
                                    console.log(links);
                                    sendData(process.env.REACT_APP_API_URL + "/api/PersonalArea/customer/update", res);
                                    sendData(process.env.REACT_APP_API_URL + "/api/PersonalArea/contacts/update", links);
                                    window.location.href = "/personal-area";
                        }}>Сохранить</button>
                        <button type="button" className="new-project-btn new-project-submit-cancel" value="Отменить"
                                id="new-project-cancel" onClick={() => {window.location.href = "/personal-area"}}>Отменить</button>
                    </div>
                </form>
            </section>
        </div>
    )
}

export default EditCustomerData;