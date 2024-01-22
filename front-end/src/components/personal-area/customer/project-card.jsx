import deadlineIcon from "../../../images/deadline-icon.svg";
import checkListIcon from "../../../images/checklist-icon.svg";
import custAvaDef from "../../../images/customer-icon.svg";
import notificationIcon from "../../../images/notification-icon.svg";
import {formatTagValue} from "../../tasks/format-tag";

const ProjectCard = (props) => {
    let project = props.projectInfo;
    console.log(project);
    return(
        <div className={!project.isCompleted ? "project-card project-card-blue" : "project-card"}>
            <div className="column-1">
                <h3>{project.name}</h3>
                <p className="project-description">{project.description}</p>
                <div className="project-fields">
                    <h4>Сферы деятельности:</h4>
                    <div className="user-fields fields-tags">
                        {project.orderCategories.categories.map((tag) =>
                            <div className="field-tag" key={"tag-"+tag.id}>{formatTagValue(tag.name)}</div>
                        )}
                    </div>
                </div>
            </div>
            <div className="column-2">
                <div className="project-params">
                    <div className="row">
                        <img className="project-card-icon" src={deadlineIcon}
                             alt="Иконка дедлайна"/>
                        <p className="deadline">Дедлайн: <span>{new Date(project.deadline).toLocaleString("ru", {year: 'numeric', month: 'long', day: 'numeric'})}</span></p>
                    </div>
                    <div className="row">
                        <img className="project-card-icon" src={checkListIcon}
                             alt="Иконка чек-листа"/>
                        <p className="status">Статус: <span>{project.isCompleted ? "Завершен" : "В процессе"}</span></p>
                    </div>
                    <div className="row">
                        <img className="project-card-icon" src={custAvaDef}
                             alt="Иконка пользователя"/>
                        <p className="participants">Участники: <span>{project.workersCount === -1 ? "∞" : project.workersCount}</span></p>
                    </div>
                    <div className="row">
                        <img className="project-card-icon" src={notificationIcon}
                             alt="Иконка уведомления"/>
                        <p className="notification">Новые уведомления: <span>{project.responseCount}</span></p>
                    </div>
                </div>
                <input className="card-button project-button" type="button" onClick={() => {window.location.href = "/tasks/my/"+project.orderId}} value="Подробнее"/>
            </div>
        </div>
    )
}

export default ProjectCard