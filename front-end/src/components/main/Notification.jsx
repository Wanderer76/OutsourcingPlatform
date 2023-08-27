import avatar from "../../images/notification-avatar.svg";
import React from "react";
import {setDateOrTimeIfToday} from "../../scripts/time-fucntions";

const Notification = (props) => {

    return(
        <div className="notification-item new-notification-item" onClick={props.onClick}>
            <div className="notification-item-avatar">
                <img src={avatar}/>
            </div>
            <div className="notification-item__text-content">
                <div className="notification-item__first-row">
                    <p className="notification-item__sender">{props.notification.projectName}</p>
                    <p className="notification-item__time">{setDateOrTimeIfToday(new Date(props.notification.createdAt))}</p>
                </div>
                <p className="notification-company-name">{props.notification.companyName}</p>
                <p className="notification-message">{props.notification.message}</p>
            </div>
        </div>
    )
}

export default Notification