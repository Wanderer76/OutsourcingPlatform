// import Header from "../../main/header";
// import Footer from "../../main/footer";
import React, {useEffect} from "react";
import {useState} from "react";
import {removeErrorMessage, removeErrorMessageForInputs, sendData} from "./valid-and-send-new-project-data";
import axios from "axios";
import {formatTagValue} from "../format-tag";
import {addElement, checkArray, deleteElement} from "../add-check-delete-tags";
import SelectVacancy from "./vacancy-select";
import competenceBlock from "../../personal-area/executor/competence-block";

const CreateNewTask = () => {
    const [themes, setTheme] = useState([]);
    const [competences, setCompetences] = useState([]);
    const [orderRoles, setOrderRoles] = useState([]);
    const [themesView, setThemesView] = useState();
    const [compsView, setCompsView] = useState();
    const [orderRolesView, setOrderRolesView] = useState();
    const [isViewParticipantCount, setViewParticipantCount] = useState(true);
    useEffect(() => {
        axios.get(process.env.REACT_APP_API_URL + "/api/Competencies/skills/0/40",
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

        axios.get(process.env.REACT_APP_API_URL + "/api/Competencies/categories/0/40",
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
                    setThemesView(response.data.map((comp) => {
                        return <option value={comp['name']} name={comp['id']}>{comp["name"]}</option>
                    }));
            })
            .catch((error) => {
                console.log(error)
            });
        axios.get(process.env.REACT_APP_API_URL + "/api/Competencies/order-roles/0/40",
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
                    setOrderRolesView(response.data.map((comp) => {
                        return <option value={comp['name']} name={comp['id']}>{comp["name"]}</option>
                    }));
            })
            .catch((error) => {
                console.log(error)
            });

    }, [])

    function clearCompetence(e) {
        console.log(e.target);
        if (e.target.checked) {
            setCompetences([]);
            document.getElementById("competence-selector").classList.add("visually-hidden");
            document.getElementById("competence-selected").classList.add("visually-hidden");
        } else {
            document.getElementById("competence-selector").classList.remove("visually-hidden");
            document.getElementById("competence-selected").classList.remove("visually-hidden");
        }
    }

    const themesBlock = themes.map((theme) => {
        return (
            <div className="new-project-field-tag" key={theme}>
                <span>{formatTagValue(theme.name)}</span>
                <button className="delete-tag-btn" type="button"
                        onClick={() => deleteElement(theme, themes, setTheme)}>x
                </button>
            </div>
        )
    })

    const competenceBlock = competences.map((comp) => {
        return (
            <div className="new-project-skill-tag" key={comp}>
                <span>{formatTagValue(comp.name)}</span>
                <button className="delete-tag-btn" type="button"
                        onClick={() => deleteElement(comp, competences, setCompetences)}>x
                </button>
            </div>
        )
    })

    return (
        // <><Header />
        <div className="main">
            <section className="new-project">
                <h1 className="page-title">Форма создания заказа</h1>
                <p className="navigation-after-title"><a href="/personal-area">личный кабинет</a>/<a
                    href="/tasks/create">форма создания заказа</a>
                </p>
                <form className="new-project" name="new-project">

                    <div className="new-project__field project-name">
                        <p>Название заказа</p>
                        <textarea className="new-project__field project-name" placeholder="Введите название заказа"
                                  name="name" id="project-name" rows="4" autoComplete="off"
                                  onInput={removeErrorMessageForInputs}></textarea>
                        <label className="visually-hidden" htmlFor="project-name">Название проекта</label>
                        <p className="new-project__text visually-hidden" id="project-name-error">Обязательно укажите
                            название вашего замечательного проекта!</p>
                    </div>

                    <div className="new-project__field project-description">
                        <p>Описание задачи</p>
                        <textarea className="new-project__field project-description"
                                  placeholder="Введите описание задачи"
                                  name="description" id="project-description" rows="32"
                                  autoComplete="off" onInput={removeErrorMessageForInputs}></textarea>
                        <label className="visually-hidden" htmlFor="project-description">Описание задачи</label>
                        <p className="new-project__text visually-hidden" id="project-description-error">Обязательно
                            введите описание, так исполнитель будет понимать, что ему делать</p>
                    </div>

                    <div className="inputs-row">
                        <div className="new-project__field project-deadline">
                            <p>Дедлайн заказа</p>
                            <input type="date" className="new-project__field project-deadline" name="deadline"
                                   id="project-deadline" onInput={removeErrorMessageForInputs}/>
                            <label className="visually-hidden" htmlFor="project-deadline">Дедлайн проекта</label>
                            <p className="new-project__text visually-hidden" id="project-deadline-error">Укажите срок
                                выполнения проекта</p>
                        </div>
                        {/*{isViewParticipantCount &&*/}
                        {/*    <div className="new-project__field project-participant-number">*/}
                        {/*        <p>Max количество участников</p>*/}
                        {/*        <input type="number" min="1" max="100" className="new-project__field project-participant-number"*/}
                        {/*               name="maxWorkers" id="project-participant-number" onInput={removeErrorMessageForInputs}/>*/}
                        {/*        <label className="visually-hidden" htmlFor="project-participant-number">Max количество*/}
                        {/*            участников</label>*/}
                        {/*        <p className="new-project__text visually-hidden" id="project-participant-number-error">Укажите возможное количество участников</p>*/}
                        {/*    </div>*/}
                        {/*}*/}
                        {/*<div className="project-competences-check-box">*/}
                        {/*    <input className="auth__field sign-in__check-box" type="checkbox"*/}
                        {/*           name="no-contractor-limit" id="no-contractor-limit" onChange={() => {setViewParticipantCount(!isViewParticipantCount)}}/>*/}
                        {/*    <label className="sign-in__label" htmlFor="no-contractor-limit">Неограниченное*/}
                        {/*        количество участников</label>*/}
                        {/*</div>*/}

                    </div>

                    <div className="inputs-row">
                        <div className="new-project__field project-new-field">
                            <p>Сфера деятельности</p>
                            <select className="new-project-select" name="project-new-field" id="project-new-field"
                                    onChange={() => {
                                        addElement("project-new-field", themes, setTheme)
                                        removeErrorMessage("project-new-field-error");
                                    }}>
                                <option value="">-- Выберите сферу --</option>
                                {themesView}
                            </select>
                            <label className="visually-hidden" htmlFor="project-new-field">Сфера деятельности</label>
                        </div>
                    </div>

                    <div className="new-project-fields">
                        <h2>Выбранные сферы деятельности</h2>
                        <div className="new-project-field-tags">
                            {themesBlock}

                        </div>
                        <p className="new-project__text visually-hidden" id="project-new-field-error">Выберите в какой
                            сфере ваш проект</p>
                    </div>

                    <div className="project-competences-check-box">
                        <input className="auth__field sign-in__check-box" type="checkbox" name="no-competencies"
                               id="no-competencies" onChange={clearCompetence}/>
                        <label className="sign-in__label" htmlFor="no-competencies">Не указывать компетенции</label>
                    </div>

                    <div className="inputs-row" id="competence-selector">
                        <div className="new-project__skill project-new-skill">
                            <p>Компетенции</p>
                            <select className="new-project-select" name="project-new-skill" id="project-new-skill"
                                    onChange={() => {
                                        let i = document.getElementById('project-new-skill')
                                        let element = i.value;
                                        if (element !== "" && checkArray(competences, element))
                                            setCompetences(([{'id': -1, 'name': element}, ...competences]));
                                        removeErrorMessage('project-new-skill-error');
                                    }
                                    }>
                                <option value="">-- Выберите компетенцию --</option>
                                {compsView}
                            </select>
                            <label className="visually-hidden" htmlFor="project-new-skill">Компетенции</label>
                        </div>
                    </div>

                    <div className="new-project-skills" id="competence-selected">
                        <h2>Выбранные компетенции</h2>
                        <div className="new-project-skills-tags">
                            {competenceBlock}

                        </div>
                        <p className="new-project__text visually-hidden" id="project-new-skill-error">Выберите
                            необходмые компетенции или, если вы не знаете какие нужны, укажите это</p>
                    </div>


                    <div className="inputs-row" id="competence-selector">
                        <div className="">
                            <p>Роли</p>
                            <div className="new-project__field project-participant-number">
                                <select className="new-project-select" name="project-new-role"
                                        id="project-new-role-header"
                                        onChange={() => {
                                           // addElement("project-new-role-header", orderRoles, setOrderRoles);
                                            let i = document.getElementById('project-new-role-header')
                                            let element = i.value;
                                            let id = i.options[i.selectedIndex].name;

                                            if (id === undefined && element === "") {
                                                setOrderRoles(orderRoles)
                                            }
                                            if (element !== "" && checkArray(orderRoles, element))
                                                setOrderRoles([{'id': orderRoles.length + 1, 'name': element, 'maxWorkers': 1}, ...orderRoles]);
                                            removeErrorMessage('project-new-role-error');
                                            const selector = document.getElementById('project-new-role-header')
                                            selector.options[0].selected = true;
                                            removeErrorMessage('project-new-role-error');
                                        }
                                        }>
                                    <option selected>-- Выберите Роль --</option>

                                    {orderRolesView}

                                </select>
                            </div>

                            <br></br>


                            {
                                !orderRoles ? null :
                                    orderRoles.sort((a, b) => a.id > b.id ? 1 : -1).map((x) => {
                                        const orderRolesName = orderRoles.map(x => x.name);
                                        return (
                                            <>
                                                <SelectVacancy key={x.name} orderRoles={orderRoles}
                                                               orderRolesView={[...orderRolesView].filter(comp =>
                                                                   !orderRolesName.includes(comp.props.value))}
                                                               currentValue={x}
                                                               setOrderRoles={setOrderRoles}
                                                               showDelete={true}>
                                                </SelectVacancy>
                                                <br></br>

                                            </>
                                        );
                                    }, [])
                            }
                            <label className="visually-hidden" htmlFor="project-new-role-header">Роли</label>
                        </div>
                        <br></br>
                        <p className="new-project__text visually-hidden" id="project-new-role-error">Выберите
                            необходмые роли</p>
                    </div>

                    {/*<div className="new-project-skills" id="competence-selected">*/}
                    {/*    <h2>Выбранные компетенции</h2>*/}
                    {/*    <div className="new-project-skills-tags">*/}
                    {/*        {competenceBlock}*/}
                    {/*    </div>*/}
                    {/*    <p className="new-project__text visually-hidden" id="project-new-skill-error">Выберите*/}
                    {/*        необходмые компетенции или, если вы не знаете какие нужны, укажите это</p>*/}
                    {/*</div>*/}

                    <input type="button" className="new-project-btn new-project-submit-btn" value="Создать проект"
                           id="new-project-submit"
                           onClick={() => sendData(themes, competences, orderRoles, process.env.REACT_APP_API_URL + "/api/Order/create")}/>
                </form>
            </section>
        </div>
        // <Footer/></>
    )
}

export default CreateNewTask;