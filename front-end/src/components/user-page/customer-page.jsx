import UserPersonal from "../personal-area/user-personal";
import ProjectCardVisitors from "./project-card-visitors";
import {useEffect, useState} from "react";
import axios from "axios";
import {useParams} from "react-router-dom";


const CustomerPage = (props) => {
    const [data, setData] = useState();
    const [orders, setOrders] = useState([]);
    const limit = 10;
    let offset = 0;
    const [lastLoadCount, setLastCount] = useState(0);
    const id = useParams();
    useEffect(() => {
        axios.get(process.env.REACT_APP_API_URL+"/api/Customer/customer_detail_open/"+id.id,
            {
                headers:
                    {'Access-Control-Allow-Origin' : '*',
                        'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization' : "Bearer " + window.sessionStorage.getItem('token')
                    }
            })
            .then((response) => {
                console.log(response.data)
                setData(response.data)
            })
            .catch((error) => {console.log(error)});

        loadOrders()
    }, [])

    function loadOrders() {
        axios.get(process.env.REACT_APP_API_URL+"/api/Customer/customer_orders/",
            {
                headers:
                    {'Access-Control-Allow-Origin' : '*',
                        'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization' : "Bearer " + window.sessionStorage.getItem('token')
                    },
                params: {
                    customerId: id.id,
                    offset: offset,
                    limit: limit
                }
            })
            .then((response) => {
                console.log(response.data)
                offset += limit
                setOrders(response.data)
                setLastCount(response.data.length);
            })
            .catch((error) => {console.log(error)});
    }

    return(
        <div className="main">
            {data !== undefined &&
                <>
                <UserPersonal res={data}/>

                {/*<section className="about-user-person">*/}
                {/*<h2>О себе:</h2>*/}
                {/*<p className="about-person-text">{data.about}</p>*/}
                {/*</section>*/}

                {/*<CompanyInfo info={data.companyInfo}/>*/}

                {orders !== undefined && orders.length !== 0 &&
                <section className="projects">
                <div className="project-title-row center">
                <h2>Заказы пользователя:</h2>
                </div>

                <div className="project-cards">
                    {orders.map((project) => {
                        return <ProjectCardVisitors key={"project-"+project.orderId} info={project}/>
                    })}
                    {lastLoadCount === limit && <input className="more-cards-btn" type="button" id={"see-more"} onClick={loadOrders}
                            value="Показать больше"/>}
                </div>
                </section>}
                </>}

        </div>
    )

}

export default CustomerPage