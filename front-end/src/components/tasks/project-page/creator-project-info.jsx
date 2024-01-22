import avatar from "../../../images/empty-user.svg"
import tgIcon from "../../../images/telegram-icon.svg"
import vkIcon from "../../../images/vk-icon.svg"
import checkListIcon from "../../../images/checklist-icon.svg"
import deadLineIcon from "../../../images/deadline-icon.svg"
import customerIcon from "../../../images/customer-icon.svg"
import React from "react";
import telegram from "../../../images/telegram-icon.svg";
import gitIcon from "../../../images/github-icon.svg";
const CreatorProjectInfo = (props) => {
    let info = props.info;
    console.log("info in customer info ")
    console.log(info)
    console.log(info.customer)
    return(

        <section className="consumer-data-card card">
            <div className="consumer-data-card column-1">
                <p className="consumer-data-card company-name">{info.companyName}</p>
                {window.sessionStorage.getItem('role') === 'customer_role'&&
                <address>
                    <p className="consumer-data-card company-address">Адрес: {info.address}</p>
                    <p className="consumer-data-card company-inn">{"ИНН: " + info.inn}</p>
                </address>
                }{window.sessionStorage.getItem('role') !== 'customer_role'&&
                null
                }

                <div className="consumer-data-card person-row">
                    <div className="consumer-data-card little-avatar">
                        <img className="consumer-data-card avatar-image" src={avatar} alt="Автар пользователя"/>
                    </div>
                    <p className="consumer-data-card person-name">{info.customer !== null ? info.username : info.username}</p>
                </div>

                <div className="consumer-data-card person-contatcts">
                    <p className="consumer-data-card person-phone">Телефон: <a href={"tel:"+info.phone}>{info.phone}</a></p>
                    <p className="consumer-data-card person-email">E-mail:
                        <a href={"mailto:" + info.email !== null ? info.email : "email"}>{info.email !== null ? info.email : "email"}</a></p>
                </div>
            </div>
            <div className="consumer-data-card column-2">
                <h3>Параметры:</h3>
                <div className="consumer-data-card project-params">
                    <div className="consumer-data-card one-param">
                        <img className="consumer-data-card param-icon" src={checkListIcon} alt = "param icon" />
                            <p className="consumer-data-card param-text">Статус: <span>{new Date(info.deadline) - new Date() > 0 ? "В процессе" : "Завершен" }</span></p>
                    </div>
                    <div className="consumer-data-card one-param">
                        <img className="consumer-data-card param-icon" src={deadLineIcon} alt="param icon"/>
                            <p className="consumer-data-card param-text">Дедлайн: <span>{new Date(info.deadline).toLocaleString("ru", {year: 'numeric', month: 'long', day: 'numeric'})}</span></p>
                    </div>

                        <div className="consumer-data-card one-param">
                            <img className="consumer-data-card param-icon" src={customerIcon} alt="param icon"/>
                            <p className="consumer-data-card param-text">Ролей: <span>{info.orderVacancies===null?0:info.orderVacancies.map(x=>x.responses).length}</span></p>
                        </div>


                </div>
                <h3>Ссылки:</h3>
                <div className="consumer-data-card person-links">
                    {
                        props.info.vk === "" && props.info.messanger === "" ?
                            <div className="no-data">Нет контактов</div> :
                            <>
                                {props.info.contacts.githubUrl !== null && props.info.contacts.githubUrl !== undefined && props.info.contacts.githubUrl !== "" &&
                                    <a target="_blank" title={props.info.contacts.githubUrl.includes("github.com/") ? props.info.contacts.githubUrl : "https://github.com/" + props.info.contacts.githubUrl} href={props.info.contacts.githubUrl.includes("github.com/") ? props.info.contacts.githubUrl : "https://github.com/" + props.info.contacts.githubUrl}><img
                                        className="social-logo" src={gitIcon} alt="Логотип Github"/></a>
                                }
                                {props.info.contacts.vkNickname !== null && props.info.contacts.vkNickname !== undefined &&  props.info.contacts.vkNickname !== "" &&
                                    <a target="_blank" title={props.info.contacts.vkNickname.includes("vk.com") ? props.info.contacts.githubUrl : "https://vk.com/" + props.info.contacts.vkNickname} href={props.info.contacts.vkNickname.includes("vk.com/") ? props.info.contacts.githubUrl : "https://vk.com/" + props.info.contacts.vkNickname}><img
                                        className="social-logo" src={vkIcon} alt="Логотип VK"/></a>
                                }
                                {props.info.contacts.messager !== null && props.info.contacts.messager !== undefined && props.info.contacts.messager !== "" &&
                                    <a target="_blank" title={props.info.contacts.messager.includes("t.me") ? props.info.contacts.messager : "https://t.me/" + props.info.contacts.messager} href={props.info.contacts.messager.includes("t.me/") ? props.info.contacts.messager : "https://t.me/" + props.info.contacts.messager}><img
                                        className="social-logo" src={tgIcon} alt="Логотип VK"/></a>
                                }

                            </>
                    }

                </div>
            </div>
        </section>
    )
}

export default CreatorProjectInfo;