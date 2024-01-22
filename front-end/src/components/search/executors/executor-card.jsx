import avatarIcon from "../../../images/empty-user.svg"


const ExecutorCard = (props) => {
    return (
        <div className="project-new-contractors contractor-card card card-search">
            <div className="new-contractor contractor-data">
                <div className="new-contractor user-avatar-little">
                    <img className="new-contractor avatar-image" src={avatarIcon}
                         alt="Аватар пользователя"/>
                </div>
                <div className="new-contractor contractor-info">
                    <p className="new-contractor contractor-username">{props.info.surname + ' ' + props.info.name + ' ' + props.info.secondName}</p>
                    <div className="contractor-additional-info">
                        <p className="new-contractor project-finished">Проектов
                            закончено:<span>{props.info.completedOrders}</span></p>
                        <p className="new-contractor city">Город: <span>Екатеринбург</span></p>
                    </div>
                </div>
            </div>

            <div className="new-contractor contractor-fields">
                <h3>Сферы деятельности:</h3>
                <div className="new-contractor tags-fields">
                    {props.info.executorCategories.categories.map((cat) => {
                        return <div className="field-card-tag"
                                    key={"category-" + props.info.executorId + "-" + cat.id}>{cat.name}</div>
                    })}
                </div>
            </div>

            <div className="new-contractor contractor-skills">
                <h3>Компетенции:</h3>
                <div className="new-contractor tags-skills">
                    {props.info.executorSkills.skills.map((skill) => {
                        return <div className="field-card-tag"
                                    key={"skill-" + props.info.executorId + "-" + skill.id}>{skill.name}</div>
                    })}

                </div>
            </div>
            <div className="searched-contractor-card buttons">
                <a className="project-card-btn" href={"/executors/" + props.info.executorId}>Перейти к профилю</a>
            </div>

        </div>
    )
}

export default ExecutorCard;