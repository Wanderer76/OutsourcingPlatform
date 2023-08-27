// import Header from "../../../main/header";
import CustomerInfo from "../customer-info";
import CustomerInfoNotAccepted from "../customer-info-not-accepted";
// import Footer from "../../../main/footer";
import {useEffect, useState} from "react";
import axios from "axios";
import {useParams} from "react-router-dom";
import BaseInfoForExecutor from "./base-info-for-executor";

const ProjectPageExecutor = () => {
    const [data, setData] = useState();
    const params = useParams();
    const url = process.env.REACT_APP_API_URL+"/api/Order/order/" + params.id
    const headers = window.sessionStorage.getItem('token') === null ?
        {'Access-Control-Allow-Origin' : '*',
            'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS'
        } :
        {'Access-Control-Allow-Origin' : '*',
            'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
            'Authorization' : "Bearer " + window.sessionStorage.getItem('token')
        }

    useEffect(() => {
        axios.get(url,
            {
                headers,

            })
            .then((response) => {
                setData(response.data);
            })
            .catch((error) => {console.log(error)});


    }, [])


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
                            <BaseInfoForExecutor info={data}/>
                            {window.sessionStorage.getItem('role') !== undefined && (data.isAccepted === true)?
                                <CustomerInfo info={data}/> :
                                <CustomerInfoNotAccepted info={data}/>
                            }
                        </> : <></>
                    }

                </div>

            </div>
            {/*<Footer/>*/}
        </>
    )
}

export default ProjectPageExecutor;