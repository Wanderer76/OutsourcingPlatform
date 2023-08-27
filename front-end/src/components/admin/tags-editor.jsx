import React, {useEffect, useState} from "react";
import axios from "axios";

const TagsEditor = () => {
    const [themes, setThemes] = useState([])
    const [skills, setSkills] = useState([])

    useEffect(() => {
        axios.get(process.env.REACT_APP_API_URL+"/api/Competencies/categories/0/100",
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    }
            })
            .then((response) => {
                setThemes(response.data)
            })
            .catch((error) => {
                console.log(error);
            })

        axios.get(process.env.REACT_APP_API_URL+"/api/Competencies/skills/0/100",
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    }
            })
            .then((response) => {
                setSkills(response.data)
            })
            .catch((error) => {
                console.log(error);
            })



    }, [])

    function deleteTag(tagId, isTheme, actions) {
        axios.post(process.env.REACT_APP_API_URL+"/api/Competencies/"+(isTheme ? "categories": "skills")+"/delete/"+tagId, null,
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    }
            })
            .then((response) => {
                actions(tagId);
            })
            .catch((error) => {
                console.log(error);
            })
    }


    function addSkill(value) {
        axios.post(process.env.REACT_APP_API_URL + "/api/Competencies/skills/create",
            null,
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    },
                params: {
                    name: value
                }

            })
            .then((response) => {
                console.log(response.data);

            })
            .catch((error) => {
                console.log(error)
            });
    }

    function addTheme(value) {
        axios.post(process.env.REACT_APP_API_URL + "/api/Competencies/categories/create",
            null,
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    },
                params: {
                    name: value
                }

            })
            .then((response) => {
                console.log(response.data);

            })
            .catch((error) => {
                console.log(error)
            });
    }



    return(
        <section className="admin-panel">
            <h1 className="tags-page">Редактор тегов</h1>
            <div className="tags-editors">
                <div className="fields-editor">
                    <h2>Сферы деятельности</h2>
                    <h3>Добавление нового тега:</h3>
                    <div className="add-tag-row">
                        <input className="add-tag-input" type="text" placeholder="Название тега (в форме текста)"
                               id="add-tag-theme-input"/>
                            <label className="visually-hidden" htmlFor="add-tag-field-input">Сфера деятельности</label>
                            <input type="button" className="add-tag-button" value="Добавить" onClick={() => addTheme(document.getElementById("add-tag-theme-input").value)}/>
                    </div>
                    <h3>Список всех тегов:</h3>
                    <ul className="all-tags-list">
                        {themes.map((theme) =>
                                <li key={"theme-"+theme.id} className="tag-item" data-id="1qerfmkFAFsfd">
                                    <div className="tag-item-name">#{theme.name}</div>
                                    <input className="tag-item-remove" type="button" value="x" data-id="1qerfmkFAFsfd"
                                           onClick={() => deleteTag(theme.id, true, (item) => {setThemes((prevState) => prevState.filter((tag) => tag.id !== item))})}/>
                                </li>)}

                    </ul>
                </div>
                <div className="skills-editor">
                    <h2>Компетенции</h2>
                    <h3>Добавление нового тега:</h3>
                    <div className="add-tag-row">
                        <input className="add-tag-input" type="text" placeholder="Название тега (в форме текста)"
                               id="add-tag-skill-input"/>
                            <label className="visually-hidden" htmlFor="add-tag-skill-input">Компетенция</label>
                            <input type="button" className="add-tag-button" value="Добавить" onClick={() => addSkill(document.getElementById("add-tag-skill-input").value)}/>
                    </div>
                    <h3>Список всех тегов:</h3>
                    <ul className="all-tags-list">
                        {skills.map((tag) =>
                            <li key={"skills-"+tag.id} className="tag-item" data-id="1qerfmkFAFsfd">
                                <div className="tag-item-name">#{tag.name}</div>
                                <input className="tag-item-remove" type="button" value="x" data-id="1qerfmkFAFsfd"
                                       onClick={() => deleteTag(tag.id, false, (item) => {setSkills((prevState) => prevState.filter((tag) => tag.id !== item))})}/>
                            </li>)}


                    </ul>
                </div>
            </div>
        </section>
    )
}

export default TagsEditor