import React from "react";
import avatarImage from "../../images/empty-user.svg"
import gitLogo from "./../../images/github-icon.svg"
import telegram from "./../../images/telegram-icon.svg"
import vk from "./../../images/vk-icon.svg"
import {_calculateAge} from "./calculate-age";

const UserPersonal = (props) => {
    let res = props.res;
    let avatar = res.avatar !== null && res.avatar !== "" && res.avatar !== undefined ? res.avatar : avatarImage;

    return (
        <>
            <div className="user-personal">
                <div className="user-card">
                    <div className="user-avatar">
                        <img className="avatar" src={avatar} alt="Аватар пользователя"/>
                    </div>

                    <div className="user-data">
                        <p className="user-name">{res['surname'] + " " + res['name'] + " " + res['secondName']}</p>
                        <div className="user-data row-1">
                            {'executor' in res ?
                                <>
                                    <p>{"Возраст: " + _calculateAge(new Date(res['executor']['birthDate']))}</p>
                                    <p>Город: {res['executor']['city']}</p>
                                </> :

                                <p>Организация: {res['customer']['companyName']}</p>
                            }
                        </div>
                        <div className="user-data row-2">
                            <div className="user-data column-1">
                                <p>E-mail: <a href={"mailto:" + res['email']}>{res['email']}</a></p>
                                <p>Телефон: <a href="tel:+79143439889">{res['phone']}</a></p>
                            </div>
                            {/*<input className="card-button user-button" type="button" value="Редактировать" onClick={() => {window.location.href="/personal-area/edit"}}/>*/}
                        </div>
                        <div className="last-row">
                            <div className="social-card">
                                <h2>Мои ссылки:</h2>

                                {res.contacts === null ?
                                    <div className="no-data">Нет данных <br/> Нажмите кнопку
                                        "Редактировать", <br/> чтобы добавить
                                        информацию о себе</div> :
                                    <div className="links">
                                        {res.contacts.contacts.map(x => {
                                            return (<div className={"fields-tags"}>
                                                <a className={'field-tag'} id={x.name}
                                                   title={x.name}
                                                   href={x.url}  target="_blank">{x.name}</a></div>)
                                        })}

                                    </div>
                                }
                            </div>
                            {(window.sessionStorage.getItem("role") === "customer_role" || window.sessionStorage.getItem("role") === "executor_role") &&
                                <input className="card-button user-button" type="button" value="Редактировать"
                                       onClick={() => {
                                           window.location.href = "/personal-area/edit"
                                       }}/>}
                        </div>
                    </div>

                </div>

            </div>
            {res.contacts !== null && res.contacts.about !== "" && res.contacts.about !== undefined && !res.isClose ?
                <section className="about-user-person">
                    <h2>О себе:</h2>
                    <p className="about-person-text">
                        {res.contacts.about}
                    </p>
                </section> : <></>}
        </>
    );
}

export default UserPersonal;