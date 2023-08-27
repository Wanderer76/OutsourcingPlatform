import Chat from "../messanger/chat";
import RightBlock from "../messanger/right-block";
import {useEffect, useState} from "react";
import {HttpTransportType, HubConnectionBuilder, HubConnectionState, LogLevel} from "@microsoft/signalr";

const UserChatsForAdmin = ({userId, hidePannel}) => {
    const refreshToken = window.sessionStorage.getItem('refresh-token');
    const [connection, setConnection] = useState(null);
    const [yourChats, setYourChats] = useState([])
    const [newChats, setNewChats] = useState([]);
    const [messages, setMessages] = useState([]);
    const [currentChatId, setCurrentChatId] = useState(-1);
    let lastMessageCount = -1;
    //let [nextOffset, setNextOffset] = useState(0);
    let nextOffset = 0;
    const limit = 10;

    useEffect(() => {
        if (connection)
            return;
        const connection_chat = new HubConnectionBuilder()
            .withUrl(process.env.REACT_APP_API_URL + "/chats?token=" + refreshToken, {
                skipNegotiation: true,
                transport: HttpTransportType.WebSockets
            })
            .configureLogging(LogLevel.Debug)
            .withAutomaticReconnect()
            .build();


        // обработчик метода начатые чаты
        connection_chat.on("ReceiveUserChatRooms", function (chats) {
            console.log(chats)
            setYourChats((prevChats) => [...prevChats, ...chats]);
        })
        connection_chat.on("ReceiveUserStartedChatRooms", function (chats) {
            console.log(chats)
        })

        connection_chat.on("ReceiveUserNotStartedChatRooms", function (chats) {
            console.log(chats);
        })

        connection_chat.on("sendmessagenotfound", function (chats) {
            console.log(chats);
        })





        connection_chat.on("SendUserMessages", function (newMessages) {
            setMessages((prevMessages) => [...(newMessages.filter((message) => {
                for (let prevMessage of prevMessages) {
                    if (prevMessage.id === message.id)
                        return false
                }
                return true;

            })), ...prevMessages])

        })
        // обработчик метода, который возвращает сообщения
        connection_chat.on("sendmessages", function (newMessages) {
            console.log(newMessages)
            // if (newMessages.length === 1){
            //     let message = newMessages[0]
            //     setMessages((prevMessages) => [message, ...(prevMessages.filter(prevMessage => prevMessage.id !== message.id))]);
            // }
            //setNextOffset(prevState => {return prevState+newMessages.length})
            setMessages((prevMessages) => [...(newMessages.filter((message) => {
                for (let prevMessage of prevMessages) {
                    if (prevMessage.id === message.id)
                        return false
                }
                return true;

            })), ...prevMessages])
        });
        setConnection(connection_chat);
    }, []);

    useEffect(() => {

        const startConnection = async () => {
            // console.assert(connection.state === HubConnectionState.Connected);
            await connection.start();

        }

        const startBaseInvokes = () => {
            // connection.invoke("ReceiveNotStartedChats", refreshToken, 0, 10);
            // connection.invoke("ReceiveStartedChats", refreshToken, 0, 10)
            connection.invoke("ReceiveAllUserChatRooms", refreshToken, userId, 0, 10)
            // connection.invoke("GetUserStartedChats", refreshToken, userId, 0, 10)
            // connection.invoke("GetUserNotStartedChats", refreshToken, userId, 0, 10)
            connection.invoke("AddAdminToUserChats", userId)


            console.log("start getting user " + userId + " chats");
        }

        if (connection)
            startConnection()
                .then(() => {
                    console.log("SignalR Connected.")
                    startBaseInvokes();
                })
                .catch(()=> console.assert(connection.state === HubConnectionState.Disconnected))

    }, [connection, userId])

    useEffect(() => {
        return function cleanup() {
            console.log("Chats destroyed");
            if (connection)
                connection.invoke("RemoveAdminFromUserChats", userId)
        }
    }, [])

    function getChatMessages(chatId, limit = 10) {
        console.log(chatId)
        console.log("Send request to get preview message with offset: " + nextOffset + " and limit " + limit )
        setMessages([]);
        setCurrentChatId(() => chatId);
        let offset = nextOffset
        nextOffset += limit
        //setNextOffset(nextOffset + limit)
        connection.invoke("ReceiveMessages", refreshToken, chatId, offset, limit)
    }

    function getMoreMessage(limit=10) {
        console.log("Send request to get preview message with offset: " + nextOffset + " and limit " + limit)
        let offset = nextOffset;
        nextOffset += limit
        //setNextOffset(nextOffset + limit)
        // connection.invoke("ReceiveMessages", refreshToken, currentChatId, offset, limit)
        connection.invoke("ReceiveUserMessages", refreshToken, currentChatId, offset, limit)
    }

    return(
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
                        {yourChats.length > 0 &&
                            <div className="chat-list-yours">
                                <p className="chat-list-header">Ваши чаты:</p>
                                <div className="chats-yours">
                                    {yourChats.map((chat) => {
                                        return <Chat chatInfo={chat} onClick={() => getChatMessages(chat.id)}
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
                                        return <Chat chatInfo={chat} onClick={() => getChatMessages(chat.id)}
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
                    <RightBlock messages={messages} onMouseEnter={()=> {}} onSubmit={() => {}} getMoreMessage={getMoreMessage}/>
                }
            </div>
            <a className="close-messenger" href="#" onClick={(e) => {
                setMessages([]);
                //setNextOffset(0);
                nextOffset = 0;
                setCurrentChatId(-1);
                connection.invoke("RemoveAdminFromUserChats", userId)
                hidePannel();
                e.preventDefault();
            }}></a>
        </section>
    )
}

export default UserChatsForAdmin