// import Header from "../main/header";
import UserPersonal from "./user-personal";
import CompetenceBlock from "./executor/competence-block";
import Educations from "./executor/educations";
import ProjectsConstructor from "./executor/projects-constructor";
import CompanyInfo from "./customer/company-info";
import ProjectsCustomer from "./customer/projects-customer";
import {resourcesExecutor} from "./area-info";
import axios from "axios";
import {useEffect, useState} from "react";
// import Footer from "../main/footer";

const PersonalArea = () => {
    const [res, setRes] = useState(undefined);
    let status = window.sessionStorage.getItem('role');

    let url = status === "customer_role" ? process.env.REACT_APP_API_URL + "/api/PersonalArea/customer/area" : process.env.REACT_APP_API_URL + "/api/PersonalArea/executor/area"
    useEffect(() => {
        axios.get(url,
            {
                headers:
                    {'Access-Control-Allow-Origin' : '*',
                        'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization' : "Bearer " + window.sessionStorage.getItem('token')
                    },

            })
            .then((response) => {
                if (response.status === 200)
                    setRes(response.data);
                console.log(response);
            })
            .catch((error) => {console.log(error)});


    }, [])


    let specialInfo;
    if (res !== undefined && 'executor' in res) {
        specialInfo =  <>
            <CompetenceBlock competencies={res['executor']['categories']} skills={res.executor.skills} />
            <Educations educations={res['executor']['educations']}/>
            <ProjectsConstructor/>
        </>
    }
    else {
        specialInfo = res !== undefined && 'customer' in res ? <>
            <CompanyInfo info={res['customer']}/>
            <ProjectsCustomer/>
        </> : <></>
    }

    return (
        <div>
            {/*<Header username={window.sessionStorage.getItem('username')}/>*/}
            <div className="main">
                {res !== undefined ?
                    <><UserPersonal res={res} status={status}/>
                        {specialInfo}</> : <></>
                }

            </div>
            {/*<Footer/>*/}
        </div>
    );
}

export default PersonalArea;