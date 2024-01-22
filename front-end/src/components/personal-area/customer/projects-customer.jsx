import deadlineIcon from "../../../images/deadline-icon.svg"
import custAvaDef from "../../../images/customer-icon.svg"
import checkListIcon from "../../../images/checklist-icon.svg"
import notificationIcon from "../../../images/notification-icon.svg"
import ProjectCard from "./project-card";
import {useEffect, useState} from "react";
import axios from "axios";

const ProjectsCustomer = (props) => {
    const [projectsData, setProjectData] = useState([]);
    const [offset, setOffset] = useState(0);
    const [lastCount, setLastCount] = useState(0);
    useEffect(() => {
        getProjectsData()

    }, [])

    function getProjectsData() {
        axios.get(process.env.REACT_APP_API_URL + "/api/Order/user_created_orders/" + offset + "/5",
            {
                headers:
                    {'Access-Control-Allow-Origin' : '*',
                        'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization' : "Bearer " + window.sessionStorage.getItem('token')
                    },

            })
            .then((response) => {
                if (response.status === 200) {
                    // setProjectData(response.data);
                    fillProjectsData(response.data);
                    setOffset(offset + 5);
                    setLastCount(response.data.length);
                }

                console.log(projectsData);
            })
            .catch((error) => {console.log(error)});
    }

    function fillProjectsData(data) {
        console.log(data)
        let projects = data
            .sort((a,b) => {
                if (a.isCompleted && !b.isCompleted)
                    return 1;
                if (!a.isCompleted && b.isCompleted)
                    return -1;
                return new Date(a.deadline) - new Date(b.deadline)
            })
            .map((project) => {
                return <ProjectCard key={"project-"+project.orderId} projectInfo={project}/>
            }
        )
        setProjectData([...projectsData, ...projects]);
    }
    return (
        <section className="projects">
            <div className="project-title-row">
                <h2>Мои заказы:</h2>
                <a className="to-new-project-form" href="/tasks/create">Создать новый заказ</a>
            </div>
            {projectsData.length !== 0 ?
                <div className="project-cards">
                    {projectsData}
                    {lastCount === 5 ?
                        <input className="more-cards-btn" type="button" value="Показать больше" onClick={getProjectsData}/> : <></> }
                </div> :
                <div className="no-data">У вас пока нет заказов! <br/> Для того, чтобы они появились, создайте его, нажав на кнопку "Создать заказ" <br/></div>

            }
            {/*<div className="project-cards">*/}
            {/*    {projectsData}*/}
            {/*</div>*/}
        </section>
    );
}

export default ProjectsCustomer;