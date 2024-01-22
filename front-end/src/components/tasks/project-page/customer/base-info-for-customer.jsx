import {useParams} from "react-router-dom";
import {useState} from "react";
import {formatTagValue} from "../../format-tag";
import logoBlack from "../../../../images/logo-black.svg";
import axios from "axios";

const BaseInfoForCustomer = (props) => {
    const params = useParams();
    const [popup, setPopup] = useState(<></>)
    let info = props.info;
    let themes;
    let competencies;
    let orderRoles;
    themes = info === null || info.orderCategories === undefined ? <></> : info.orderCategories.map((theme) => {
        return(<div className="field-card-tag" key={theme.id}>{formatTagValue(theme.name)}</div>);
    });

    competencies = info === null || info.orderSkills === null  ? <></> : info.orderSkills.map((comp) => {
        return(<div className="field-card-tag" key={comp.id}>{formatTagValue(comp.name)}</div>);
    });

    orderRoles = info === null || info.orderVacancies === undefined  ? <></> : info.orderVacancies.map((comp) => {
        return(<div className="field-card-tag" key={comp.orderRole.id}>{formatTagValue(comp.orderRole.name)}</div>);
    });
    function viewFinishPopup() {
        setPopup(<div className="popup-wrapper">
            <div className="popup leave-account-popup">
                <img className="finish-project-popup-logo" src={logoBlack}/>
                    <h2 className="leave-text">Закрыть заказ раньше срока дедлайна?</h2>
                    <p id={"finishPopupText"} hidden>Сюда вставится текст ошибки</p>
                    <div className="popup-buttons buttons">
                        <a id="closeProjectButton" className="project-card-btn" onClick={finishOrder}>Закрыть проект</a>
                        <a className="project-card-btn" onClick={hidePopup}>Отмена</a>
                </div>
            </div>
        </div>)
    }

    function viewDeletePopup() {
        setPopup(<div className="popup-wrapper">
            <div className="popup leave-account-popup">
                <img className="finish-project-popup-logo" src={logoBlack}/>
                    <h2 className="leave-text">Вы желаете удалить заказ?</h2>
                <p id={"deletePopupText"} hidden>Сюда вставится текст ошибки</p>
                    <div className="popup-buttons buttons">
                        <a id="deleteProjectButton" className="project-card-btn" onClick={deleteOrder}>Удалить</a>
                        <a className="project-card-btn" onClick={hidePopup}>Отмена</a>
                    </div>
            </div>
        </div>)
    }

    function hidePopup() {
        setPopup(<></>)
    }
    function deleteOrder() {
        axios.delete(process.env.REACT_APP_API_URL+"/api/Order/delete/" + params.id,
            {
                headers:
                    {'Access-Control-Allow-Origin' : '*',
                        'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        Authorization : "Bearer " + window.sessionStorage.getItem('token')
                    },
            })
            .then((response) => {
                console.log("project " + params.id + " deleted")
                window.location.href = "/personal-area";
            })
            .catch((error) => {
                console.log(error.response.data);
                document.getElementById("deleteProjectButton").hidden = true;
                document.getElementById("deletePopupText").textContent = error.response.data;
                document.getElementById("deletePopupText").hidden = false;
            });
    }

    function finishOrder() {
        axios.post(process.env.REACT_APP_API_URL+"/api/Order/finish_project/" + params.id, null,
            {
                headers:
                    {'Access-Control-Allow-Origin' : '*',
                        'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization' : "Bearer " + window.sessionStorage.getItem('token')
                    },

            })
            .then((response) => {
                console.log("project " + params.id + " deleted")
                window.location.href = "/personal-area";
            })
            .catch((error) => {
                console.log(error.response.data)
                document.getElementById("closeProjectButton").hidden = true;
                document.getElementById("finishPopupText").textContent = error.response.data;
                document.getElementById("finishPopupText").hidden = false;
            });
    }

    return(
        <>
            {popup}
            <section className="project-desc-card">
                <h2 className="visually-hidden">Описание заказа</h2>
                <p className="project-card-name">{info.name}</p>
                <p className="project-card-description">{info.description}</p>
                {info.orderCategories !== null && info.orderCategories.length !== 0 &&
                    <div className="project-card-tags fields-row">
                        <h3>Сферы деятельности:</h3>
                        <div className="project-card-tags tags-fields">
                            {themes}
                        </div>
                    </div>
                }
                {info.orderSkills !== null && info.orderSkills.length !== 0 &&
                    <div className="project-card-tags skills-row">
                        <h3>Компетенции:</h3>
                        <div className="project-card-tags tags-skills">
                            {[...competencies]}
                        </div>
                    </div>
                }

                {info.orderVacancies !== null && info.orderVacancies.length !== 0 &&
                    <div className="project-card-tags skills-row">
                        <h3>Роли:</h3>
                        <div className="project-card-tags tags-skills">
                            {[...orderRoles]}
                        </div>
                    </div>
                }

                {/*{window.sessionStorage.getItem('status') === "contractor" ?*/}
                {/*    <a className="finish-project project-card-btn" href="#">Завершить проект</a> :*/}
                {/*    <></>*/}
                {/*}*/}
                {window.sessionStorage.getItem('role') === undefined ?
                    <a className="finish-project project-card-btn" href="/signin">Отправить заявку</a> :
                    <></>
                }

                {props.info.isCompleted === false &&
                    <div className="project-card-tags control-btns">
                        <h3>Действие с заказом:</h3>
                        <div className="project-card-tags tags-skills">
                            <button type="button" className="control-project" onClick={viewFinishPopup}>Завершить</button>
                            <button type="button" className="control-project" onClick={() => {window.location.href = "update/" + params.id}}>Редактировать</button>
                            <button className="control-project" onClick={viewDeletePopup}>Удалить</button>
                        </div>
                    </div>
                }
            </section>
        </>
    )
}

export default BaseInfoForCustomer