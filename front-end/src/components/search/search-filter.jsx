import React, {useEffect, useState} from "react";
import axios from "axios";
import ProjectCardSearch from "./projects/project-card-search";
import {removeErrorMessage} from "../tasks/create-update/valid-and-send-new-project-data";
import {addElement, deleteElement} from "../tasks/add-check-delete-tags";
import {formatTagValue} from "../tasks/format-tag";

const SearchFilter = ({thing, setSkillsFilter, setThemesFilter, sendForm}) => {
    const [skills, setSkills] = useState();
    const [competenciesList, setComps] = useState();

    const [themes, setTheme] = useState([]);
    const [competence, setCompetence] = useState([]);
    const config = window.sessionStorage.getItem('token') !== null ? {
            headers:
                {'Access-Control-Allow-Origin' : '*',
                    'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                    'Authorization' : "Bearer " + window.sessionStorage.getItem('token')
                },

        } :
        {
            headers:
                {'Access-Control-Allow-Origin' : '*',
                    'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                },

        }
    useEffect(() => {
        axios.get(process.env.REACT_APP_API_URL+"/api/Competencies/skills/0/30", config)
            .then((response) => {
                if (response.status === 200) {
                    console.log(response.data);
                    setSkills(response.data.map((skill) => {
                        return <option key={'skill-'+skill.id} value={skill.name}>{skill.name}</option>
                    }));
                }
            })
            .catch((error) => {console.log(error)});

        axios.get(process.env.REACT_APP_API_URL+"/api/Competencies/categories/0/30",config)
            .then((response) => {
                if (response.status === 200) {
                    console.log(response.data);
                    setComps(response.data.map((comp) => {
                        return <option key={'comp-'+comp.id} value={comp.name}>{comp.name}</option>
                    }));
                }
            })
            .catch((error) => {console.log(error)});

    }, [])

    function findProjectsByFilter() {
        axios.post()
    }

    const themesBlock = themes.map((theme, index) => {
        return(
            <div className="new-project-field-tag" key={"themeFilter-"+index}>
                <span>{formatTagValue(theme.name)}</span>
                <button className="delete-tag-btn" type="button"
                        onClick={() => {
                            deleteElement(theme, themes, setTheme);
                            deleteElementFromFilter(setThemesFilter,theme)
                        }}>x</button>
            </div>
        )
    })


    const competenceBlock = competence.map((comp, index) => {
        return(
            <div className="new-project-skill-tag" key={"skillFilter-"+index}>
                <span>{formatTagValue(comp.name)}</span>
                <button className="delete-tag-btn" type="button"
                        onClick={() => {
                            deleteElement(comp, competence, setCompetence)
                            deleteElementFromFilter(setSkillsFilter, comp);
                        }}>x</button>
            </div>
        )
    })

    function deleteElementFromFilter(setFilterArr, element) {
        setFilterArr((preview) => [...preview.filter(el => el.name !== element.name)])
    }


    return(
        <section className="search-filter">
            <h1 className="page-title">Поиск {thing}</h1>
            <p className="navigation-after-title"><a href="/">главная страница</a>/
                {thing === "заказов" ?
                    <a href="/tasks/search">поиск заказов</a> :
                    <a href="/executors/search">поиск исполнителей</a>
                }
            </p>
            <form className="new-project" name="search-filter">
                <div className="inputs-row">
                    <div className="new-project__field project-new-field">
                        <p>Сфера деятельности</p>
                        <select className="new-project-select" name="project-new-field" id="project-new-field"
                                onChange={(event) => {
                                    addElement("project-new-field", themes, setTheme);
                                    setThemesFilter(prevState => [...prevState, {'id': 0, 'name': event.target.value}])
                                }}>
                            <option value="">-- Выберите сферу --</option>
                            {competenciesList !== undefined ? competenciesList : <option value="">Загрузка данных</option>}
                        </select>
                        <label className="visually-hidden" htmlFor="project-new-field">Сфера деятельности</label>
                    </div>

                    <div className="new-project__skill project-new-skill">
                        <p>Компетенции</p>
                        <select className="new-project-select" name="project-new-skill" id="project-new-skill"
                                onChange={(event) => {
                                    addElement("project-new-skill", competence, setCompetence);
                                    setSkillsFilter(prevState => [...prevState, {'id': 0, 'name': event.target.value}])
                                    // skillsFilter.push({'id': 0, 'name': event.target.value})
                                    // console.log(skillsFilter);
                        }}>
                            <option value="">-- Выберите компетенцию --</option>
                            {skills !== undefined ? skills : <option value="">Загрузка данных</option>}
                        </select>
                        <label className="visually-hidden" htmlFor="project-new-skill">Дедлайн проекта</label>
                    </div>
                </div>
                <div className="new-project-fields">
                    <h2>Выбранные сферы деятельности</h2>
                    <div className="new-project-field-tags">
                        {themesBlock}
                    </div>
                    <p className="new-project__text visually-hidden" id="project-new-field-error">Error text (Текст
                        ошибки)!</p>
                </div>

                <div className="new-project-skills">
                    <h2>Выбранные компетенции</h2>
                    <div className="new-project-skills-tags">
                        {competenceBlock}
                    </div>
                    <p className="new-project__text visually-hidden" id="project-new-skill-error">Error text (Текст
                        ошибки)!</p>
                </div>

                <input type="button" className="filter-search-btn" value="Поиск" id="filter-search-btn" onClick={(e) => {
                    e.preventDefault();
                    sendForm();
                }}/>
            </form>
        </section>
    )
}

export default SearchFilter;