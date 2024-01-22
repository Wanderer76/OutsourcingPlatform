import axios from "axios";
import React from "react";

function validData(themes, competence, orderRoles) {
    console.log(...competence);
    let data = {}
    let textareas = document.getElementsByTagName('textarea');
    let inputs = document.getElementsByTagName('input');

    let isRight = checkElementsArrayAndAppendToData(textareas, data);
    isRight = checkElementsArrayAndAppendToData(inputs, data);

    if (themes !== undefined && themes.length !== 0)
        data.orderCategories = themes.map(x=>({id:x.id,name:x.name}));
    else {
        isRight = false;
        let errorMessage = document.getElementById('project-new-field-error');
        errorMessage.classList.remove('visually-hidden');
        errorMessage.parentElement.classList.add('error');
    }
    if ((competence !== undefined && themes.length !== 0) || document.getElementById('no-competencies').checked)

        data.orderSkills =
            competence.map(x=>({id:x.id,name:x.name}));
    else {
        isRight = false;
        let errorMessage = document.getElementById('project-new-skill-error');
        errorMessage.classList.remove('visually-hidden');
        errorMessage.parentElement.classList.add('error');
    }

    if (!orderRoles || orderRoles.length === 0) {
        isRight = false;
        let errorMessage = document.getElementById('project-new-role-error');
        errorMessage.classList.remove('visually-hidden');
        errorMessage.parentElement.classList.add('error');
    } else {
        data.orderVacancies =
            orderRoles.map(x => ({
                maxWorkers: x.maxWorkers,
                orderRole: {
                    name: x.name
                }
            }));
    }

    console.log(data)
    return isRight ? data : isRight;
}

function checkElementsArrayAndAppendToData(arr, data) {
    let isRight = true;
    for (const element of arr) {
        if (element.value === null || element.value === "") {
            isRight = false;
            element.parentElement.classList.add("error");
            element.parentElement.children[3].classList.remove("visually-hidden");
            if (element.type === "date")
                element.parentElement.children[3].textContent = "Укажите срок выполнения проекта";
        } else if (element.type === "date" && new Date(element.value) - new Date() < 0) {
            isRight = false;
            element.parentElement.classList.add("error");
            element.parentElement.children[3].classList.remove("visually-hidden");
            element.parentElement.children[3].textContent = "Указана прошедшая дата";
        } else if (element.type === "number" && element.value <= 0) {
            isRight = false;
            element.parentElement.classList.add("error");
            element.parentElement.children[3].classList.remove("visually-hidden");
            element.parentElement.children[3].textContent = "Число участников не может быть отрицательным";
        } else if (element.type !== "button" && element.type !== "checkbox")
            data[element.name] = element.value;
    }

    return isRight;
}

export function removeErrorMessageForInputs(e) {
    console.log(e.target);
    e.target.parentElement.classList.remove("error");
    e.target.parentElement.children[3].classList.add("visually-hidden");
}

export function removeErrorMessage(blockId) {
    let errorMessage = document.getElementById(blockId);
    errorMessage.classList.add('visually-hidden');
    errorMessage.parentElement.classList.remove('error');
}

export function sendData(themes, competence, orderRoles, url, orderId = -1) {
    let data = validData(themes, competence, orderRoles);
    if (data !== false)
        console.log("Data correct, congratulate you, thumbs)");
    else {
        console.log("Data incorrect, loser!");
        return;
    }
    data.price = 0;
    console.log(data);
    if (!("maxWorkers" in data))
        data.maxWorkers = -1;
    delete data[""];
    axios.post(url, data,
        {
            headers:
                {
                    'Access-Control-Allow-Origin': '*',
                    'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,PATCH,OPTIONS',
                    'Authorization': "Bearer " + window.sessionStorage.getItem('token')
                },

        })
        .then((response) => {
            console.log(response);
            window.location.href = orderId !== -1 ? "/tasks/my/" + orderId : "/personal-area";
        })
        .catch((error) => {
            console.log(error)
        });
}