import deadlineIcon from "../../../images/deadline-icon.svg"
import custAvaDef from "../../../images/customer-icon.svg"
import checkListIcon from "../../../images/checklist-icon.svg"
import {useEffect, useState} from "react";
import axios from "axios";
import {formatTagValue} from "../../tasks/format-tag";
const ProjectsConstructor = () => {
    const [projectsData, setProjectsData] = useState([]);
    const [offset, setOffset] = useState(0);
    const [lastCount, setLastCount] = useState(0);
    useEffect(() => {
        getProjectsData()
    }, [])

    function getProjectsData() {
        axios.get(process.env.REACT_APP_API_URL+"/api/Order/executor_orders/"+ offset + "/5",
            {
                headers:
                    {'Access-Control-Allow-Origin' : '*',
                        'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization' : "Bearer " + window.sessionStorage.getItem('token')
                    },

            })
            .then((response) => {
                fillProjectsData(response.data);
                setOffset(offset + 5);
                setLastCount(response.data.length);
                console.log(response.data);
            })
            .catch((error) => {console.log(error)});
    }

    function fillProjectsData(data) {
    console.log(data);
        let projects = data
            .sort((a,b) => {
                if (a.isCompleted && !b.isCompleted)
                    return 1;
                if (!a.isCompleted && b.isCompleted)
                    return -1;
                return new Date(a.deadline) - new Date(b.deadline)
            })
            .map((project) =>
            <div className={!project.isCompleted ? "project-card project-card-blue" : "project-card"} key={"order-"+project.orderId}>
                <div className="column-1">
                    <h3>{project.name}</h3>
                    <p className="project-description">{project.description}</p>
                    <div className="project-fields">
                        <h4>Сферы деятельности:</h4>
                        <div className="user-fields fields-tags">
                            {project.orderCategories.categories.map((tag) =>
                                <div className="field-tag" key={"themes-"+tag.id+"order-"+project.orderId}>{formatTagValue(tag.name)}</div>
                            )}
                        </div>
                    </div>
                </div>
                <div className="column-2">
                    <div className="project-params">
                        <div className="row">
                            <div className="customer-avatar">
                                <img className="customer-avatar" src={project.customerAva === null || project.customerAva === undefined ? custAvaDef : project.customerAva}
                                     alt="Аватар заказчика"/>
                            </div>
                            <p className="customer-name">{project.companyName}</p>
                        </div>
                        <div className="row">
                            <img className="project-card-icon" src={deadlineIcon}
                                 alt="Иконка дедлайна"/>
                            <p className="deadline">Дедлайн: <span>{new Date(project.deadline).toLocaleString("ru", {year: 'numeric', month: 'long', day: 'numeric'})}</span></p>
                        </div>
                        <div className="row">
                            <img className="project-card-icon" src={checkListIcon}
                                 alt="Иконка чек-листа"/>
                            <p className="status">Статус: <span>{!project.isCompleted ? "В процессе" : "Завершен"}</span></p>
                        </div>
                    </div>
                    <button className="card-button project-button" type="button" onClick={(e) => {
                        window.location.href = "/tasks/" + project.orderId
                    }}>Подробнее</button>
                </div>
            </div>)


        setProjectsData([...projectsData, ...projects]);

    }

    return (
        <section className="projects">
            <h2 className="center">Мои проекты:</h2>

            {projectsData.length !== 0 ?
                <div className="project-cards">
                    {projectsData}
                    {lastCount === 5 ?
                    <input className="more-cards-btn" type="button" value="Показать больше"/> : <></> }
                </div> :
                <div className="no-data">У вас пока нет заказов! <br/> Для того, чтобы они появились, откликнитесь на
                    них в разделе "Заказы" <br/> Чтобы увеличить вероятность получения заказа, отредактируйте
                    информацию о себе. <br/> Добавьте данные о ваших навыках, описание о себе, о вашем
                    образовании.</div>

            }

        </section>
    );
}

export default ProjectsConstructor;