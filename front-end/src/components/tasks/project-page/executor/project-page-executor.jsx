// import Header from "../../../main/header";
import CreatorProjectInfo from "../creator-project-info";
import CustomerInfoNotAccepted from "../customer-info-not-accepted";
// import Footer from "../../../main/footer";
import React, {useEffect, useState} from "react";
import axios from "axios";
import {useParams} from "react-router-dom";
import BaseInfoForExecutor from "./base-info-for-executor";
import ReviewCard from "../../../user-page/review-card";

const ProjectPageExecutor = () => {
    const [data, setData] = useState();
    const params = useParams();
    const [reviews, setReviews] = useState([]);

    const url = process.env.REACT_APP_API_URL + "/api/Order/order/" + params.id
    const headers = window.sessionStorage.getItem('token') === null ?
        {
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS'
        } :
        {
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
            'Authorization': "Bearer " + window.sessionStorage.getItem('token')
        }

    useEffect(() => {
        axios.get(url,
            {
                headers,

            })
            .then((response) => {
                console.log(response.data)
                setData(response.data);
                if (window.sessionStorage.getItem('token') !== null)
                    axios.get(process.env.REACT_APP_API_URL + "/api/Executor/executor_reviews/" + response.data.orderId,
                        {
                            headers,
                        })
                        .then((r) => {
                            console.log(r.data);
                            setReviews(r.data);

                        })
                        .catch((error) => {
                            console.log(error)
                        });
            })
            .catch((error) => {
                console.log(error)
            });


    }, [])


    return (
        <>
            {/*<Header/>*/}
            <div className="main">
                <div className="current-project">
                    <h1 className="page-title">Страница заказа</h1>
                    <p className="navigation-after-title"><a href="/personal-area">личный кабинет</a>/<a href="#">страница
                        заказа</a>
                    </p>
                    {data !== undefined ?
                        <>

                            <BaseInfoForExecutor info={data}/>
                            {window.sessionStorage.getItem('role') !== undefined && (data.isAccepted === true) ?
                                <CreatorProjectInfo info={data}/> :
                                <CustomerInfoNotAccepted info={data}/>

                            }
                            {reviews.length > 0 &&
                                <section className="card">
                                    <h2 style={{
                                        textAlign: "center"
                                    }}>Отзыв о вашей работе:</h2>
                                    <div className="card-content">
                                        {reviews.map((review) => {
                                            console.log(review);
                                            return <ReviewCard info={review} key={"review-my-" + review.id}/>
                                        })}
                                    </div>
                                </section>
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