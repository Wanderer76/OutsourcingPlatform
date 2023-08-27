import deadlineIcon from "../../images/deadline-icon.svg";
import checkListIcon from "../../images/checklist-icon.svg";

const ProjectCardVisitors = ({info}) => {
    return (
        <div className={info.isCompleted ? "project-card green" : "project-card"}>
            <div className="column-1">
                <h3>{info.name}</h3>
                <p className="project-description">{info.description}</p>
                <div className="project-fields">
                    <h4>Сферы деятельности:</h4>
                    <div className="user-fields fields-tags">
                        {info.orderCategories.map((tag) => {
                            return <div key={tag.id} className="field-tag">{tag.name}</div>
                        })}
                    </div>
                </div>
            </div>
            <div className="column-2">
                <div className="project-params">
                    <div className="row">
                        <img className="project-card-icon" src={deadlineIcon}
                             alt="Иконка дедлайна"/>
                            <p className="deadline">Дедлайн: <span>{new Date(info.deadline).toLocaleString("ru", {year: 'numeric', month: 'long', day: 'numeric'})}</span></p>
                    </div>
                    <div className="row">
                        <img className="project-card-icon" src={checkListIcon}
                             alt="Иконка чек-листа"/>
                            <p className="status">Статус: <span>{info.isCompleted ? "Завершен" : "В процессе"}</span></p>
                    </div>
                </div>
                <input className="card-button project-button" type="button" value="Поcмотреть"/>
            </div>
        </div>
    )
}

export default ProjectCardVisitors