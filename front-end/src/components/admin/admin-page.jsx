import React, {useEffect, useState} from "react";
import AdminHeader from "./admin-header";
import Footer from "../main/footer";
import emptyUser from "../../images/empty-user.svg"
import DetailUserInfo from "./detail-user-info";
import StatisticPanel from "./statistic-panel";
import axios from "axios";
import UserLi from "./user-li";
import detailUserInfo from "./detail-user-info";
import UserChatsForAdmin from "../main/user-chats-for-admin";
const AdminPage = () => {
    let status = "all";
    const [usersCount, setUsersCount] = useState(0);
    const [currentOffset, setCurrentOffset] = useState(1);
    const limit = 10;
    const [users, setUsers] = useState([]);
    const [usersSystemInfo, setUsersSystemInfo] = useState({banndedCount: 0, customerCount: 0, executorCount: 0})
    const [pageUl, setPageUl] = useState(<></>)
    const [detailInfoBlock, setDetailInfoBlock] = useState(<></>)
    const [chatsPannel, setChatsPannel] = useState(<></>)
    useEffect(() => {
        axios.get(process.env.REACT_APP_API_URL+"/api/Admin/users_system_info",
            {
                headers:
                    {'Access-Control-Allow-Origin' : '*',
                        'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization' : "Bearer " + window.sessionStorage.getItem('token')
                    },

            })
            .then((response) => {
                setUsersSystemInfo(response.data)
                let allUsersCount = response.data.executorCount+response.data.customerCount
                changeMenu(allUsersCount)
                setUsersCount(allUsersCount);
            })
            .catch((error) => {console.log(error)});
    }, [])

    useEffect(() => {
        hidePannel()
    }, [detailInfoBlock])

    function getUsers(offset) {
        let config;
        switch (status) {
            case "all":
                config = {
                    headers:
                        {
                            'Access-Control-Allow-Origin': '*',
                            'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                            'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                        }
                }
                break
            case "blocked":
                config = {
                    headers:
                        {
                            'Access-Control-Allow-Origin': '*',
                            'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                            'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                        },
                    params:
                        {
                            isBanned: true
                        }
                }
                break
            case "active":
                config = {
                    headers:
                        {
                            'Access-Control-Allow-Origin': '*',
                            'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                            'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                        },
                    params:
                        {
                            isBanned: false
                        }
                }
                break
        }
        axios.get(process.env.REACT_APP_API_URL+"/api/Admin/all_users/"+offset+"/" + limit, config)
            .then((response) => {
                setUsers(response.data);
            })
            .catch((error) => {console.log(error)});
    }

    function changeMenu(allUsersCount, newOffset=1) {
        console.log("newoffset: " + newOffset);

        if (newOffset <= 0 || allUsersCount <= (newOffset-1) * limit)
            return
        setCurrentOffset(newOffset);
        console.log("currentOffset: " + currentOffset)
        let offset = (newOffset-1) * limit
        getUsers(offset)
        let pageButtons = []
        for (let i = 1; i < allUsersCount / limit + 1; i++) {
            pageButtons.push(<li key={"pageButtons-" + i} className={i===newOffset ? "pagination-item checked" : "pagination-item"} onClick={(e) => changeMenu(allUsersCount, i)}>{i}</li>)
        }
        setPageUl(pageButtons)
    }

    function changeStatus(e) {
        console.log(e.target.id)
        status = e.target.id;
        setCurrentOffset(1)
        switch (status) {
            case "all":
                setUsersCount(usersSystemInfo.customerCount+usersSystemInfo.executorCount)
                changeMenu(usersSystemInfo.customerCount+usersSystemInfo.executorCount)
                break
            case "blocked":
                setUsersCount(usersSystemInfo.bannedCount)
                changeMenu(usersSystemInfo.bannedCount)
                break
            case "active":
                console.log()
                setUsersCount(usersSystemInfo.customerCount+usersSystemInfo.executorCount - usersSystemInfo.bannedCount)
                changeMenu(usersSystemInfo.customerCount+usersSystemInfo.executorCount - usersSystemInfo.bannedCount)
        }
        document.getElementById("all").className = "role-btn";
        document.getElementById("active").className = "role-btn";
        document.getElementById("blocked").className = "role-btn";
        e.target.className = "role-btn checked"
    }


    function getDetailInfo(username) {
        setDetailInfoBlock(<DetailUserInfo key={"detailUserInfo="+username} username={username} viewChatsClick={viewChatsPannel}/>)
    }

    function viewChatsPannel(userId) {
        setChatsPannel(<UserChatsForAdmin userId={userId} hidePannel={hidePannel}/>)
    }
    function hidePannel() {
        setChatsPannel(<></>)
    }


    return(
        <>
            <section className="admin-panel">
                <StatisticPanel usersCount = {usersSystemInfo}/>
                    <div className="all-users">
                        <h2>Фильтр:</h2>
                        <div className="all-users-filters">
                            <div className="users-filters-radio">
                                <input className="visually-hidden" type="radio" id="show-all" name="role" value="all"/>
                                    <label className="role-btn checked" htmlFor="show-all" id={"all"}  onClick={e => changeStatus(e)}>Все пользователи</label>
                                    <input className="visually-hidden" type="radio" id="show-active" name="role"
                                           value="active"/>
                                        <label className="role-btn" htmlFor="show-active" id={"active"} onClick={e => changeStatus(e)}>Только активные</label>
                                        <input className="visually-hidden" type="radio" id="show-blocked" name="role"
                                               value="blocked"/>
                                            <label className="role-btn" htmlFor="show-blocked" id={"blocked"} onClick={e => changeStatus(e)}>Только
                                                заблокированные</label>
                            </div>
                        </div>
                        <h2>Результаты:</h2>
                        <div className="all-users-list">
                            <ul className="list-result-items">
                                {users.map(user => <UserLi key={user.id} id={user.id} name={user.name} surname={user.surname} secondName={user.secondName} userRole={user.userRole} isBanned={user.isBanned} onClick={() => getDetailInfo(user.username)}/>)}

                            </ul>
                            {usersCount > limit &&
                                <ul className="list-pagination">
                                    {/*<li className="pagination-item edge" onClick={() => changeMenu(usersSystemInfo.customerCount + usersSystemInfo.executorCount, currentOffset - 1)}>Назад</li>*/}
                                    {pageUl}
                                    {/*<li className="pagination-item edge" onClick={() => changeMenu(usersSystemInfo.customerCount + usersSystemInfo.executorCount, currentOffset + 1)}>Далее</li>*/}
                                </ul>}
                        </div>
                    </div>

                {detailInfoBlock}
                {chatsPannel}
            </section>
        </>
    )
}

export default AdminPage;