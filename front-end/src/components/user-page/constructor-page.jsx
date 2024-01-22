import CompetenceBlock from "../personal-area/executor/competence-block";
import Educations from "../personal-area/executor/educations";
import ReviewCard from "./review-card";
import React, {useEffect, useState} from "react";
import axios from "axios";
import {useParams} from "react-router-dom";
import emptyUserIcon from "../../images/empty-user.svg"
// import Header from "../main/header";
// import Footer from "../main/footer";
import gitLogo from "../../images/github-icon.svg";
import vk from "../../images/vk-icon.svg";
import {_calculateAge} from "../personal-area/calculate-age";

const ConstructorPage = (props) => {
    const [data, setData] = useState();
    const {id} = useParams();
    const [reviews, setReviews] = useState([])
    const [count, setCount] = useState(0);
    const [offset, setOffset] = useState(0);
    let headers = window.sessionStorage.getItem('token') === null ?
        {
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
        } :
        {
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
            'Authorization': "Bearer " + window.sessionStorage.getItem('token')
        }

    useEffect(() => {
        axios.get(process.env.REACT_APP_API_URL + "/api/Executor/executor_detail/" + id,
            {
                headers,

            })
            .then((response) => {
                console.log(response.data);
                setData(response.data);
            })
            .catch((error) => {
                console.log(error)
            });

        getReviews(offset);


    }, [])


    function getReviews(offset) {
        axios.get(process.env.REACT_APP_API_URL + "/api/Executor/executor_reviews/" + id + "/" + offset + "/10",
            {
                headers,
            })
            .then((response) => {
                console.log(response.data);
                setReviews([...reviews, ...response.data]);
                setCount(response.data.length);
                setOffset(offset++);
            })
            .catch((error) => {
                console.log(error)
            });
    }

    return (
        <>
            {/*<Header />*/}
            <div className="main">
                {data !== undefined ?
                    <>
                        <div className="user-personal">
                            <div className="user-card">
                                <div className="user-avatar">
                                    <img className="avatar" src={emptyUserIcon} alt="Аватар пользователя"/>
                                </div>

                                <div className="user-data">
                                    <p className="user-username">{data.surname + " " + data.name + " " + data.secondName}</p>
                                    <div className="user-data row-1">
                                        <p>{"Возраст: " + _calculateAge(new Date(data['executor']['birthDate']))}</p>
                                        <p>Город: {data['executor']['city']}</p>
                                    </div>
                                    <div className="user-data row-2">
                                        <div className="user-data column-1">
                                            {'email' in data && 'phone' in data ?
                                                <>
                                                    <p>E-mail: <a href={"mailto:" + data['email']}>{data['email']}</a>
                                                    </p>
                                                    <p>Телефон: <a href="tel:+79143439889">{data['phone']}</a></p>
                                                </> :
                                                <p>Проектов создано: 0</p>
                                            }
                                        </div>
                                    </div>
                                    <div className="last-row">
                                        <div className="social-card">
                                            <h2>Мои ссылки:</h2>
                                            {data.contacts === null ||  data.isClose ?
                                                <div className="no-data">
                                                    <p>Пока вы не можете просматривать данные пользователя.</p>
                                                    <p>Ссылки будут доступны после принятия исполнителя в заказ.</p>
                                                </div> :
                                                <div className="links">

                                                    {data.contacts.contacts.map(contact => {
                                                        return (<div className={"fields-tags"}>
                                                            <a className={'field-tag'} id={contact.name}
                                                               title={contact.name}
                                                               href={contact.url}>{contact.name}</a></div>)


                                                    })}
                                                    {/*{data.contacts.githubUrl !== null && data.contacts.githubUrl !== undefined &&*/}
                                                    {/*    <a target="_blank"*/}
                                                    {/*       title={data.contacts.githubUrl.includes("github.com/") ? data.contacts.githubUrl : "https://github.com/" + data.contacts.githubUrl}*/}
                                                    {/*       href={data.contacts.githubUrl.includes("github.com/") ? data.contacts.githubUrl : "https://github.com/" + data.contacts.githubUrl}><img*/}
                                                    {/*        className="social-logo" src={gitLogo} alt="Логотип Github"/></a>*/}
                                                    {/*}*/}
                                                    {/*{data.contacts.vkNickname !== null && data.contacts.vkNickname !== undefined &&*/}
                                                    {/*    <a target="_blank"*/}
                                                    {/*       title={data.contacts.vkNickname.includes("vk.com") ? data.contacts.githubUrl : "https://vk.com/" + data.contacts.vkNickname}*/}
                                                    {/*       href={data.contacts.vkNickname.includes("vk.com/") ? data.contacts.githubUrl : "https://vk.com/" + data.contacts.vkNickname}><img*/}
                                                    {/*        className="social-logo" src={vk} alt="Логотип VK"/></a>*/}
                                                    {/*}*/}
                                                    {/*{data.contacts.messager !== null && data.contacts.messager !== undefined &&*/}
                                                    {/*    <a target="_blank"*/}
                                                    {/*       title={data.contacts.messager.includes("t.me") ? data.contacts.messager : "https://t.me/" + data.contacts.messager}*/}
                                                    {/*       href={data.contacts.messager.includes("t.me/") ? data.contacts.messager : "https://vk.com/" + data.contacts.messager}><img*/}
                                                    {/*        className="social-logo" src={vk} alt="Логотип VK"/></a>*/}
                                                    {/*}*/}
                                                </div>
                                            }
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        {'about' in data &&
                            <section className="about-user-person">
                                <h2>О себе:</h2>
                                <p className="about-person-text">{data.about === "" || data.about === null ? "Пользователь решил не рассказывать о себе" : data.about}</p>
                            </section>
                        }

                        <CompetenceBlock competencies={data.executor.categories} skills={data.executor.skills}/>
                        {data.executor.educations !== null &&
                            < Educations educations={data.executor.educations}/>
                        }
                        {reviews.length !== 0 &&
                            <section className="reviews">
                                <h2 className="center">Отзывы о работе исполнителя:</h2>
                                <div className="reviews-cards">
                                    {reviews.map((review) => {
                                        console.log(review);
                                        return <ReviewCard info={review} key={"review-" + review.id}/>
                                    })}
                                    {count > 10 &&
                                        <input className="more-cards-btn" type="button" value="Показать больше"/>}
                                </div>
                            </section>
                        }
                    </> :
                    <></>
                }
            </div>
            {/*<Footer />*/}
        </>
    )
}

export default ConstructorPage;