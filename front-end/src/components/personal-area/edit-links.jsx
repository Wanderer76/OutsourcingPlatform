import {useEffect, useState} from "react";

const EditLinks = (props) => {
    const curLinks = fillCurLinks();


    function fillCurLinks() {
        let copy = [];
        for (let key in props.links) {
            if (props.links[key] !== null)
                copy.push(<div className="new-project-field-tag">
                    <span>{props.links[key]}</span>
                    <button className="delete-tag-btn">x</button>
                </div>)
        }
        console.log(copy)
        return copy;
    }

    function addLink() {
        let type = document.getElementById("link-new-field").value;
        let value = document.getElementById("current-link").value;
        let copy = props.links;
        copy[type] = value
        props.setLinks(copy);
    }


    return(
        <>
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

                    {curLinks}

                </div>
                <p className="new-project__text visually-hidden" id="project-new-links-error">Error text (Текст
                    ошибки)!</p>
            </div>

            <div className="new-project__field person-description">
                <p>О себе</p>
                <textarea className="new-project__field person-description" placeholder="Напишите о себе"
                          name="person-description" id="person-description" rows="8"
                          autoComplete="off"></textarea>
                <label className="visually-hidden" htmlFor="person-description">О себе</label>
                <p className="new-project__text visually-hidden" id="person-description-error">Error text (Текст
                    ошибки)!</p>
            </div>
        </>
    )
}

export default EditLinks;