import React, {useEffect, useState} from "react";
import Logo from "../../images/header-logo.svg"
import LogoBlack from "../../images/logo-black.svg"
import messageIcon from "../../images/notification-header.svg"
import notificationIcon from "../../images/messenger-header.svg"
import {HttpTransportType, HubConnectionBuilder, HubConnectionState, LogLevel} from "@microsoft/signalr";
import Notification from "./Notification";


const Header = () => {
    const refreshToken = 'Bearer ' + window.sessionStorage.getItem('token');
    let username = window.sessionStorage.getItem("username")
    const [popup, setPopup] = useState(<></>);
    const [connection, setConnection] = useState(null);

    const [notifications, setNotifications] = useState([]);
    const [chatNotificationCount, setChatNotificationCount] = useState(0)

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl(process.env.REACT_APP_API_URL + "/notifications?token=" + refreshToken, {
                skipNegotiation: true,
                transport: HttpTransportType.WebSockets
            })
            .configureLogging(LogLevel.Information)
            .withAutomaticReconnect()
            .build();


//обработчик привязан к ReceiveAllUserNotifications методу, получает все уведомления
        newConnection.on("receivenotifications", function (newNotifications, notificationsCount) {
            console.log(newNotifications)
            let count = 0;
            let newNotificationsFiltered = [];
            for (let newNot of newNotifications) {
                if (newNot.type === "Chat")
                    count++;
                else newNotificationsFiltered.push(newNot);
            }
            console.log(count);
            setNotifications((prevNot) => [...newNotificationsFiltered, ...prevNot])
            setChatNotificationCount(notificationsCount.chatNotificationCount);
        });

//метод для получения последних уведомлений
        newConnection.on("sendlatestnotification", function (newNotifications) {
            console.log(newNotifications);
            // if (newNotifications.type === "Chat") {
            //     setChatNotificationCount(chatNotificationCount+1);
            //     console.log(chatNotificationCount);
            // }
            // else setNotifications((prevNot) => [newNotifications, ...prevNot])
            if (newNotifications.type !== "Chat")
                setNotifications((prevNot) => [newNotifications, ...prevNot])
        });

        newConnection.on("sendnotificationcount", (count) => {
            setChatNotificationCount(count.chatNotificationCount);
        })

        setConnection(newConnection);
    }, [])

    useEffect(() => {
        const startConnection = async () => {
            // console.assert(connection.state === HubConnectionState.Connected);
            await connection.start();
            // console.log("SignalR Connected.");
            var result = await connection.invoke("ReceiveAllUserNotifications", refreshToken, 0, 10)
            // console.log(result);
        }
        if (connection)
            startConnection()
                .then(() => console.log("SignalR connected."))
                .catch(() => console.assert(connection.state === HubConnectionState.Connected));

    }, [connection]);

    function viewPopup() {
        setPopup(<div className="popup-wrapper">
            <div className="popup leave-account-popup">
                <img className="finish-project-popup-logo" src={LogoBlack}/>
                <p className="leave-text">Желаете выйти из личного кабинета?</p>
                <div className="popup-buttons buttons">
                    <a className="project-card-btn" href="/" onClick={() => window.sessionStorage.clear()}>Выйти</a>
                    <a className="project-card-btn" onClick={hidePopup}>Отмена</a>
                </div>
            </div>
        </div>)
    }

    function toogleNotificationsView() {
        document.getElementById('notifications-block').classList.toggle('visually-hidden');
    }

    function hidePopup() {
        setPopup(<></>)
    }


    return (
        <>
            {popup}
            <header className="main-header">
                <div className="logo">
                    <a href="/" className="logo-link header-logo">
                        <img className="header-logo" src={Logo} alt="Логотип платформы Первый Старт"/>
                    </a>
                    <div className="dropdown">
                        {username == null ?
                            <>
                                <a className="auth-link" href="/signin">
                                    <div className="nav-username">Вход</div>
                                </a>
                            </> :
                            <>
                                <a className="auth-link" href="/personal-area">
                                    <div className="nav-username">{username}</div>
                                </a>
                                <div className="dropdown-content">
                                    <a href="/personal-area">Личный кабинет</a>
                                    <a onClick={viewPopup}>Выход</a>
                                </div>
                            </>
                        }

                    </div>
                </div>

                <input className="side-menu" type="checkbox" id="side-menu"/>
                <label className="dash" htmlFor="side-menu"><span className="dash-line"></span></label>

                <nav className="nav">
                    <ul className="nav-menu">
                        <li className="nav-for-mobile"><a href={username !== null ? "/personal-area" : "/signin"}>Личный
                            кабинет</a></li>
                        {/*<li><a href="/404">О системе</a></li>*/}
                        <li><a href="/executors/search">Исполнители</a></li>
                        <li><a href="/tasks/search">Заказы</a></li>
                        {username !== null &&
                            <>
                                <li><a href="/personal-area">Личный кабинет</a></li>
                                <li className="nav-item-with-counter">
                                    <div className="header-icon">
                                        <img src={messageIcon}/>
                                        {chatNotificationCount !== 0 &&
                                            <div className="counter-tag">{chatNotificationCount}</div>}
                                    </div>
                                    <a href="/messenger">Сообщения</a>
                                </li>
                                <li className="nav-item-with-counter">
                                    <div className="header-icon">
                                        <img src={notificationIcon}/>
                                        {notifications.length !== 0 &&
                                            <div className="counter-tag">{notifications.length}</div>}
                                    </div>
                                    <a href="#" onClick={() => {
                                        toogleNotificationsView()
                                        console.log("Notifications: ");
                                        console.log(notifications);
                                    }}>Уведомления</a>
                                </li>
                            </>
                        }
                        {username !== null ?
                            <li className="nav-for-mobile"><a href="src/components/main/header#" onClick={(e) => {
                                viewPopup();
                                e.preventDefault();
                            }
                            }>Выход</a></li> :
                            <></>
                        }
                    </ul>

                    {/*Список уведомлений*/}
                    <div className="notification-block visually-hidden" id="notifications-block">
                        <div className="notification-list">
                            {notifications.map((notification) => {
                                return <Notification key={notification.id} notification={notification} onClick={() => {
                                    connection.invoke("SetNotificationViewed", notification.id);
                                        window.location.href = window.sessionStorage.getItem('role') === 'customer_role' || notification.receiverUsername == window.sessionStorage.getItem('username') ? '/tasks/my/' + notification.orderId : '/tasks/' + notification.orderId;
                                }}/>
                            })}
                        </div>
                        <a className="notification-hide" onClick={toogleNotificationsView}>Свернуть</a>
                    </div>
                </nav>
            </header>
        </>
    );
}

export default Header;
