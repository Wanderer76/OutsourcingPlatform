import React, {useEffect, useState} from "react";
import {initializeMasks} from "../../sign-up/masks-checker";
import axios from "axios";
import {fillExecutorDataForEdit} from "./fill-executor-data-for-edit";
import {addElement, checkArray, deleteElement} from "../../tasks/add-check-delete-tags";
import {formatTagValue} from "../../tasks/format-tag";

const EditExecutorData = (props) => {
    const [res, setRes] = useState();
    const [links, setLinks] = useState();
    const [edus, setEdu] = useState();
    const [skills, setSkills] = useState();
    const [categories, setCategories] = useState();
    const [skillsView, setSkillsView] = useState()
    const [compsView, setCompsView] = useState();
    const [linksView, setLinksView] = useState();
    useEffect(() => {
        document.onchange = () => {
            initializeMasks();
        }

        document.onreadystatechange = () => {
            initializeMasks();
        }

        axios.get(process.env.REACT_APP_API_URL + "/api/PersonalArea/executor/area",
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    },

            })
            .then((response) => {
                console.log(response.data);
                fillExecutorDataForEdit(response.data);
                setRes({
                    surname: response.data.surname,
                    name: response.data.name,
                    secondName: response.data.secondName,
                    phone: response.data.phone,
                    email: response.data.email,
                    birthDate: response.data.executor.birthDate,
                    city: response.data.executor.city,
                    password: ""
                });
                if (response.data.contacts !== null) {
                    var links = response.data.contacts.contacts.map(x => Object.create({'name': x.name, 'url': x.url}))
                    setLinks({
                        about: response.data.contacts.about,
                        contacts: links.map(x => Object.getPrototypeOf(x))
                        //githubUrl: response.data.contacts.githubUrl,
                        //messager: response.data.contacts.messager
                    })
                } else
                    setLinks({
                        about: null,
                        contacts: []
                    })
                setEdu(response.data.executor.educations);
                setSkills(response.data.executor.skills);
                setCategories(response.data.executor.categories)
            })
            .catch((error) => {
                console.log('здесь должна быть ошибка')
                console.log(error);
                alert(error)
            });

        axios.get(process.env.REACT_APP_API_URL + "/api/Competencies/skills/0/20",
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    },

            })
            .then((response) => {
                if (response.status === 200)
                    setSkillsView(response.data.map((comp) => {
                        return <option value={comp['name']} name={comp['id']}>{comp["name"]}</option>
                    }));
            })
            .catch((error) => {
                console.log(error)
            });

        axios.get(process.env.REACT_APP_API_URL + "/api/Competencies/categories/0/20",
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    },

            })
            .then((response) => {
                if (response.status === 200)
                    setCompsView(response.data.map((comp) => {
                        return <option value={comp['name']} name={comp['id']}>{comp["name"]}</option>
                    }));
            })
            .catch((error) => {
                console.log(error)
            });


        axios.get(process.env.REACT_APP_API_URL + "/api/PersonalArea/contacts/list",
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    },

            })
            .then((response) => {
                if (response.status === 200)
                    setLinksView(response.data.map((comp) => {
                        return <option value={comp}>{comp}</option>
                    }));
            })
            .catch((error) => {
                console.log(error)
            });

    }, [])

    function validAndAddEdu() {
        let place = document.getElementById('education-place');
        let speciality = document.getElementById('education-speciality');
        let year = document.getElementById('education-year');
        let good = true
        if (place.value === null || place.value === "") {
            good = false
            document.getElementById("education-place-block").classList.add('error')
            document.getElementById('education-place-error').classList.remove('visually-hidden')
        }
        if (speciality.value === null || speciality.value === "") {
            good = false
            document.getElementById("education-speciality-block").classList.add('error')
            document.getElementById('education-speciality-error').classList.remove('visually-hidden')
        }
        if (year.value === null || year.value === "" || year.value < 1960) {
            good = false
            document.getElementById("education-year-block").classList.add('error')
            document.getElementById('education-year-error').classList.remove('visually-hidden')
        }

        if (good) {
            let value = {
                id: null,
                graduationYear: year.value,
                place: place.value,
                speciality: speciality.value
            }
            if (edus.indexOf(value) === -1) {
                console.log('added');
                setEdu([...edus, value]);
            } else console.log('not')
        }

    }

    function addLink() {
        let type = document.getElementById("link-new-field").value;
        let value = document.getElementById("current-link").value;
        if (type === '' || value === '')
            return

        let copy = {...links};

        const tmp = copy.contacts.filter(x => x.name === type);
        if (tmp.length === 1) {
            copy.contacts = [...copy.contacts.filter(x => x.name !== type), {name: type, url: value}]
        } else
            copy.contacts = [...copy.contacts, {name: type, url: value}]
        setLinks(copy);
        console.log(links);
    }

    function removeLink(linkField) {
        let copy = {...links}
        copy.contacts = copy.contacts.filter(x => x.name !== linkField);
        setLinks(copy);
    }

    function change(fieldName, value, setState, state) {
        let copy_res = state;
        copy_res[fieldName] = value;
        setState(copy_res);
        console.log(res);
        console.log(links);
    }

    async function sendData(url, data) {
        console.log("send data: ");
        console.log(data);
        await axios.post(url, data,
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    },

            })
            .then((response) => {
                console.log("info send");
            })
            .catch((error) => {
                console.log(error)
            });
    }

    function changePassword() {

        let pass = document.getElementById("user-password").value;
        let repeatPass = document.getElementById("user-password-repeat").value;
        if (pass.length < 6) {
            document.getElementById("password-error").classList.remove("visually-hidden");
            document.getElementById("password-error").textContent = "Слишком короткий пароль";
            change("password", "", setRes, res)
        } else if (pass === repeatPass) {
            change("password", pass, setRes, res)
            document.getElementById("password-error").classList.add("visually-hidden");
        } else {
            change("password", "", setRes, res)
            document.getElementById("password-error").classList.remove("visually-hidden");
            document.getElementById("password-error").textContent = "Пароли не совпадают!"
        }
    }

    return (
        <div className="main">
            <section className="new-project">
                <h1 className="page-title">Редактирование пользователя</h1>
                <p className="navigation-after-title"><a href="/personal-area">личный кабинет</a>/<a href="#">форма
                    редактирования</a></p>
                <form className="new-project" name="new-project">

                    <div className="user-personal-data block">
                        <div className="user-personal-data column-1">
                            <div className="user-data-edit__field">
                                <p>Фамилия</p>
                                <input className="user-data-edit_input" type="text" placeholder="Фамилия" name="surname"
                                       id="surname"
                                       onChange={event => change("surname", event.target.value, setRes, res)}/>
                                <label className="user-data__label visually-hidden" htmlFor="surname">Фамилия</label>
                                <p className="input-error__text visually-hidden" id="surname-error">Error text (Текст
                                    ошибки)!</p>
                            </div>

                            <div className="user-data-edit__field">
                                <p>Имя</p>
                                <input className="user-data-edit_input" type="text" placeholder="Имя" name="name"
                                       id="name" onChange={event => change("name", event.target.value, setRes, res)}/>
                                <label className="user-data__label visually-hidden" htmlFor="name">Имя</label>
                                <p className="input-error__text visually-hidden" id="name-error">Error text (Текст
                                    ошибки)!</p>
                            </div>

                            <div className="user-data-edit__field">
                                <p>Отчество</p>
                                <input className="user-data-edit_input" type="text" placeholder="Отчество"
                                       name="second-name" id="second-name"
                                       onChange={event => change("secondName", event.target.value, setRes, res)}/>
                                <label className="user-data__label visually-hidden"
                                       htmlFor="second-name">Отчество</label>
                                <p className="input-error__text visually-hidden" id="second-name-error">Error text
                                    (Текст ошибки)!</p>
                            </div>

                            <div className="user-data-edit__field field-check-box">
                                <input className="user-data-edit__field user-data__check-box" type="checkbox"
                                       name="no-second-name" id="no-second-name" onChange={() => {
                                    document.getElementById('second-name').classList.toggle('visually-hidden')
                                }}/>
                                <label className="user-data__label" htmlFor="no-second-name">Нет отчества</label>
                            </div>
                        </div>
                        <div className="user-personal-data column-2">
                            <div className="user-data-edit__field">
                                <p>Дата рождения</p>
                                <input className="user-data-edit_input" type="date" name="birth-date" id="birth-date"
                                       onChange={event => change("birthDate", event.target.value, setRes, res)}/>
                                <label className="user-data__label visually-hidden" htmlFor="birth-date">Дата
                                    рождения</label>
                                <p className="input-error__text visually-hidden" id="birth-date-error">Error text (Текст
                                    ошибки)!</p>
                            </div>

                            <div className="user-data-edit__field">
                                <p>Город</p>
                                <input className="user-data-edit_input" type="text" placeholder="Город" name="city"
                                       id="city" onChange={event => change("city", event.target.value, setRes, res)}/>
                                <label className="user-data__label visually-hidden" htmlFor="city">Город</label>
                                <p className="input-error__text visually-hidden" id="city-error">Error text (Текст
                                    ошибки)!</p>
                            </div>

                            <div className="user-data-edit__field">
                                <p>Телефон</p>
                                <input className="user-data-edit_input" type="tel" placeholder="+7-XXX-XXX-XX-XX"
                                       name="phone" id="phone"
                                       onChange={event => change("phone", event.target.value, setRes, res)}/>
                                <label className="user-data__label visually-hidden" htmlFor="phone">Телефон</label>
                                <p className="input-error__text visually-hidden" id="phone-error">Error text (Текст
                                    ошибки)!</p>
                            </div>
                        </div>
                        <div className="user-personal-data column-3">
                            <div className="user-data-edit__field">
                                <p>Email</p>
                                <input className="user-data-edit_input" type="email" placeholder="your@email.com"
                                       name="email" id="email"
                                       onChange={event => change("email", event.target.value, setRes, res)}/>
                                <label className="user-data__label visually-hidden" htmlFor="email">Email</label>
                                <p className="input-error__text visually-hidden" id="email-error">Error text (Текст
                                    ошибки)!</p>
                            </div>

                            <div className="password-block">
                                <div className="user-data-edit__field password">
                                    <p className="error">Пароль</p>
                                    <div className="password-with-control">
                                        <input type="password" id="user-password"
                                               name="password" className="user-data-edit_input error"
                                               onChange={() => changePassword()}/>
                                        <a href="#" className="password-control"
                                           onClick={() => {
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
                                        <input type="password" name="user-password-repeat" id="user-password-repeat"
                                               className="user-data-edit_input error"
                                               onChange={() => changePassword()}/>
                                        <a href="#" className="password-control"
                                           onClick={() => {
                                               let inp = document.getElementById('user-password-repeat');
                                               inp.type = inp.type === "text" ? "password" : "text"
                                           }}></a>
                                    </div>
                                    {/*<input className="user-data-edit_input error" type="password"*/}
                                    {/*       name="user-password-repeat" id="user-password-repeat"/>*/}
                                    <label className="user-data__label visually-hidden" htmlFor="user-password-repeat">Повторите
                                        пароль</label>
                                    <p className="input-error__text visually-hidden" id="password-error">Error text
                                        (Текст ошибки)!</p>
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
                                        {/*<option value="vkNickname">Вконтакте</option>*/}
                                        {/*<option value="messager">Telegram</option>*/}
                                        {/*<option value="githubUrl">GitHub</option>*/}
                                        {linksView}

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

                        <input type="button" className="new-project-btn" value="Добавить" id="add-new-link"
                               onClick={e => {
                                   addLink()
                               }}/>
                    </div>

                    <div className="new-project-links">
                        <h2>Мои ссылки</h2>
                        {links === undefined && null}
                        {links !== undefined &&
                            <div className="new-project-links-tags">
                                {
                                    links.contacts.map(contact => {
                                        return (<div className="new-project-field-tag" key={contact.url}>
                                                <span>{contact.name}:</span> <span>{contact.url}</span>
                                                <button className="delete-tag-btn" type="button" onClick={() => {
                                                    removeLink(contact.name);
                                                }}>x
                                                </button>
                                            </div>
                                        )
                                    })

                                }
                            </div>
                        }
                        <p className="new-project__text visually-hidden" id="project-new-links-error">Error text (Текст
                            ошибки)!</p>
                    </div>

                    <div className="new-project__field person-description">
                        <p>О себе</p>
                        <textarea className="new-project__field person-description" placeholder="Напишите о себе"
                                  name="person-description" id="person-description" rows="8"
                                  autoComplete="off" onChange={(e) => {
                            links.about = e.target.value
                        }}></textarea>
                        <label className="visually-hidden" htmlFor="person-description">О себе</label>
                        <p className="new-project__text visually-hidden" id="person-description-error">Error text (Текст
                            ошибки)!</p>
                    </div>
                    <div className="inputs-row">
                        <div className="new-project__field project-new-field">
                            <p>Сфера деятельности</p>
                            <select className="new-project-select" name="project-new-field" id="project-new-field"
                                    onChange={() => {
                                        if (categories.length < 3)
                                            addElement("project-new-field", categories, setCategories)
                                    }}>
                                {compsView}
                            </select>
                            <label className="visually-hidden" htmlFor="project-new-field">Сфера деятельности</label>
                        </div>
                    </div>

                    <div className="new-project-fields">
                        <h2>Выбранные сферы деятельности</h2>
                        <div className="new-project-field-tags">
                            {categories !== undefined ? categories.map((cat) => {
                                return (
                                    <div className="new-project-field-tag">
                                        <span>{formatTagValue(cat.name)}</span>
                                        <button type="button" className="delete-tag-btn"
                                                onClick={() => deleteElement(cat, categories, setCategories)}>x
                                        </button>
                                    </div>)
                            }) : <></>}


                        </div>
                        <p className="new-project__text visually-hidden" id="project-new-field-error">Error text (Текст
                            ошибки)!</p>
                    </div>

                    <div className="inputs-row">
                        <div className="new-project__skill project-new-skill">
                            <p>Компетенции</p>
                            <select className="new-project-select" name="project-new-skill" id="project-new-skill"
                                    onChange={() => {
                                        if (skills.length < 5) addElement("project-new-skill", skills, setSkills)
                                    }}>
                                <option value="">-- Выберите роль --</option>
                                {skillsView}
                            </select>
                            <label className="visually-hidden" htmlFor="project-new-skill">Роли</label>
                        </div>
                    </div>

                    <div className="new-project-skills">
                        <h2>Выбранные компетенции</h2>
                        <div className="new-project-skills-tags">
                            {skills !== undefined ? skills.map((skill) => {
                                return (
                                    <div className="new-project-field-tag">
                                        <span>{formatTagValue(skill.name)}</span>
                                        <button type="button" className="delete-tag-btn"
                                                onClick={() => deleteElement(skill, skills, setSkills)}>x
                                        </button>
                                    </div>)
                            }) : <></>}
                        </div>
                        <p className="new-project__text visually-hidden" id="project-new-skill-error">Error text (Текст
                            ошибки)!</p>
                    </div>

                    <div className="user-education-info block">
                        <div className="user-education-info column-1">
                            <h2>Образование</h2>
                            <div className="new-education__field education-place" id="education-place-block">
                                <p>Место обучения</p>
                                <input className="new-education__field" name="education-place" id="education-place"
                                       autoComplete="off" onChange={() => {
                                    document.getElementById("education-place-block").classList.remove('error')
                                    document.getElementById('education-place-error').classList.add('visually-hidden')
                                }}/>
                                <label className="visually-hidden" htmlFor="education-place">Место обучения</label>
                                <p className="new-education__text visually-hidden" id="education-place-error">Введите
                                    место обучения</p>
                            </div>

                            <div className="new-education__field education-speciality" id="education-speciality-block">
                                <p>Специальность</p>
                                <input className="new-education__field" name="education-speciality"
                                       id="education-speciality" autoComplete="off" onChange={() => {
                                    document.getElementById("education-speciality-block").classList.remove('error')
                                    document.getElementById('education-speciality-error').classList.add('visually-hidden')
                                }
                                }/>
                                <label className="visually-hidden"
                                       htmlFor="education-speciality">Специальность</label>
                                <p className="new-education__text visually-hidden"
                                   id="education-speciality-error">Введите название специальности</p>
                            </div>

                            <div className="new-education__field education-year" id="education-year-block">
                                <p>Год окончания</p>
                                <input class="new-education__field" type="number" min="1900" max="2099"
                                       defaultValue={new Date().getFullYear()} step="1" name="education-year"
                                       id="education-year"
                                       autoComplete="off" onChange={() => {
                                    document.getElementById("education-year-block").classList.remove('error')
                                    document.getElementById('education-year-error').classList.add('visually-hidden')
                                }
                                }/>
                                <label className="visually-hidden" htmlFor="education-year">Год окончания</label>
                                <p className="new-education__text visually-hidden" id="education-year-error">Введите год
                                    окончания обучения, если еще не закончили, укажите предполагаемый</p>
                            </div>

                            {/*<div className="new-education__field field-check-box">*/}
                            {/*    <input className="new-education__field education__check-box" type="checkbox"*/}
                            {/*           name="education-not-finished" id="education-not-finished"/>*/}
                            {/*    <label className="sign-in__label" htmlFor="education-not-finished">В процессе*/}
                            {/*        получения</label>*/}
                            {/*</div>*/}

                            <input type="button" className="new-project-btn" value="Добавить" id="add-new-education"
                                   onClick={() => validAndAddEdu()}/>
                        </div>

                        <div className="user-education-info column-2">
                            <h2>Мое образование</h2>
                            <div className="user-education-info tab">
                                {edus !== undefined ?
                                    edus.map(edu => {
                                        return (
                                            <div className="user-education-info education-row-card">
                                                <div className="education-row-card-params">
                                                    <p className="education-row-card place">{edu.place}</p>
                                                    <p className="education-row-card  specialty">{edu.speciality}</p>
                                                    <p className="education-row-card year">{edu.graduationYear}</p>
                                                </div>
                                                <button className="delete-edu-btn" type="button"
                                                        onClick={() => deleteElement(edu, edus, setEdu)}>x
                                                </button>
                                            </div>
                                        )
                                    }) :
                                    <></>

                                }

                            </div>
                        </div>
                    </div>
                    <div className="new-project__btns">
                        <button type="button" className="new-project-btn new-project-submit-btn" value="Сохранить"
                                id="new-project-submit" onClick={async () => {

                            await Promise.all([sendData(process.env.REACT_APP_API_URL + "/api/PersonalArea/executor/update", res),
                                sendData(process.env.REACT_APP_API_URL + "/api/PersonalArea/contacts/update", links),
                                sendData(process.env.REACT_APP_API_URL + "/api/PersonalArea/categories/update", categories),
                                sendData(process.env.REACT_APP_API_URL + "/api/PersonalArea/skills/update", skills),
                                sendData(process.env.REACT_APP_API_URL + "/api/PersonalArea/educations/update", edus)])
                            window.location.href = "/personal-area";
                        }}
                        >Сохранить
                        </button>
                        <button type="button" className="new-project-btn new-project-submit-cancel" value="Отменить"
                                id="new-project-cancel" onClick={() => {
                            window.location.href = "/personal-area"
                        }}>Отменить
                        </button>
                    </div>
                </form>
            </section>
        </div>
    )
}

export default EditExecutorData;