import React from "react";

const StatisticPanel = ({usersCount}) => {
    return(
        <>
            <h1>Панель администратора</h1>
            <div className="system-info">
                <h2>Сведения о системе:</h2>
                <div className="param-blocks">
                    <div className="param-blocks column">
                        <div className="param-item">Всего пользователей: {usersCount.executorCount+usersCount.customerCount}</div>
                        <div className="param-item">Заблокированных: {usersCount.bannedCount}</div>
                    </div>
                    <div className="param-blocks column">
                        <div className="param-item">Исполнителей: {usersCount.executorCount}</div>
                        <div className="param-item">Заказчиков: {usersCount.customerCount}</div>
                    </div>
                </div>
            </div>
        </>
    )
}

export default StatisticPanel;