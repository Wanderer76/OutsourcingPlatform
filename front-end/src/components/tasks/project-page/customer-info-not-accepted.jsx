import avatar from "../../../images/empty-user.svg"
import checkListIcon from "../../../images/checklist-icon.svg"
import deadLineIcon from "../../../images/deadline-icon.svg"

const CustomerInfoNotAccepted = (props) => {
    return(
        <section className="consumer-data-card card">
            <div className="consumer-data-card column-1">
                <p className="consumer-data-card company-name">{props.info.companyName}</p>
                <div className="consumer-data-card person-row">
                    <div className="consumer-data-card little-avatar">
                        <img className="consumer-data-card avatar-image" src={props.info.avatar !== "" && props.info.avatar !== undefined ? props.info.avatar : avatar}
                             alt="Автар пользователя"/>
                    </div>
                    <p className="consumer-data-card person-username">{props.info.nickname}</p>
                </div>
            </div>
            <div className="consumer-data-card column-2">
                <h3>Параметры:</h3>
                <div className="consumer-data-card project-params">
                    <div className="consumer-data-card one-param">
                        <img className="consumer-data-card param-icon" src={checkListIcon}
                             alt="param icon"/>
                            <p className="consumer-data-card param-text">Статус: <span>{new Date(props.info.deadline) - new Date() > 0 ? "В процессе" : "Завершен" }</span></p>
                    </div>
                    <div className="consumer-data-card one-param">
                        <img className="consumer-data-card param-icon" src={deadLineIcon}
                             alt="param icon"/>
                            <p className="consumer-data-card param-text">Дедлайн: <span>{new Date(props.info.deadline).toLocaleString("ru", {year: 'numeric', month: 'long', day: 'numeric'})}</span></p>
                    </div>
                </div>
            </div>
        </section>
    )
}

export default CustomerInfoNotAccepted;