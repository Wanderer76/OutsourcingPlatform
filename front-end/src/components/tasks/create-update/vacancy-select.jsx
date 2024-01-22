import {addElement, checkArray, deleteElement, editElement} from "../add-check-delete-tags";
import {removeErrorMessage, removeErrorMessageForInputs, sendData} from "./valid-and-send-new-project-data";
import React, {useState} from "react";

export default function SelectVacancy({orderRoles, setOrderRoles, orderRolesView, currentValue, showDelete}) {
    const [maxWorkers, setMaxWorkers] = useState(1);
    return (
        <>
            <div style={{float: "left", paddingTop: "10px"}}>
                <div className="new-project__field project-participant-number">
                    <select className="new-project-select" name="project-new-role"
                            id={"project-new-role" + currentValue.id}
                            onChange={() => {
                                const i = document.getElementById('project-new-role' + currentValue.id)
                                const element = i.value;
                                orderRoles = orderRoles.filter(x => x.id !== currentValue.id);
                                orderRoles.push({
                                    'id': currentValue.id,
                                    'name': element,
                                    'maxWorkers': maxWorkers
                                })
                                setOrderRoles(orderRoles)

                                removeErrorMessage('project-new-role-error');
                            }
                            }>

                        {currentValue && <option value="">{currentValue.name}</option>}
                        {orderRolesView}
                    </select>
                </div>
            </div>
            <div style={{float: "left", marginLeft: "10px", paddingTop: "10px"}}>
                <input type="number" min={'1'} max={'100'} maxLength={3}
                       className="new-project__field project-participant-number"
                       name="maxWorkers" id="project-participant-number"
                       placeholder={'количество человек'}
                       alt={'количество человек'}
                       defaultValue={currentValue.maxWorkers}
                       onChange={(x) => {

                           orderRoles = orderRoles.filter(x => x.id !== currentValue.id);
                           orderRoles.push({
                               'id': currentValue.id,
                               'name': currentValue.name,
                               'maxWorkers': x.target.value
                           },)
                           setOrderRoles(orderRoles)

                       }}
                       onKeyDown={(evt) => ["e", "E", "+", "-"].includes(evt.key) && evt.preventDefault()}
                       onInput={e => {
                           console.log(e.target.value);
                           e.target.parentElement.classList.remove("error");
                           document.getElementById('project-new-role-error').classList.add("visually-hidden")
                           if (e.key ==='e')
                               return
                           if (!/[0-9]/.test(e.key) || Number.parseInt(e.target.value) > 99) {
                               e.target.value.replace(/\D/g, "")
                           }

                           if (e.target.value > 100)
                               e.target.value = '100'
                       }}/>
            </div>
            {showDelete ? <div style={{float: "left", marginLeft: "10px", paddingTop: "10px"}}>
                <input type="button"
                       className={"new-project-btn tag-item-remove"}
                       value="Удалить"
                       id={currentValue.id}
                       onClick={() => {
                           deleteElement(currentValue, orderRoles, setOrderRoles);
                       }
                       }
                />
            </div> : null}
        </>
    )

}