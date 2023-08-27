import {personalData} from "./executor/personal-data";
import EditExecutorData from "./executor/edit-executor-data";
import EditCustomerData from "./customer/edit-customer-data";
// import Header from "../main/header";
// import Footer from "../main/footer";

const EditData = () => {

    return(
        <>
            {/*<Header/>*/}
            {window.sessionStorage.getItem('role') === "customer_role" ?
                <EditCustomerData/> :
                <EditExecutorData/>
            }
            {/*<Footer/>*/}
        </>

    )
}

export default EditData;