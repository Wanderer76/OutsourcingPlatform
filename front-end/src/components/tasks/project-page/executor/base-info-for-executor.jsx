import {useParams} from "react-router-dom";
import {useState} from "react";
import {formatTagValue} from "../../format-tag";
import logoBlack from "../../../../images/logo-black.svg";
import axios from "axios";

const BaseInfoForExecutor = (props) => {
    const params = useParams();
    const [popup, setPopup] = useState(<></>)
    let info = props.info;

    const cancelButton = <button className="finish-project project-card-btn" onClick={() => viewCancelResponsePopup()}>Отменить заявку</button>;
    const finishButton = <button className="finish-project project-card-btn" onClick={(event) => viewPopup()}  disabled={info.isCompleted}>Завершить заказ</button>;
    const finishedButton = <button className="finish-project project-card-btn" disabled={true}>Заказ завершен</button>;
    const responseButton = <button className="finish-project project-card-btn" onClick={(e) => {viewResponsePopup()}}>Отправить заявку</button>;
    const responseButtonNotAuth = <a className="finish-project project-card-btn" href="/signin">Отправить заявку</a>;
    const [button, setButton] = useState(getButtonAtStart(info))


    console.log("info: ");
    console.log(info);
    let themes;
    let competencies;
    themes = info === null || info.orderCategories === undefined ? <></> : info.orderCategories.map((theme) => {
        return(<div className="field-card-tag" key={theme.id}>{formatTagValue(theme.name)}</div>);
    })

    competencies = info === null || info.orderSkills === undefined  ? <></> : info.orderSkills.map((comp) => {
        return(<div className="field-card-tag" key={comp.id}>{formatTagValue(comp.name)}</div>);
    })

    function getButtonAtStart(info) {
        if (window.sessionStorage.getItem('role') === undefined)
            return responseButtonNotAuth;
        if (window.sessionStorage.getItem('role') === "executor_role") {
            if (!info.isResponded)
                return responseButton;
            else {
                if (info.isAccepted)
                    return info.isCompleted ? finishedButton : finishButton;
                else return cancelButton;
            }
        }
        return <></>
    }

    function viewPopup() {
        setPopup(<div className="popup-wrapper">
            <div className="popup finish-project-popup">
                <img className="finish-project-popup-logo" src={logoBlack}/>
                <div className="finish-project-popup column-1">
                    <div id={"popup-text"}>
                        <h2>Закончить проект?</h2>
                        <p>К сожалению, на нашей платформе пока нет инструментов для отправки проектов.</p>
                        <p>Свяжитесь со своим заказчиком и передайте ему вашу работу.</p>
                        <p>После передачи работы нажмите на кнопку “Завершить проект” — это откроет возможность
                            заказчику написать отзыв на вашу работу.</p>
                        <p>Мы обещаем создать удобный инструментарий для отправки проектов в дальнейшем :)</p>
                    </div>
                    <div id={"popup-finish-error"} hidden>
                        <h2>Что-то пошло не так...</h2>
                        <p id={"error-text"}></p>
                    </div>
                    <div className="popup-buttons buttons">
                        <a id="finish-button" className="project-card-btn" onClick={() => {finishOrder()}}>Закончить проект</a>
                        <a id="cancel-close-button" className="project-card-btn"onClick={hidePopup}>Отмена</a>
                    </div>
                </div>
            </div>
        </div>)
    }

    function viewResponsePopup() {
        setPopup(<div className="popup-wrapper">
            <div className="popup popup leave-account-popup">
                <img className="finish-project-popup-logo" src={logoBlack}/>
                    <h2 className="leave-text">Отправить заявку на принятие в заказ?</h2>

                    <div className="popup-buttons buttons">
                        <a className="project-card-btn" onClick={() => {response()}}>Отправить</a>
                        <a className="project-card-btn" onClick={hidePopup}>Отмена</a>
                    </div>
            </div>
        </div>)
    }

    function viewCancelResponsePopup() {
        setPopup(<div className="popup-wrapper">
            <div className="popup leave-account-popup">
                <img className="finish-project-popup-logo" src={logoBlack}/>

                    <h2 className="leave-text">Отменить заявку на принятие в заказ?</h2>

                    <div className="popup-buttons buttons">
                        <a className="project-card-btn" onClick={() => {cancelResponse()}}>Отменить</a>
                        <a className="project-card-btn" onClick={() => hidePopup}>Отмена</a>
                    </div>

            </div>
        </div>)
    }

    function hidePopup() {
        setPopup(<></>)
    }
    function finishOrder(e) {
        axios.post(process.env.REACT_APP_API_URL+"/api/Executor/finish_project/" + params.id, null,
            {
                headers:
                    {'Access-Control-Allow-Origin' : '*',
                        'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization' : "Bearer " + window.sessionStorage.getItem('token')
                    },

            })
            .then((response) => {
                console.log("project " + params.id + " finished")
                hidePopup();
                setButton(finishedButton);
            })
            .catch((error) => {
                console.log(error)
                document.getElementById("popup-text").hidden = true;
                document.getElementById("popup-finish-error").hidden = false;
                document.getElementById("error-text").textContent = error.response.data;

            });
    }

    function response() {
        axios.post(process.env.REACT_APP_API_URL+"/api/Order/response/" + params.id, null,
            {
                headers:
                    {'Access-Control-Allow-Origin' : '*',
                        'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization' : "Bearer " + window.sessionStorage.getItem('token')
                    },

            })
            .then((response) => {
                console.log("project " + params.id + " response")
                setButton(cancelButton)
                hidePopup()
            })
            .catch((error) => {console.log(error)});
    }

    function cancelResponse() {
        axios.post(process.env.REACT_APP_API_URL+"/api/Order/response/" + params.id, null,
            {
                headers:
                    {'Access-Control-Allow-Origin' : '*',
                        'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization' : "Bearer " + window.sessionStorage.getItem('token')
                    },
                    params: {
                        reaction : false
                    },

            })
            .then((response) => {
                console.log("project " + params.id + " response")
                setButton(responseButton)
                hidePopup()
            })
            .catch((error) => {console.log(error)});
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
                            {competencies}
                        </div>
                    </div>}

                {/*{window.sessionStorage.getItem('role') === undefined &&*/}
                {/*    <a className="finish-project project-card-btn" href="/signin">Отправить заявку</a>*/}
                {/*}*/}
                {/*{window.sessionStorage.getItem('role') === "executor_role" ?*/}
                {/*    !props.info.isResponded ?*/}
                {/*        <button className="finish-project project-card-btn" onClick={(e) => {*/}
                {/*            viewResponsePopup(e);*/}
                {/*        }}>Отправить заявку</button> :*/}
                {/*        props.info.isAccepted ?*/}
                {/*            <button className="finish-project project-card-btn" onClick={(event) => viewPopup(event)}>Завершить заказ</button> :*/}
                {/*            <button className="finish-project project-card-btn" onClick={(event) => cancelResponse(event)}>Отменить заявку</button> :*/}
                {/*    <></>*/}
                {/*}*/}
                {button}

            </section>
        </>
    )
}

export default BaseInfoForExecutor