import emptyUser from "../../images/empty-user.svg";
import React from "react";

const UserLi = ({id, surname, name, secondName, userRole, isBanned, onClick}) => {
    return(
        <li className="result-item">
            <div className="item-avatar">
                <img width="45" height="45" src={emptyUser}
                     alt="Аватар пользователя"/>
            </div>
            <div className="user-data">
                <div className="user-data-item">id: {id}</div>
                <div className="user-data-item">{surname + " " + name + " " + secondName}</div>
                <div className="user-data-item">{userRole === "executor_role" ? "Исполнитель" : userRole === "customer_role" ? "Заказчик" : "WTP"}</div>
                <div className="user-data-item">{isBanned ? "Заблокированный" : "Активный"}</div>
            </div>
            <input type="button" className="check-user" data-id="7888940" value="Открыть" onClick={onClick}/>
        </li>
    )
}

export default UserLi;