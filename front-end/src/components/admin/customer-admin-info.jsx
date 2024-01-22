import emptyUser from "../../images/empty-user.svg";
import React from "react";
const CustomerAdminInfo = ({info, onClick, viewChatsClick}) => {
    return(
        <div className="search-user-card">
            <a className="close-user-card" href="#"></a>
            <h2>Просмотр пользователя c ID: {info.contacts !== undefined ? info.contacts.id : "Данные загружаются"}</h2>
            <div className="search-user-content">
                <div className="user-left-block">
                    <div className="user-avatar">
                        <img src={emptyUser} alt="Аватар пользователя"/>
                    </div>
                    <input type="button" data-id="25667373" className="user-control-btn" id="unblock-user"
                           value={info.isBanned ? "Разблокировать" : "Заблокировать"} onClick={onClick}/>

                    <input type="button" data-id="25667373" className="user-control-btn" id="visit-user"
                           value="Смотреть профиль" onClick={() => {
                        window.location.href = "/customers/" + info.customerId
                    }}/>
                    <input type="button" data-id="25667373" className="user-control-btn" id="open-user-chats"
                           value="Смотреть чаты" onClick={viewChatsClick}/>
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
                            <input className="auth_input" type="text" name="status" id="status" value={info.isBanned ? "Заблокирован" : "Активный"}
                                   readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="status">Статус</label>
                        </div>

                        <div className="auth__field">
                            <p>Количество заказов</p>
                            <input className="auth_input" type="text" name="order-count" id="order-count" value={info.orderCount}
                                   readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="order-count">Количество
                                заказов</label>
                        </div>

                        <div className="auth__field">
                            <p>Количество закрытых заказов</p>
                            <input className="auth_input" type="text" name="order-finished-count"
                                   id="order-finished-count" value={info.closedOrderCount} readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="order-finished-count">Количество
                                закрытых заказов</label>
                        </div>

                        <div className="auth__field">
                            <p>Комментарий администратора</p>
                            <textarea className="admin-comment-textarea" name="admin-comment" id="admin-comment" value={info.bannedMessage}
                                      readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="admin-comment">Количество
                                закрытых заказов</label>
                        </div>
                    </div>
                    <div className="user-right-block-column">
                        <div className="auth__field">
                            <p>Фамилия</p>
                            <input className="auth_input" type="text" placeholder="Фамилия" name="surname" id="surname"
                                   value={info.surname} readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="surname">Фамилия</label>
                            <p className="input-error__text visually-hidden" id="surname-error">Error text (Текст
                                ошибки)!</p>
                        </div>

                        <div className="auth__field">
                            <p>Имя</p>
                            <input className="auth_input" type="text" placeholder="Имя" name="name" id="name"
                                   value={info.name} readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="name">Имя</label>
                            <p className="input-error__text visually-hidden" id="name-error">Error text (Текст
                                ошибки)!</p>
                        </div>

                        {info.secondName !== "" && info.secondName !== null &&
                        <div className="auth__field">
                            <p>Отчество</p>
                            <input className="auth_input" type="text" placeholder="Отчество" name="second-name"
                                   id="second-name" value={info.secondName} readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="second-name">Отчество</label>
                            <p className="input-error__text visually-hidden" id="second-name-error">Error text (Текст
                                ошибки)!</p>
                        </div>}

                        <div className="auth__field field-check-box">
                            <input className="auth__field sign-in__check-box" type="checkbox" name="no-second-name"
                                   id="no-second-name" disabled="disabled" checked={info.secondName === "" || info.secondName===null}/>
                            <label className="sign-in__label" htmlFor="no-second-name">Нет отчества</label>
                        </div>

                        <div className="auth__field">
                            <p>Название организации</p>
                            <input className="auth_input" type="text" placeholder="Название организации"
                                   name="company-name" id="company-name" value={info.companyName} readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="company-name">Название
                                организации</label>
                            <p className="input-error__text visually-hidden" id="company-name-error">Error text (Текст
                                ошибки)!</p>
                        </div>

                        <div className="auth__field">
                            <p>Адрес</p>
                            <input className="auth_input" type="text" placeholder="Адрес" name="address" id="address"
                                   value={info.address} readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="address">Адрес</label>
                            <p className="input-error__text visually-hidden" id="address-error">Error text (Текст
                                ошибки)!</p>
                        </div>

                        <div className="auth__field">
                            <p>ИНН</p>
                            <input className="auth_input" type="text" placeholder="XXX-XXX-XXXX" name="inn" id="inn"
                                   value={info.inn} readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="inn">Индивидуальный налоговый
                                номер</label>
                            <p className="input-error__text visually-hidden" id="inn-error">Error text (Текст
                                ошибки)!</p>
                        </div>

                        {/*<div className="auth__field field-check-box">*/}
                        {/*    <input className="auth__field sign-in__check-box" type="checkbox" name="no-second-name"*/}
                        {/*           id="own-business-check" disabled="disabled"/>*/}
                        {/*    <label className="sign-in__label" htmlFor="own-business-check">У меня ИП</label>*/}
                        {/*</div>*/}
                    </div>
                    <div className="user-right-block-column">
                        <div className="auth__field">
                            <p>Телефон</p>
                            <input className="auth_input" type="tel" placeholder="+7-XXX-XXX-XX-XX" name="phone"
                                   id="phone" value={info.phone} readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="phone">Телефон</label>
                            <p className="input-error__text visually-hidden" id="phone-error">Error text (Текст
                                ошибки)!</p>
                        </div>

                        <div className="auth__field">
                            <p>Username</p>
                            <input className="auth_input" type="text" placeholder="@username" name="username"
                                   id="username" value={info.userName} readOnly/>
                            <label className="sign-in__label visually-hidden" htmlFor="username">Username</label>
                            <p className="input-error__text visually-hidden" id="username-error">Error text (Текст
                                ошибки)!</p>
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

export default CustomerAdminInfo