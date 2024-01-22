import {reviewData} from "./reviewData";
import emptyIcon from "../../../images/empty-user.svg"
import gitIcon from "../../../images/github-icon.svg"
import vkIcon from "../../../images/vk-icon.svg"
import tgIcon from "../../../images/telegram-icon.svg"
import reviewIcon from "../../../images/review-icon.svg"
import checklistIcon from "../../../images/checklist-icon.svg"
// import Header from "../../main/header";
// import Footer from "../../main/footer";
import axios from "axios";
import {useParams} from "react-router-dom";
import React, {useEffect, useState} from "react";
import {executors} from "../project-page/project-info";
import {formatTagValue} from "../format-tag";


const Review = () => {
    const params = useParams();
    const [data, setData] = useState();
    const [userInfo, setUserInfo] = useState();
    const [themes, setThemes] = useState()
    const [competencies, setComps] = useState()

    function fillUserInfo(info) {

    }

    useEffect(() => {
        axios.get(process.env.REACT_APP_API_URL + "/api/Order/detail_order_info/" + params.orderId,
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    },

            })
            .then((response) => {
                console.log(response.data)
                setData(response.data);
                setComps(response.data.orderSkills.map((comp) => {
                    return <div className="field-card-tag" key={"skill-" + comp.id}>{formatTagValue(comp.name)}</div>
                }));

                setThemes(response.data.orderCategories.map((theme) => {
                    return <div className="field-card-tag" key={"theme-" + theme.id}>{formatTagValue(theme.name)}</div>
                }))


                let info = response.data.executorResponse.map((executor) => {
                    if (executor.executorId === Number(params.executorId))
                        return executor;
                }).filter(x => x !== undefined)
                console.log('userInfo');
                console.log(info);
                setUserInfo(info);
                console.log(info)

            })
            .catch((error) => {
                console.log(error)
            });


    }, [])

    function sendReview() {
        //console.log(document.getElementById("new-review-text").value);
        axios.post(process.env.REACT_APP_API_URL + "/api/Order/create_review",
            {
                executorId: params.executorId,
                orderId: params.orderId,
                reviewText: document.getElementById("new-review-text").value
            },
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    },

            })
            .then((response) => {
                console.log("review send");
                window.location.href = "/tasks/my/" + params.orderId;
            })
            .catch((error) => {
                console.log(error)
            });
    }

    return (
        <>
            {/*<Header/>*/}
            {data !== undefined && userInfo !== undefined && userInfo.length > 0 &&
                <div className="main">
                    <div className="new-review">
                        <h1 className="page-title">Написание отзыва</h1>
                        <p className="navigation-after-title"><a href="/personal-area">личный кабинет</a>/<a href="#">написать
                            отзыв</a></p>

                        <div className="project-contractors contractor-card card">
                            <div className="project-contractors contractor-card column-1">
                                <div className="project-contractors contractor-card contractor-data">
                                    <div className="project-contractors contractor-card user-avatar">
                                        <img className="project-contractors contractor-card avatar-image"
                                             src={emptyIcon} alt="Аватар пользователя"/>
                                    </div>

                                    <div className="project-contractors contractor-card contractor-data-rows">
                                        <p className="project-contractors contractor-card contractor-name">{userInfo[0].fio}</p>
                                        <div className="project-contractors age-and-city">
                                            <p className="project-contractors age">Возраст: {userInfo[0].birthdate}</p>
                                            <p className="project-contractors city">Город: {"Екатеринбург"}</p>
                                        </div>
                                        <p className="project-contractors email">E-mail: {userInfo[0].email}</p>
                                        <p className="project-contractors phone">Телефон: {userInfo[0].phone}</p>
                                    </div>
                                </div>

                                <div className="project-contractors contractor-card contractor-links">
                                    {(userInfo[0].vkNickname !== "" || userInfo[0].githubUrl !== "" || userInfo[0].messager !== "") &&
                                        <>
                                            <h3>Ссылки пользователя:</h3>
                                            <div className="contractor-data-card person-links">
                                                {userInfo[0].githubUrl !== "" &&
                                                    <div className="contractor-data-card one-link">
                                                        <img className="contractor-data-card link-icon" src={gitIcon}
                                                             alt="link icon"/>
                                                        <a className="contractor-data-card link-text"
                                                           href="#">{userInfo[0].githubUrl}</a>
                                                    </div>}
                                                {userInfo[0].vkNickname !== "" &&
                                                    <div className="contractor-data-card one-link">
                                                        <img className="contractor-data-card link-icon" src={vkIcon}
                                                             alt="link icon"/>
                                                        <a className="contractor-data-card link-text"
                                                           href="#">{userInfo[0].vkNickname}</a>
                                                    </div>}
                                                {userInfo[0].messager !== "" &&
                                                    <div className="contractor-data-card one-link">
                                                        <img className="contractor-data-card link-icon" src={tgIcon}
                                                             alt="link icon"/>
                                                        <a className="contractor-data-card link-text"
                                                           href="#">{userInfo[0].messager}</a>
                                                    </div>}
                                            </div>
                                        </>
                                    }
                                </div>
                            </div>
                            <div className="project-contractors contractor-card column-2">
                                <div className="project-contractors contractor-card params">
                                    <h3>Параметры:</h3>
                                    <div className="consumer-data-card project-params">
                                        <div className="consumer-data-card one-param">
                                            <img className="consumer-data-card param-icon" src={checklistIcon}
                                                 alt="param icon"/>
                                            <p className="consumer-data-card param-text">Статус: <span>{userInfo[0].isOrderCompleted ? "Завершен" : "В процессе"}</span>
                                            </p>
                                        </div>
                                        <div className="consumer-data-card one-param">
                                            <img className="consumer-data-card param-icon" src={reviewIcon}
                                                 alt="param icon"/>
                                            <p className="consumer-data-card param-text">Отзыв: <span>{userInfo[0].isRated ? "Предоставлен" : "Не предоставлен"}</span>
                                            </p>
                                        </div>
                                    </div>
                                </div>

                                <div className="project-contractors contractor-card buttons">
                                    <a className="project-card-btn" href={"/executors/" + params.executorId}>Перейти к
                                        профилю</a>
                                </div>
                            </div>
                        </div>

                        <section className="project-desc-card">
                            <h2 className="visually-hidden">Описание заказа</h2>
                            <p className="project-card-name">{data.name}</p>
                            <p className="project-card-description">{data.description}</p>
                            <div className="project-card-tags fields-row">
                                <h3>Сферы деятельности:</h3>
                                <div className="project-card-tags tags-fields">
                                    {themes}
                                </div>
                            </div>

                            <div className="project-card-tags skills-row">
                                <h3>Компетенции:</h3>
                                <div className="project-card-tags tags-skills">
                                    {competencies}
                                </div>
                            </div>
                        </section>

                        <form className="new-review-form">
                            <h2>Отзыв на пользователя</h2>
                            <div className="review-block">
                        <textarea className="new-review-text" placeholder="Напишите отзыв" name="new-review-text"
                                  id="new-review-text" rows="32" autoComplete="off"></textarea>
                                <label className="visually-hidden" htmlFor="new-review-text">Отзыв на
                                    пользователя</label>
                                <p className="new-project__text visually-hidden"
                                   id="project-participant-number-error">Error
                                    text (Текст ошибки)!</p>
                            </div>
                            <button type="button" className="review-submit" onClick={sendReview}>Отправить</button>
                        </form>
                    </div>
                </div>}
            {/*<Footer/>*/}
        </>
    )
}

export default Review;