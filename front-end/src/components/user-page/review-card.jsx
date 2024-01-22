import deadLineIcon from "../../images/deadline-icon.svg"
import customerIcon from "../../images/customer-icon.svg"
const ReviewCard = (props) => {
    return(
        <div className="review-card">
            <div className="project-params">
                <div className="row">
                    <div className="customer-avatar">
                        <img className="customer-avatar" src={customerIcon}
                             alt="Аватар заказчика"/>
                    </div>
                    <p className="customer-name">{props.info.companyName}</p>
                </div>
                <div className="row">
                    <img className="project-card-icon" src={deadLineIcon}
                         alt="Иконка чек-листа"/>
                    <p className="status">Закончен: <span>{new Date(props.info.projectCompleted).toLocaleString("ru", {
                        year: 'numeric',
                        month: 'long',
                        day: 'numeric'
                    })}</span></p>
                </div>
            </div>

            <h3>{props.info.projectnName}</h3>
            <div className="project-fields">
                <h4>Сферы деятельности:</h4>
                <div className="user-fields fields-tags">
                    {props.info.competitences !== undefined && props.info.competitences.map((theme) => {
                        return <div className="field-tag" key={theme + "-" + props.info.orderId}>{theme}</div>
                    })}
                </div>
            </div>
            <h3 className="customer-name">Отзыв:</h3>
            <p className="review-text">{props.info.description}</p>


            <input className="card-button project-button" type="button" value="Поcмотреть проект"
                   onClick={() => window.location.href = "/tasks/" + props.info.orderId}/>
        </div>
    )
}

export default ReviewCard;