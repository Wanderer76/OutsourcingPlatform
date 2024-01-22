import reviewIcon from "../../../images/review-icon.svg"
import checklistIcon from "../../../images/checklist-icon.svg"
import shakeHandsIcon from "../../../images/shake-hands-icon.svg"
import emptyUserIcon from "../../../images/empty-user.svg"
import gitIcon from "../../../images/github-icon.svg"
import vkIcon from "../../../images/vk-icon.svg"
import tgIcon from "../../../images/telegram-icon.svg"
import React, {useState} from "react";
import {useParams} from "react-router-dom";
import axios from "axios";
import gitLogo from "../../../images/github-icon.svg";
import vk from "../../../images/vk-icon.svg";


const ExecutorCard = (props) => {
    const [popup, setPopup] = useState(<></>)
    const params = useParams()

    function viewPopup() {
        setPopup(
            <div className="popup-wrapper">
                <div className="popup delete-project-popup">
                    <h2>Удалить исполнителя из заказа?</h2>
                    <div className="user-for-delete">
                        <div className="user-photo">
                            <img src={emptyUserIcon}/>
                        </div>
                        <div className="user-for-delete-name">{props.info.fio}</div>
                    </div>
                    <textarea className="reason-for-deleting" id="reason-for-deleting" name="reason-for-deleting"
                              placeholder="Опишите причину, по которой вы решили удалить исполнителя из Вашего заказа."></textarea>
                    <div className="popup-delete-buttons buttons">
                        <a className="project-card-btn" onClick={hidePopup}>Отмена</a>
                        <a className="project-card-btn" onClick={deleteExecutor}>Удалить</a>
                    </div>
                </div>
            </div>)
    }

    function hidePopup() {
        setPopup(<></>)
    }

    function deleteExecutor() {
        let messageValue = document.getElementById("reason-for-deleting").value;
        console.log(params.id);
        axios.delete(process.env.REACT_APP_API_URL + '/api/Order/delete_executor/', {
            headers:
                {
                    'Access-Control-Allow-Origin': '*',
                    'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                    'Authorization': "Bearer " + window.sessionStorage.getItem('token')

                },
            data:
                {
                    executorId: props.info.executorId,
                    orderId: params.id,
                    vacancyId: props.info.orderVacancy.id,
                    message: messageValue
                }
        })
            .then((response) => {
                console.log(response.data);
                console.log("deleted");
                window.location.reload();
            })
            .catch((error) => {
                console.log(error)
            });


    }

    return (
        <>
            {popup}
            <div
                className={props.info.isCompleted ? "project-contractors contractor-card card blue" : "project-contractors contractor-card card"}>
                <div className="project-contractors contractor-card column-1">
                    <div className="project-contractors contractor-card contractor-data">
                        <div className="project-contractors contractor-card user-avatar">
                            <img className="project-contractors contractor-card avatar-image"
                                 src={props.info.avatar === undefined ? emptyUserIcon : props.info.avatar}
                                 alt="Аватар пользователя"/>
                        </div>

                        <div className="project-contractors contractor-card contractor-data-rows">
                            <p className="project-contractors contractor-card contractor-name">{props.info.fio}</p>
                            <div className="project-contractors age-and-city">
                                <p className="project-contractors age">Возраст: {props.info.age}</p>
                                <p className="project-contractors city">Город: {props.info.city}</p>
                            </div>
                            <p className="project-contractors email">E-mail: {props.info.email}</p>
                            <p className="project-contractors phone">Телефон: {props.info.phone}</p>
                        </div>
                    </div>
                    {props.info.contacts.githubUrl !== "" || props.info.contacts.vkNickname !== "" || props.info.contacts.messager !== "" ?
                        <div className="project-contractors contractor-card contractor-links">
                            <h3>Ссылки пользователя:</h3>
                            <div className="contractor-data-card person-links">
                                {props.info.contacts.githubUrl !== null && props.info.contacts.githubUrl !== undefined && props.info.contacts.githubUrl !== "" &&
                                    <a target="_blank"
                                       title={props.info.contacts.githubUrl.includes("github.com/") ? props.info.contacts.githubUrl : "https://github.com/" + props.info.contacts.githubUrl}
                                       href={props.info.contacts.githubUrl.includes("github.com/") ? props.info.contacts.githubUrl : "https://github.com/" + props.info.contacts.githubUrl}><img
                                        className="social-logo" src={gitIcon} alt="Логотип Github"/></a>
                                }
                                {props.info.contacts.vkNickname !== null && props.info.contacts.vkNickname !== undefined && props.info.contacts.vkNickname !== "" &&
                                    <a target="_blank"
                                       title={props.info.contacts.vkNickname.includes("vk.com") ? props.info.contacts.githubUrl : "https://vk.com/" + props.info.contacts.vkNickname}
                                       href={props.info.contacts.vkNickname.includes("vk.com/") ? props.info.contacts.githubUrl : "https://vk.com/" + props.info.contacts.vkNickname}><img
                                        className="social-logo" src={vkIcon} alt="Логотип VK"/></a>
                                }
                                {props.info.contacts.messager !== null && props.info.contacts.messager !== undefined && props.info.contacts.messager !== "" &&
                                    <a target="_blank"
                                       title={props.info.contacts.messager.includes("t.me") ? props.info.contacts.messager : "https://t.me/" + props.info.contacts.messager}
                                       href={props.info.contacts.messager.includes("t.me/") ? props.info.contacts.messager : "https://t.me/" + props.info.contacts.messager}><img
                                        className="social-logo" src={tgIcon} alt="Логотип VK"/></a>
                                }
                                {/*{props.info.contacts.githubUrl !== "" &&*/}
                                {/*<div className="contractor-data-card one-link">*/}
                                {/*    <img className="contractor-data-card link-icon" src={gitIcon}/>*/}
                                {/*        <a className="contractor-data-card link-text" href="#">{props.info.contacts.githubUrl}</a>*/}
                                {/*</div> }*/}

                                {/*{props.info.contacts.vkNickname !== "" &&*/}
                                {/*<div className="contractor-data-card one-link">*/}
                                {/*    <img className="contractor-data-card link-icon" src={vkIcon}*/}
                                {/*         alt="link icon"/>*/}
                                {/*        <a className="contractor-data-card link-text" href="#">{props.info.contacts.vkNickname}</a>*/}
                                {/*</div> }*/}

                                {/*{props.info.contacts.messager !== "" &&*/}
                                {/*<div className="contractor-data-card one-link">*/}
                                {/*    <img className="contractor-data-card link-icon" src={tgIcon}*/}
                                {/*         alt="link icon"/>*/}
                                {/*        <a className="contractor-data-card link-text"*/}
                                {/*           href="#">{props.info.contacts.messager}</a>*/}
                                {/*</div> }*/}
                            </div>
                        </div> :
                        <></>
                    }

                </div>
                <div className="project-contractors contractor-card column-2">
                    <div className="project-contractors contractor-card params">
                        <h3>Параметры:</h3>
                        <div className="consumer-data-card project-params">
                            <div className="consumer-data-card one-param">
                                <img className="consumer-data-card param-icon" src={checklistIcon}
                                     alt="param icon"/>
                                <p className="consumer-data-card param-text">Роль: <span>{props.info.orderVacancy.orderRole.name}</span>
                                </p>
                            </div>
                            <div className="consumer-data-card one-param">
                                <img className="consumer-data-card param-icon" src={checklistIcon}
                                     alt="param icon"/>
                                <p className="consumer-data-card param-text">Статус: <span>{!props.info.isCompleted ? "В процессе" : "Завершен"}</span>
                                </p>
                            </div>
                            {props.info.projectStatus === "Завершен" ?
                                <div className="consumer-data-card one-param">
                                    <img className="consumer-data-card param-icon" src={reviewIcon}
                                         alt="param icon"/>
                                    <p className="consumer-data-card param-text">Отзыв: <span>{props.info.reviewStatus}</span>
                                    </p>
                                </div> :

                                <div className="consumer-data-card one-param">
                                    <img className="consumer-data-card param-icon" src={shakeHandsIcon}
                                         alt="param icon"/>
                                    <p className="consumer-data-card param-text">Проектов
                                        завершено: <span>{props.info.completedOrders}</span></p>
                                </div>
                            }

                        </div>
                    </div>

                    <div className="project-contractors contractor-card buttons">
                        <a className="project-card-btn" href={"/executors/" + props.info.executorId}>Перейти к
                            профилю</a>
                        {!props.info.isResourceUploaded && null}
                        {props.info.isResourceUploaded &&
                            <a className="project-card-btn blue"
                               href={process.env.REACT_APP_API_URL + "/files/ObjectStorage/get-project-file/" + props.info.responseId}


                            >Скачать
                                файл
                                результатом</a>}
                        {!props.info.isRated && props.info.isCompleted ?
                            <a className="project-card-btn blue"
                               href={"/review/" + props.info.executorId + "/" + props.orderId}>Написать отзыв</a> :
                            !props.info.isCompleted && !props.isCompleted ?
                                <a className="project-card-btn" onClick={viewPopup}>Удалить исполнителя</a> : <></>
                        }

                    </div>
                </div>
            </div>
        </>
    )
}

export default ExecutorCard;