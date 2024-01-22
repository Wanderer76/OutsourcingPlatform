import {useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import {formatTagValue} from "../../format-tag";
import logoBlack from "../../../../images/logo-black.svg";
import axios from "axios";

const BaseInfoForExecutor = (props) => {
    const params = useParams();
    const [popup, setPopup] = useState(<></>)

    let info = props.info;

    const cancelButton = <button className="finish-project project-card-btn"
                                 onClick={() => viewCancelResponsePopup()}>Отменить заявку</button>;
    const finishButton = <button className="finish-project project-card-btn" onClick={(event) => viewPopup()}
                                 disabled={info.isCompleted}>Завершить заказ</button>;
    //const finishedButton = <button className="finish-project project-card-btn" disabled={true}>Заказ завершен</button>;

    const unfinishedButton = <button className="finish-project project-card-btn" onClick={
        e => {
            viewUnfinishPopup()
        }
    }>Отменить
        завершение заказа</button>;


    const responseButton = info.orderVacancies.map(vacancy => (
        <div key={vacancy.orderRole.id} className={'last-row'}>
            <div className="field-card-tag">{formatTagValue(vacancy.orderRole.name)}
            </div>
            <br></br><p
            className={"field-card-tag"}>{"Места " + (vacancy.responses === null ? 0 : vacancy.responses.length) + "/" + vacancy.maxWorkers}</p>
            {vacancy.responses === null ? <button className="project-card-btn" onClick={(e) => {
                viewResponsePopup(vacancy.id)
            }}>Отправить заявку
            </button> : vacancy.responses.length < vacancy.maxWorkers ?
                <button className="project-card-btn" onClick={(e) => {
                    viewResponsePopup(vacancy.id)
                }}>Отправить заявку
                </button> : null}
        </div>))

    const responseButtonNotAuth = <a className="finish-project project-card-btn" href="/signin">Отправить заявку</a>;
    const [button, setButton] = useState(getButtonAtStart(info))


    console.log("info: ");
    console.log(info);
    let themes;
    let competencies;
    let orderRoles;
    themes = info === null || info.orderCategories === undefined ? <></> : info.orderCategories.map((theme) => {
        return (<div className="field-card-tag" key={theme.id}>{formatTagValue(theme.name)}</div>);
    })

    competencies = info === null || info.orderSkills === undefined ?
        <></> :
        !info.isResponded ? info.orderSkills.map((comp) => {
                return (<div className="field-card-tag" key={comp.id}>{formatTagValue(comp.name)}</div>)
            }) :
            info.orderSkills.filter(x => x.responses
                != null).map((comp) => {
                return (<div className="field-card-tag" key={comp.id}>{formatTagValue(comp.name)}</div>)
            });
    orderRoles = info === null || info.orderVacancies === undefined ?
        <></> :
        !info.isResponded ? info.orderVacancies.map((comp) => {
                return (<div className="field-card-tag" key={comp.orderRole.id}>{formatTagValue(comp.orderRole.name)}</div>)
            }) :
            info.orderVacancies.filter(x => x.responses
                != null).map((comp) => {
                return (
                    <div className="field-card-tag" key={comp.orderRole.id}>{formatTagValue(comp.orderRole.name)}</div>)
            });

    function getButtonAtStart(info) {
        if (window.sessionStorage.getItem('role') === undefined)
            return responseButtonNotAuth;
        if (window.sessionStorage.getItem('role') === "executor_role") {
            if (!info.isResponded)
                return responseButton;
            else {
                if (info.isAccepted)
                    return info.isCompleted ? unfinishedButton : finishButton;
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
                        <p>Вы можете приложить файл в формате <span>.zip или .rar</span></p>
                        <p>Или свяжитесь со своим заказчиком и передайте ему вашу работу.</p>
                        <p>После передачи работы нажмите на кнопку “Завершить проект” — это откроет возможность
                            заказчику написать отзыв на вашу работу.</p>
                        <input id={'file-input'} type={'file'} accept={'.zip,.rar'}/>
                    </div>
                    <div id={"popup-finish-error"} hidden>
                        <h2>Что-то пошло не так...</h2>
                        <p id={"error-text"}></p>
                    </div>
                    <div className="popup-buttons buttons">
                        <a id="finish-button" className="project-card-btn" onClick={async () => {
                            await finishOrder(document.getElementById('file-input').files[0])
                        }}>Закончить проект</a>
                        <a id="cancel-close-button" className="project-card-btn" onClick={hidePopup}>Отмена</a>
                    </div>
                </div>
            </div>
        </div>)
    }

    function viewUnfinishPopup() {
        setPopup(<div className="popup-wrapper">
            <div className="popup finish-project-popup">
                <img className="finish-project-popup-logo" src={logoBlack}/>
                <div className="finish-project-popup column-1">
                    <div id={"popup-text"}>
                        <h2>Отменить завершение проекта?</h2>
                    </div>
                    <div id={"popup-finish-error"} hidden>
                        <h2>Что-то пошло не так...</h2>
                        <p id={"error-text"}></p>
                    </div>
                    <div className="popup-buttons buttons">
                        <a id="finish-button" className="project-card-btn" onClick={async () => {
                            await axios.post(process.env.REACT_APP_API_URL + "/api/Executor/finish_project/" + params.id, null,
                                {
                                    headers:
                                        {
                                            'Access-Control-Allow-Origin': '*',
                                            'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                                            'Authorization': "Bearer " + window.sessionStorage.getItem('token'),
                                        },
                                    params: {
                                        isFinish: false
                                    },
                                })
                                .then((response) => {
                                    console.log("project " + params.id + " finished")
                                    hidePopup();
                                    setButton(finishButton);
                                })
                        }}>Возобновить проект</a>
                        <a id="cancel-close-button" className="project-card-btn" onClick={hidePopup}>Отмена</a>
                    </div>
                </div>
            </div>
        </div>)
    }

    function viewResponsePopup(vacancyId) {
        setPopup(<div className="popup-wrapper">
            <div className="popup popup leave-account-popup">
                <img className="finish-project-popup-logo" src={logoBlack}/>
                <h2 className="leave-text">Отправить заявку на принятие в заказ?</h2>

                <div className="popup-buttons buttons">
                    <a className="project-card-btn" onClick={() => {
                        response(vacancyId)
                    }}>Отправить</a>
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
                    <a className="project-card-btn" onClick={() => {
                        cancelResponse()
                    }}>Отменить заявку</a>
                    <a className="project-card-btn" onClick={() => hidePopup()}>Закрыть</a>
                </div>

            </div>
        </div>)
    }

    function hidePopup() {
        setPopup(<></>)
    }

    const headers =
        {
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
            'Authorization': "Bearer " + window.sessionStorage.getItem('token')
        }


    async function finishOrder(file) {
        await axios.post(process.env.REACT_APP_API_URL + "/api/Executor/finish_project/" + params.id, null,
            {
                params: {
                    isFinish: true
                },
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token'),
                    },

            })
            .then((response) => {
                console.log("project " + params.id + " finished")
                setButton(unfinishedButton);
            })
            .catch((error) => {
                console.log(error)
                document.getElementById("popup-text").hidden = true;
                document.getElementById("popup-finish-error").hidden = false;
                document.getElementById("error-text").textContent = error.response.data;

            });
        if (file !== null) {
            var formData = new FormData();
            formData.append('projectFile', file)
            await axios.post(process.env.REACT_APP_API_URL + "/files/ObjectStorage/upload-project-file/" + props.info.orderId + '/' + params.id, formData, {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token'),
                        'Content-Type': 'multipart/form-data'
                    },
            }).then(async response => {
                await axios.post(process.env.REACT_APP_API_URL + "/api/Executor/set_file_exists/" + params.id, null,
                    {
                        headers:
                            {
                                'Access-Control-Allow-Origin': '*',
                                'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                                'Authorization': "Bearer " + window.sessionStorage.getItem('token'),
                            },
                    })
            }).catch(error => {
                console.log(error)
            })
        }
        hidePopup();

    }

    function response(vacancyId) {
        axios.post(process.env.REACT_APP_API_URL + "/api/Order/response/" + params.id, null,
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    },
                params: {
                    vacancyId: vacancyId
                }
            })
            .then((response) => {
                console.log("project " + params.id + " response")
                setButton(cancelButton)
                hidePopup()
            })
            .catch((error) => {
                console.log(error)
                alert(error.response.data)
            });
    }

    function cancelResponse() {
        axios.post(process.env.REACT_APP_API_URL + "/api/Order/response/" + params.id, null,
            {
                headers:
                    {
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                        'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                    },
                params: {
                    reaction: false,
                },

            })
            .then((response) => {
                console.log("project " + params.id + " response")
                setButton(responseButton)
                hidePopup()
            })
            .catch((error) => {
                alert(error.response.data)
                console.log(error)
            });
    }

    return (
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
                    <div className="project-card-tags fields-row">
                        <h3>Компетенции:</h3>
                        <div className="project-card-tags tags-fields">
                            {competencies}
                        </div>
                    </div>
                }
                {info.orderVacancies !== null && info.orderVacancies.length !== 0 &&
                    <div className="project-card-tags skills-row">
                        {!info.isResponded && <h3>Роли:</h3>}
                        {info.isResponded && <h3>Отклик на роль:</h3>}
                        <div className="project-card-tags tags-skills">
                            {orderRoles}
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