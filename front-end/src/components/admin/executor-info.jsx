import emptyUser from "../../images/empty-user.svg";
import React from "react";

const ExecutorInfo = ({info, onClick, viewChatsClick}) => {
    return(
        <div className="search-user-card">
            <a className="close-user-card" href="#"></a>
            <h2>Просмотр пользователя c ID: {info.contacts.id}</h2>
            <div className="search-user-content">
                <div className="user-left-block">
                    <div className="user-avatar">
                        <img src={emptyUser} alt="Аватар пользователя"/>
                    </div>
                    {/*Первая кнопка меняется в зависимости от состояния*/}
                    <input type="button" data-id="25667373" className="user-control-btn" id="unblock-user"
                               value={info.isBanned ? "Разблокировать" : "Заблокировать"} onClick={onClick}/>
                    <input type="button" data-id="25667373" className="user-control-btn" id="visit-user"
                           value="Смотреть профиль" onClick={() => {
                        window.location.href = "/executors/" + info.contacts.id
                    }}/>
                    <input type="button" data-id="25667373" className="user-control-btn"
                           id="open-user-chats" value="Смотреть чаты" onClick={viewChatsClick}/>
                </div>
                <div className="user-right-block">
                    <div className="user-right-block-column">
                        <div className="auth__field">
                            <p>Роль</p>
                            <input className="auth_input" type="text" name="role" id="role" value="Исполнитель"
                                   readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="role">Роль</label>
                        </div>
                        <div className="auth__field">
                            <p>Статус</p>
                            <input className="auth_input" type="text" name="status" id="status"
                                   value={info.isBanned ? "Заблокирован" : !info.isBanned ? "Активный" : "Черт его знает" } readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="status">Статус</label>
                        </div>
                        <div className="auth__field">
                            <p>Количество заказов</p>
                            <input className="auth_input" type="text" name="order-count" id="order-count"
                                   value={info.projectsCount} readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="order-count">Количество
                                заказов</label>
                        </div>

                        <div className="auth__field">
                            <p>Количество закрытых заказов</p>
                            <input className="auth_input" type="text" name="order-finished-count"
                                   id="order-finished-count" value={info.finishProjectCount} readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="order-finished-count">Количество
                                закрытых заказов</label>
                        </div>
                        <div className="auth__field">
                            <p>Комментарий администратора</p>
                            <textarea className="admin-comment-textarea" name="admin-comment" id="admin-comment"
                                      readOnly value={info.bannedMessage}>
                                </textarea>
                            <label className="sign-in__label visually-hidden" htmlFor="admin-comment">Комментарий администратора</label>
                        </div>
                    </div>
                    <div className="user-right-block-column">
                        <div className="auth__field">
                            <p>Фамилия</p>
                            <input className="auth_input" type="text" placeholder="Фамилия" name="surname"
                                   id="surname" value={info.surname} readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="surname">Фамилия</label>
                            <p className="input-error__text visually-hidden" id="surname-error">Error text
                                (Текст ошибки)!</p>
                        </div>

                        <div className="auth__field">
                            <p>Имя</p>
                            <input className="auth_input" type="text" placeholder="Имя" name="name" id="name"
                                   value={info.name} readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="name">Имя</label>
                            <p className="input-error__text visually-hidden" id="name-error">Error text (Текст
                                ошибки)!</p>
                        </div>

                        <div className="auth__field">
                            <p>Отчество</p>
                            <input className="auth_input" type="text" placeholder="Отчество" name="second-name"
                                   id="second-name" value={info.secondName} readOnly/>
                            <label className="sign-in__label visually-hidden"
                                   htmlFor="second-name">Отчество</label>
                            <p className="input-error__text visually-hidden" id="second-name-error">Error text
                                (Текст ошибки)!</p>
                        </div>

                        <div className="auth__field field-check-box">
                            <input className="auth__field sign-in__check-box" type="checkbox"
                                   name="no-second-name" id="no-second-name" disabled="disabled"/>
                            <label className="sign-in__label" htmlFor="no-second-name">Нет отчества</label>
                        </div>

                        <div className="auth__field">
                            <p>Дата рождения</p>
                            <input className="auth_input" type="date" name="birth-date" id="birth-date"
                                   value={info.executor.birthdate} readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="birth-date">Дата
                                рождения</label>
                            <p className="input-error__text visually-hidden" id="birth-date-error">Error text
                                (Текст ошибки)!</p>
                        </div>

                        <div className="auth__field">
                            <p>Город</p>
                            <input className="auth_input" type="text" placeholder="Город" name="city" id="city"
                                   value={info.executor.city} readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="city">Город</label>
                            <p className="input-error__text visually-hidden" id="city-error">Error text (Текст
                                ошибки)!</p>
                        </div>

                        <div className="auth__field">
                            <p>Телефон</p>
                            <input className="auth_input" type="tel" placeholder="+7-XXX-XXX-XX-XX" name="phone"
                                   id="phone" value={info.phone} readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="phone">Телефон</label>
                            <p className="input-error__text visually-hidden" id="phone-error">Error text (Текст
                                ошибки)!</p>
                        </div>
                    </div>
                    <div className="user-right-block-column">
                        <div className="auth__field">
                            <p>Username</p>
                            <input className="auth_input" type="text" placeholder="@username" name="username"
                                   id="username" value={info.userName} readOnly/>
                            <label className="sign-in__label visually-hidden"
                                   htmlFor="username">Username</label>
                            <p className="input-error__text visually-hidden" id="username-error">Error text
                                (Текст ошибки)!</p>
                        </div>

                        <div className="auth__field">
                            <p>Email</p>
                            <input className="auth_input" type="email" placeholder="your@email.com" name="email"
                                   id="email" value={info.email} readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="email">Email</label>
                            <p className="input-error__text visually-hidden" id="email-error">Error text (Текст
                                ошибки)!</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    )
}

export default ExecutorInfo