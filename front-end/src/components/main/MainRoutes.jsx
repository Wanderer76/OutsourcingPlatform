import Header from "./header";
import Footer from "./footer";
import {Route, Routes} from "react-router-dom";
import PersonalArea from "../personal-area/perosnal-area";
import EditData from "../personal-area/edit-data";
import CreateNewTask from "../tasks/create-update/create-new-task";
import UpdateTask from "../tasks/create-update/update-task";
import ProjectPageCustomer from "../tasks/project-page/customer/project-page-customer";
import ProjectPageExecutor from "../tasks/project-page/executor/project-page-executor";
import Review from "../tasks/review/review";
import ProjectsSearch from "../search/projects/projects-search";
import CustomerPage from "../user-page/customer-page";
import ConstructorPage from "../user-page/constructor-page";
import ExecutorsSearch from "../search/executors/executors-search";
import NotFoundPage from "./not-found-page";
import MainPage from "./main";
import Messanger from "../messanger/messanger";
import AdminHeader from "../admin/admin-header";
import AdminPage from "../admin/admin-page";
import TagsEditor from "../admin/tags-editor";

const MainRoutes = () => {
    return (
        <>
            {window.sessionStorage.role !== "admin_role" ?  <Header /> : <AdminHeader />}
            <Routes>
                <Route path="/" element={<MainPage />}/>
                <Route path="/personal-area" element={<PersonalArea />}/>
                <Route path="/personal-area/edit" element={<EditData />}/>

                <Route path="/tasks/create" element={<CreateNewTask />}/>
                <Route path="/tasks/my/update/:orderId" element={<UpdateTask />}/>
                <Route path="/tasks/my/:id" element={<ProjectPageCustomer />} />
                <Route path="/tasks/:id" element={<ProjectPageExecutor />} />
                <Route path="/tasks/review" element={<Review />} />
                <Route path="/tasks/search" element={<ProjectsSearch/>} />

                <Route path="/customers/:id" element={<CustomerPage/>}/>
                <Route path="/executors/:id" element={<ConstructorPage/>}/>
                <Route path="/executors/search" element={<ExecutorsSearch/>} />
                <Route path="/messenger" element={<Messanger/>}/>
                <Route path="/review/:executorId/:orderId" element={<Review/>} />
                <Route path="/*" element={<NotFoundPage/>} />
                <Route path={"/admin"} element={<AdminPage />} />
                <Route path={"/admin/tags-editor"} element={<TagsEditor />} />

            </Routes>
            <Footer />
        </>
    )
}

export default MainRoutes