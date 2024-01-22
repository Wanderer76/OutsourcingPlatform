import avatar from "../../images/notification-avatar.svg"
import {useEffect, useRef, useState} from "react";
import {HttpTransportType, HubConnectionBuilder, HubConnectionState, LogLevel} from "@microsoft/signalr";
import chat from "./chat";
import Chat from "./chat";
import RightBlock from "./right-block";
import {mergeArrays} from "./sort-messages";

const Messanger = () => {
    const refreshToken = 'Bearer ' + window.sessionStorage.getItem('token');
    const [connection, setConnection] = useState(null);
    const [yourChats, setYourChats] = useState([])
    const [newChats, setNewChats] = useState([]);
    const [messages, setMessages] = useState([]);
    const [activeChat, setActiveChat] = useState(-1);
    const [currentChatId, setCurrentChatId] = useState(-1);
    let lastMessageCount = -1;
    //let [nextOffset, setNextOffset] = useState(0);
    let nextOffset = 0;
    const limit = 10;


    useEffect(() => {
        if (connection)
            return;
        const connection_chat = new HubConnectionBuilder()
            .withUrl(process.env.REACT_APP_API_URL + "/chats?token=" + 'Bearer ' + window.sessionStorage.getItem('token'), {
                skipNegotiation: true,
                transport: HttpTransportType.WebSockets
            })

            .configureLogging(LogLevel.Debug)
            .withAutomaticReconnect()
            .build();

        // // обработчик метода все чаты
        // connection_chat.on("ReceiveChatRooms", function (chats) {
        //     console.log(JSON.stringify(chats))
        // });

// обработчик метода не начатые чаты
        connection_chat.on("ReceiveNotStartedChatRooms", function (chats) {
            let newChats = chats;
            console.log(newChats);
            setNewChats((prevChats) => [...prevChats, ...newChats]);

        });

// обработчик метода начатые чаты
        connection_chat.on("ReceiveStartedChatRooms", function (chats) {
            let newChats = chats;
            console.log(newChats);
            setYourChats((prevChats) => [...prevChats, ...newChats]);
        });

// обработчик метода, который возвращает чат и сообщения
//         connection_chat.on("sendchatroom", function (chat, messages) {
//             console.log(JSON.stringify(chat))
//             console.log(JSON.stringify(messages))
//         });

// обработчик метода, который возвращает сообщения
        connection_chat.on("sendmessages", function (newMessages) {
            console.log(newMessages)
            // if (newMessages.length === 1){
            //     let message = newMessages[0]
            //     setMessages((prevMessages) => [message, ...(prevMessages.filter(prevMessage => prevMessage.id !== message.id))]);
            // }
            //setNextOffset(prevState => {return prevState+newMessages.length})
            if (activeChat !== -1) {
                newMessages.forEach(async m=>await markViewed(m.id));
            }
            setMessages((prevMessages) => [...(newMessages.filter((message) => {
                for (let prevMessage of prevMessages) {
                    if (prevMessage.id === message.id)
                        return false
                }
                return true;

            })), ...prevMessages])
        });

        // обработчик метода, который возвращает отправленное сообщение
        connection_chat.on("sendasync", async function (message) {
            console.log(message);
            console.log(currentChatId);
            if (activeChat !== -1) {
                await markViewed(message.id)
            }
            setMessages((prevMessages) => {
                if (prevMessages.length !== 0 && prevMessages[0].chatId === message.chatId)
                    return [...prevMessages, message]
                else if (prevMessages.length === 0)
                    return [message];
                else return prevMessages;
            });

            if (message.senderUsername !== window.sessionStorage.getItem('username'))
                setYourChats((chats) =>
                    chats.map((chat) =>
                        chat.id === message.chatId
                            ? {...chat, unreadCount: chat.unreadCount + 1}
                            : chat
                    )
                )

        });

        connection_chat.on("markmessage", function (message) {
            //console.log(message);
            //console.log(currentChatId)
            if (message.isViewed) {
                setMessages((prevMessages) =>
                    prevMessages.map((prevMessage) =>
                        prevMessage.id === message.id
                            ? {...prevMessage, isViewed: message.isViewed}
                            : prevMessage
                    )
                )

                setYourChats((chats) =>
                    chats.map((chat) =>
                        chat.id === message.chatId
                            ? {...chat, unreadCount: chat.unreadCount - 1}
                            : chat
                    )
                )
            }

        });

        setConnection(connection_chat);

    }, [])

    useEffect(() => {

        const startConnection = async () => {
            // console.assert(connection.state === HubConnectionState.Connected);
            await connection.start();

        }

        const startBaseInvokes = () => {
            connection.invoke("ReceiveNotStartedChats", refreshToken, 0, 10);
            connection.invoke("ReceiveStartedChats", refreshToken, 0, 10)
        }

        if (connection)
            startConnection()
                .then(() => {
                    console.log("SignalR Connected.")
                    startBaseInvokes();
                })
                .catch(() => console.assert(connection.state === HubConnectionState.Disconnected))

    }, [connection])

    function getChatMessages(chatId, limit = 10) {
        console.log(chatId)
        console.log("Send request to get preview message with offset: " + nextOffset + " and limit " + limit)
        setMessages([]);
        setCurrentChatId(() => chatId);
        setActiveChat(chatId);
        let offset = nextOffset
        nextOffset += limit
        //setNextOffset(nextOffset + limit)
        connection.invoke("ReceiveMessages", refreshToken, chatId, offset, limit)
    }

    function setAllMessagesViewed(chatId) {
        connection.invoke("SetAllUserCharMessagesViewed", chatId, refreshToken);
    }

    function getMoreMessage(limit = 10) {
        console.log("Send request to get preview message with offset: " + nextOffset + " and limit " + limit)
        let offset = nextOffset;
        nextOffset += limit
        //setNextOffset(nextOffset + limit)
        connection.invoke("ReceiveMessages", refreshToken, currentChatId, offset, limit)
    }

    async function markViewed(messageId) {
        await connection.invoke("SetMessageViewed", messageId, refreshToken)

    }

    function sendMessage(message) {
        console.log(message);
        message = message.trim()
        if (message !== null && message !== "")
            connection.invoke("Send", message, refreshToken, currentChatId);
    }


    return (
        <section className="messenger">
            <div className="messenger-window">
                <div className="messenger-left-block">
                    <div className="search-chat">
                        <form id="search-chat">
                            <input type="text" placeholder="Поиск"/>
                            <button type="submit" className="visually-hidden"></button>
                        </form>
                    </div>
                    <div className="chats-overflow">
                        {/*<div className="chat-list-results">*/}
                        {/*    <p className="chat-list-header">Результаты поиска:</p>*/}
                        {/*    <div className="chats-results">*/}
                        {/*        <div className="chat-item chat-item-new">*/}
                        {/*            <div className="chat-avatar">*/}
                        {/*                <img src={avatar}/>*/}
                        {/*            </div>*/}
                        {/*            <div className="chat-item-text-content">*/}
                        {/*                <div className="chat-item-first-row">*/}
                        {/*                    <div className="chat-item-name">*/}
                        {/*                        <p className="chat-sender-name">Мария Вишневская</p>*/}
                        {/*                        <div className="message-counter">22</div>*/}
                        {/*                    </div>*/}
                        {/*                    <p className="chat-item-time">14:22</p>*/}
                        {/*                </div>*/}
                        {/*                <p className="chat-item-company-name">Маринс-хоум Отель</p>*/}
                        {/*                <p className="chat-item-message">У меня есть некоторые правки к версии, которую*/}
                        {/*                    вы прислали мне вчера.</p>*/}
                        {/*            </div>*/}
                        {/*        </div>*/}
                        {/*        <div className="chat-item">*/}
                        {/*            <div className="chat-avatar">*/}
                        {/*                <img src={avatar}/>*/}
                        {/*            </div>*/}
                        {/*            <div className="chat-item-text-content">*/}
                        {/*                <div className="chat-item-first-row">*/}
                        {/*                    <div className="chat-item-name">*/}
                        {/*                        <p className="chat-sender-name">Марина Логинова</p>*/}
                        {/*                    </div>*/}
                        {/*                </div>*/}
                        {/*                <p className="chat-item-company-name">Студия №21</p>*/}
                        {/*            </div>*/}
                        {/*        </div>*/}
                        {/*    </div>*/}
                        {/*</div>*/}
                        {yourChats.length > 0 &&
                            <div className="chat-list-yours">
                                <p className="chat-list-header">Ваши чаты:</p>
                                <div className="chats-yours">
                                    {yourChats.map((chat) => {
                                        console.log(chat)
                                        return <Chat chatInfo={chat} onClick={() => {
                                            setActiveChat(chat.id)
                                            setAllMessagesViewed(chat.id)
                                            getChatMessages(chat.id)
                                        }
                                        }
                                                     key={chat.id}/>
                                    })}
                                </div>
                            </div>
                        }

                        {newChats.length > 0 &&
                            <div className="chat-list-begin">
                                <p className="chat-list-header">Начать общение:</p>
                                <div className="chats-begin">
                                    {newChats.map((chat) => {
                                        return <Chat chatInfo={chat} onClick={() => {
                                            setActiveChat(chat.id)
                                            getChatMessages(chat.id)
                                        }}
                                                     key={chat.id}/>
                                    })}
                                </div>
                            </div>
                        }
                    </div>
                </div>

                {currentChatId === -1 ?
                    <div className="messenger-right-block">
                        <p className="no-chat">Выберите чат <br/>или начните новый</p>
                    </div> :
                    <RightBlock messages={messages} onMouseEnter={event => {
                        markViewed(event)
                        setMessages(messages)
                    }} onSubmit={sendMessage} getMoreMessage={getMoreMessage}/>
                }
            </div>
            <a className="close-messenger" href="#" onClick={(e) => {
                setMessages([]);
                //setNextOffset(0);
                nextOffset = 0;
                setCurrentChatId(-1);
                setActiveChat(-1)
                e.preventDefault();
            }}></a>
        </section>
    )
}


export default Messanger;