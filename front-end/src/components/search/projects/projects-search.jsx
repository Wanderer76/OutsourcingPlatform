// import Header from "../../main/header";
import ProjectCardSearch from "./project-card-search";
import {useEffect, useState} from "react";
import axios from "axios";
import SearchFilter from "../search-filter";
// import Footer from "../../main/footer";


const ProjectsSearch = () => {
    const [count, setCount] = useState();
    const [projects, setProjects] = useState();
    const [skillsFilter, setSkillsFilter] = useState([]);
    const [themesFilter, setThemesFilter] = useState([]);
    const config = window.sessionStorage.getItem('token') !== null ? {
            headers:
                {'Access-Control-Allow-Origin' : '*',
                    'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                    'Authorization' : "Bearer " + window.sessionStorage.getItem('token')
                },

        } :
        {
            headers:
                {'Access-Control-Allow-Origin' : '*',
                    'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                },

        }
    useEffect(() => {
        axios.post(process.env.REACT_APP_API_URL+"/api/Order/order_list/0/10", {}, config)
            .then((response) => {
                if (response.status === 200)
                    console.log(response.data)
                    setProjects(response.data.orders.map((project) => {
                        return <ProjectCardSearch key={project.orderId} info={project}/>
                    }));
                setCount(response.data.count);
            })
            .catch((error) => {console.log(error)});

    }, [])

    function filterOrders() {
        let data =  skillsFilter.length === 0 && themesFilter.length === 0 ? {} : {
                        "skills": skillsFilter,
                        "categories": themesFilter
                    }
        console.log(data)
        axios.post(process.env.REACT_APP_API_URL+"/api/Order/order_list/0/10",
            data,config)
            .then((response) => {
                if (response.status === 200)
                    console.log(response.data)
                setProjects(response.data.orders.map((project) => {
                    return <ProjectCardSearch key={project.orderId} info={project}/>
                }));
                setCount(response.data.count);
            })
            .catch((error) => {console.log(error)});
        // console.log({
        //             "skills": skillsFilter,
        //             "categories": themesFilter
        //         })
    }

    return(
        <>
        {/*<Header username={window.sessionStorage.getItem('username')}/>*/}
        <div className="main">

            <SearchFilter thing={"заказов"} setSkillsFilter={setSkillsFilter} setThemesFilter={setThemesFilter} sendForm={filterOrders}/>
            <section className="projects">
                <h2 className="center">Результаты поиска:</h2>
                <div className="project-cards">
                    {projects === undefined ? <></> : projects}


                    { count / 10 > 1 &&
                        <ul className="list-pagination">
                            <li className="pagination-item edge">Назад</li>
                            <li className="pagination-item checked">1</li>
                            <li className="pagination-item">2</li>
                            <li className="pagination-item">3</li>
                            <li className="pagination-item">...</li>
                            <li className="pagination-item">10</li>
                            <li className="pagination-item edge">Далее</li>
                        </ul>}
                </div>
            </section>
        </div>
        {/*<Footer/>*/}
        </>
    )
}

export default ProjectsSearch;