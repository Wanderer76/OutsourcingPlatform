import {useEffect, useState} from "react";
import axios from "axios";
import ExecutorCard from "./executor-card";
// import Header from "../../main/header";
import SearchFilter from "../search-filter";
import ProjectCardSearch from "../projects/project-card-search";
// import Footer from "../../main/footer";
const ExecutorsSearch = () => {
    const [executors, setExecutors] = useState();
    const [count, setCount] = useState()
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
        axios.post(process.env.REACT_APP_API_URL+"/api/Executor/executor_list/0/10", {}, config)
            .then((response) => {
                console.log(response.data)
                if (response.status === 200)
                    setExecutors(response.data.executors.map((project) => {
                        return <ExecutorCard key={project.executorId} info={project}/>
                    }));
                setCount(response.data.count)
            })
            .catch((error) => {console.log(error)});

    }, [])

    function filterOrders() {
        let data =  skillsFilter.length === 0 && themesFilter.length === 0 ? {} : {
            "skills": skillsFilter,
            "categories": themesFilter
        }
        console.log(data)
        axios.post(process.env.REACT_APP_API_URL+"/api/Executor/executor_list/0/10",
            data,config)
            .then((response) => {
                if (response.status === 200)
                    console.log(response.data)
                setExecutors(response.data.executors.map((project) => {
                    return <ExecutorCard key={project.executorId} info={project}/>
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
            {/*<Header/>*/}
            <div className="main">
                <SearchFilter thing={"исполнителей"} setSkillsFilter={setSkillsFilter} setThemesFilter={setThemesFilter} sendForm={filterOrders}/>
                <section className="projects">
                    <h2 className="center">Результаты поиска:</h2>
                    <div className="project-cards">
                        {executors === undefined ? <></> : executors}
                    </div>
                </section>
            </div>
            {/*<Footer/>*/}
        </>
    )
}

export default ExecutorsSearch