import {useEffect, useState} from "react";
import {useParams} from "react-router-dom";
import axios from "axios";
// import Header from "../../../main/header";
import CreatorProjectInfo from "../creator-project-info";
import ExecutorBlock from "../executor-block";
import NewExecutorsBlock from "../new-executors-block";
// import Footer from "../../../main/footer";
import BaseInfoForCustomer from "./base-info-for-customer";

const ProjectPageCustomer = () => {
    const [data, setData] = useState();
    const [newExecutors, setNewExecutors] = useState([]);
    const [executors, setExecutors] = useState([])
    const [count, setCount] = useState();

    let params = useParams();
    const url = process.env.REACT_APP_API_URL+'/api/Order/detail_order_info/' + params.id
    const headers = window.sessionStorage.getItem('token') === null ?
        {'Access-Control-Allow-Origin' : '*',
            'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS'
        } :
        {'Access-Control-Allow-Origin' : '*',
            'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
            'Authorization' : "Bearer " + window.sessionStorage.getItem('token')
        }

    useEffect(() => {
        console.log(params.id)
        axios.get(process.env.REACT_APP_API_URL+'/api/Order/detail_order_info/' + params.id,
            {
                headers,

            })
            .then((response) => {
                console.log(response.data);
                setData(response.data);
                filterAndSetExecutors(response.data.executorResponse);
                setCount(window.sessionStorage.getItem('role') === 'customer_role' && data !== undefined ? getNotifyCount(data.executorResponse) : 0)
            })
            .catch((error) => {console.log(error)});


    }, [])
    console.log(data);
    console.log("count " + count);

    function getNotifyCount(executors) {
        let count = 0;
        for(let executor of executors) {
            if ((executor.projectStatus === "Завершен" && executor.reviewStatus === "Не предоставлен") || executor.isAccepted === null)
                count++;
        }
        return count;
    }

    function filterAndSetExecutors(executors) {
        let accepted = []
        let news = []
        for (let executor of executors) {
            if (executor.isAccepted)
                accepted.push(executor)
            else news.push(executor)
        }
        setNewExecutors(news);
        setExecutors(accepted);
    }

    return(
        <>
            {/*<Header/>*/}
            <div className="main">
                <div className="current-project">
                    <h1 className="page-title">Страница заказа</h1>
                    <p className="navigation-after-title"><a href="/personal-area">личный кабинет</a>/<a href="#">страница заказа</a>
                    </p>
                    {data !== undefined ?
                        <>
                            <BaseInfoForCustomer info={data}/>
                            <CreatorProjectInfo info={data}/> :

                            {
                                data.executorResponse !== undefined ?
                                    <>
                                        {executors.length !== 0 && <ExecutorBlock executors={executors} orderId={data.orderId} isCompleted={data.isCompleted}/> }
                                        {newExecutors.length !== 0 && !data.isCompleted && <NewExecutorsBlock executors={newExecutors}/> }
                                    </> :
                                    <></>
                            }
                        </> : <></>
                    }

                </div>

            </div>
            {/*<Footer/>*/}
        </>
    )
}

export default ProjectPageCustomer