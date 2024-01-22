import {useEffect, useState} from "react";
import axios from "axios";
import emptyAvatar from "../../../images/empty-user.svg"
import deadlineIcon from "../../../images/deadline-icon.svg"
import LogoBlack from "../../../images/logo-black.svg"


const ProjectCardSearch = (props) => {
    const [popup, setPopup] = useState(<></>);
    const [button, setButton] = useState(createButton());
    let skills = props.info.orderSkills.skills.map((skill) => {
        return <div className="field-tag" key={"skillProject-" + props.info.orderId + "-" + skill.id}>{skill.name}</div>
    })

    let orderRoles = props.info.vacancies.map(x=>x.orderRole).map((skill) => {
        return <div className="field-tag" key={"skillProject-" + props.info.orderId + "-" + skill.id}>{skill.name}</div>
    })

    let categories = props.info.orderCategories.categories.map((category) => {
        return <div className="field-tag"
                    key={"categoryProject-" + props.info.orderId + "-" + category.id}>{category.name}</div>
    })

    function viewPopup(id, event) {
        setPopup(<div className="popup-wrapper">
            <div className="popup leave-account-popup">
                <img className="finish-project-popup-logo" src={LogoBlack}/>
                <p className="leave-text">Отправить заявку на принятие заказа?</p>
                <p class="visually-hidden" id="popup-error">Что-то пошло не так, возможно, вы уже откликались?</p>
                <div className="popup-buttons buttons">
                    <a className="project-card-btn" href="#" onClick={(e) => {
                        e.preventDefault()
                        response(id, event);
                    }}>Отправить</a>
                    <a className="project-card-btn" href="#" onClick={hidePopup}>Отмена</a>
                </div>
            </div>
        </div>)
    }

    function hidePopup() {
        setPopup(<></>)
    }

    function response(id, event) {
        axios.post(process.env.REACT_APP_API_URL + "/api/Order/response/" + props.info.orderId, null,
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    },

            })
            .then((response) => {
                console.log("project " + id + " response")
                // event.target.value = "Заявка отправлена";
                // event.target.disabled = true;
                // event.target.hidden = true;
                clearButton()
                hidePopup()
            })
            .catch((error) => {
                console.log(error)
                document.getElementById('popup-error').classList.remove('visually-hidden');
                document.getElementById('popup-error').textContent = error.response.data;
                alert(error.response.data)
            });
    }

    function createChatroom(orderId) {
        axios.post(process.env.REACT_APP_API_URL + "/api/Order/create-chat-room", null,
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    },
                params: {
                    orderId: props.info.orderId
                }

            })
            .then((response) => {
                console.log("project " + orderId + " create chatroom")
                window.location.href = '/messenger'
                //clearButton()
                //hidePopup()
            })
            .catch((error) => {
                alert(error.response.data)
                console.log(error)
              //  document.getElementById('popup-error').classList.remove('visually-hidden');
               // document.getElementById('popup-error').textContent = error.response.data;
            });
    }

    function createButton() {
        if (window.sessionStorage.getItem("role") === "executor_role" && !props.info.isResponsed)
            return <input className="card-button" type="button" onClick={(e) => {
                e.preventDefault();
                //viewPopup(props.info.orderId, e);
                createChatroom(props.info.orderId);
            }} value="Написать"/>
        return null
    }

    function clearButton() {
        setButton(<></>);
    }

    return (
        <>
            {popup}
            <div className="project-card" key={'div-' + props.info.orderId}>
                <div className="column-1">
                    <h3>{props.info.name}</h3>
                    <p className="project-description">{props.info.description}</p>
                    <div className="project-fields">
                        <h4>Сферы деятельности:</h4>
                        <div className="user-fields fields-tags">
                            {categories}
                        </div>
                    </div>
                    <div className="project-fields">
                        <h4>Компетенции:</h4>
                        <div className="user-fields fields-tags">
                            {skills}
                        </div>
                    </div>
                    <div className="project-fields">
                        <h4>Роли:</h4>
                        <div className="user-fields fields-tags">
                            {orderRoles}
                        </div>
                    </div>
                </div>
                <div className="column-2">
                    <div className="project-params">
                        <div className="row">
                            <div className="customer-avatar">
                                <img className="customer-avatar" src={emptyAvatar}
                                     alt="Аватар заказчика"/>
                            </div>
                            <p className="customer-name">{props.info.companyName}</p>
                        </div>
                        <div className="row">
                            <img className="project-card-icon" src={deadlineIcon}
                                 alt="Иконка дедлайна"/>
                            <p className="deadline">Дедлайн: <span>{new Date(props.info.deadline).toLocaleString("ru", {
                                year: 'numeric',
                                month: 'long',
                                day: 'numeric'
                            })}</span></p>
                        </div>
                    </div>
                    <div className="project-card-btns">
                        <input className="card-button blue" type="button" onClick={() => {
                            window.location.href = "/tasks/" + props.info.orderId
                        }} value="Поcмотреть"/>
                        {button}
                    </div>
                </div>
            </div>
        </>
    )
}

export default ProjectCardSearch