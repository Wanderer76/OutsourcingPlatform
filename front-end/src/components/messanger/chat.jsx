import avatar from "../../images/notification-avatar.svg";
import {getTimeFromDate} from "../../scripts/time-fucntions";

const Chat = ({chatInfo, onClick}) => {
    return (
        <div className={chatInfo.unreadCount > 0 ? "chat-item chat-item-new" : "chat-item"} onClick={onClick}>
            <div className="chat-avatar">
                <img src={avatar}/>
            </div>
            <div className="chat-item-text-content">
                <div className="chat-item-first-row">
                    <div className="chat-item-name">
                        <p className="chat-sender-name">{
                            window.sessionStorage.getItem('username') === chatInfo.senderUsername ?
                                chatInfo.senderName + " " + chatInfo.senderSurname :
                                chatInfo.receiverName + " " + chatInfo.receiverSurname}</p>
                        {chatInfo.unreadCount > 0 && <div className="message-counter">{chatInfo.unreadCount}</div>}
                    </div>
                    <p className="chat-item-time">{getTimeFromDate(chatInfo.createdAt)}</p>
                </div>
                {window.sessionStorage.getItem('role') === 'executor_role' &&
                    <p className="chat-item-company-name">{chatInfo.companyName}</p>}
                <p className="chat-item-message">{chatInfo.lastMessageText}</p>
            </div>
        </div>
    )
}

export default Chat;