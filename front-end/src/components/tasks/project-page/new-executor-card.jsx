import emptyUserIcon from "../../../images/empty-user.svg"
import {useState} from "react";
import axios from "axios";

const NewExecutorCard = (props) => {
    //let themes = props.info.themes.map((theme) => {return <div className="field-card-tag">{theme}</div>})
    //let competence = props.info.themes.map((comp) => {return <div className="field-card-tag">{comp}</div>})
    let themes = props.info.categories.map((theme) => {
        return <div className="field-tag">{theme.name}</div>
    });
    let competence = props.info.skills.map((skill) => {
        return <div className="field-tag">{skill.name}</div>
    });
    ;

    function accept() {
        axios.post(process.env.REACT_APP_API_URL + "/api/Order/update_response",
            null,

            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    },

                params:
                    {
                        'isAccept': true,
                        'responseId': props.info.responseId,
                        vacancyId: props.info.orderVacancy.id
                    }

            })
            .then((response) => {
                console.log("you accepted user " + props.info.executorId)
                window.location.reload();
            })
            .catch((error) => {
                console.log(error)
            });
    }

    function reject() {
        axios.post(process.env.REACT_APP_API_URL + "/api/Order/update_response",
            null,

            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    },

                params:
                    {
                        'isAccept': false,
                        'responseId': props.info.responseId,
                        vacancyId: props.info.orderVacancy.id
                    }

            })
            .then((response) => {
                if (response.status === 200) {
                    console.log("you accepted user" + props.info.executorId)
                    window.location.reload();
                }


            })
            .catch((error) => {
                console.log(error)
            });
    }


    return (
        <div className="project-new-contractors contractor-card card">
            <div className="new-contractor contractor-data">
                <div className="new-contractor user-avatar">
                    <img className="new-contractor avatar-image" src={emptyUserIcon}
                         alt="Аватар пользователя"/>
                </div>
                <div className="new-contractor contractor-info">
                    <p className="new-contractor contractor-username">{props.info.username}</p>
                    <p className="new-contractor project-finished">Проектов
                        закончено: <span>{props.info.completedOrders}</span></p>
                    <p className="new-contractor project-finished">Отклик на
                        компетенцию: <span>{props.info.orderVacancy.orderRole.name}</span></p>

                    <div className="new-contractors buttons">
                        <a className="project-card-btn blue" onClick={accept}>Принять в проект</a>
                        <a className="project-card-btn" href={"/executors/" + props.info.executorId}>Перейти к
                            профилю</a>
                        <a className="project-card-btn" onClick={reject}>Отклонить</a>
                    </div>
                </div>
            </div>

            <div className="new-contractor contractor-fields">
                <h3>Сферы деятельности:</h3>
                <div className="new-contractor tags-fields">
                    {themes}
                </div>
            </div>

            <div className="new-contractor contractor-skills">
                <h3>Компетенции:</h3>
                <div className="new-contractor tags-skills">
                    {competence}
                </div>
            </div>
        </div>
    )
}

export default NewExecutorCard;