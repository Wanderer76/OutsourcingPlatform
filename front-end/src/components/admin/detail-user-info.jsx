import React, {useEffect, useState} from "react";
import axios from "axios";
import ExecutorInfo from "./executor-info";
import CustomerAdminInfo from "./customer-admin-info";
import emptyUser from "../../images/empty-user.svg";

const DetailUserInfo = ({username, viewChatsClick}) => {
    const [userInfo, setInfo] = useState();
    const [popup, setPopup] = useState(<></>);
    useEffect(() => {
        console.log(username)
        axios.get(process.env.REACT_APP_API_URL + "/api/Admin/detail_info",
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    },
                params: {
                    username: username
                }

            })
            .then((response) => {
                console.log(response.data)
                setInfo(response.data);
            })
            .catch((error) => {
                console.log(error)
            });
    }, [])

    function viewPopup()
    {
        setPopup(
            <div className="popup-wrapper">
                <div className="popup delete-project-popup">
                    <h2>{userInfo.isBanned? "Разблокировать" : "Заблокировать"} пользователя</h2>
                    <div className="user-for-delete">
                        <div className="user-photo">
                            <img src={emptyUser}/>
                        </div>
                        <div className="user-for-delete-name">{userInfo.surname} {userInfo.name} {userInfo.secondName}</div>
                    </div>
                    <textarea
                        className="reason-for-deleting"
                        id="reason-for-deleting"
                        name="reason-for-deleting"
                        placeholder="Комментарий к блокировке пользователя."
                    ></textarea>
                    <div className="popup-delete-buttons buttons">
                        <a className="project-card-btn" href="#" onClick={(e) => {
                            e.preventDefault()
                            setPopup(<></>)
                        }}>Отмена</a>
                        <a className="project-card-btn" href="#" onClick={(e) => {
                            e.preventDefault()
                            banUser()
                        }}>{userInfo.isBanned? "Разблокировать" : "Заблокировать"}</a>
                    </div>
                </div>
            </div>)
    }

    function banUser() {
        axios.post(process.env.REACT_APP_API_URL + "/api/Admin/change_account_status",
            {
                userId: userInfo.contacts.id,
                isBanned: !userInfo.isBanned,
                message: document.getElementById("reason-for-deleting").value
            },
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    }

            })
            .then((response) => {
                console.log("user " + userInfo.contacts.id + " banned");
                userInfo.isBanned = !userInfo.isBanned
                setPopup(<></>)

            })
            .catch((error) => {
                console.log(error)
            });
    }
    return(
        <>
            {popup}
            {userInfo !== undefined ? ("executor" in userInfo ? <ExecutorInfo info={userInfo} onClick={viewPopup} viewChatsClick={() => viewChatsClick(userInfo.contacts.id)}/> : <CustomerAdminInfo info={userInfo} onClick={viewPopup} viewChatsClick={() => viewChatsClick(userInfo.contacts.id)}/>) : <></>
            }
        </>
    )
}

export default DetailUserInfo;