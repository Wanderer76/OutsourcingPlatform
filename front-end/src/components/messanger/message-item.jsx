import avatar from "../../images/notification-avatar.svg";
import {getTimeFromDate} from "../../scripts/time-fucntions";

const MessageItem = ({info, onMouseEnter}) => {

    return(
        <div className={info.isViewed ? "message-item" : "message-item message-item-new"} onMouseEnter={!info.isViewed ? onMouseEnter : ()=>{}}>
            <div className="chat-avatar">
                <img src={avatar}/>
            </div>
            <div className="message-item-text-block">
                <div className="message-item-first-row">
                    <p className="message-item-sender-name">{info.senderName +" "+ info.senderSurname}</p>
                    <p className="message-item-time">{getTimeFromDate(info.createdAt)}</p>
                </div>
                {/*<div className="message-item-company">Маринс-хоум Отель</div>*/}
                <div className="message-item__message-text">{info.messageText}</div>
            </div>
        </div>
    )
}

export default MessageItem;