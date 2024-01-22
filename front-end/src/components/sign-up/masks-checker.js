import IMask from 'imask'

let phoneMask;
let usernameMask;
let innMask;
let currentLengthNum = 10;
document.onchange = () => {
    initializeMasks();
}

document.onreadystatechange = () => {
    initializeMasks();
}

export function initializeMasks() {
    if(document.readyState === 'complete'){
        phoneMask = document.getElementById('phone') !== null ? new IMask(document.getElementById('phone'), {mask: "+{7} (000) 000-00-00"}) : null;
        usernameMask = document.getElementById('username') !== null ? 
            new IMask(document.getElementById('username'), {mask: '{@}####[#][#][#][#][#][#][#][#][#][#][#][#][#][#][#][#]', definitions: {'#': /[a-zA-Z0-9_]/}}) : null;
        innMask = document.getElementById('inn') !== null ? new IMask(document.getElementById('inn'), {mask : '000-000-0000', lazy: false}) : null;
    }
}

export function checkMask(maskName) {
    
    if (phoneMask !== null && maskName === 'phone'){
        return phoneMask.masked.isComplete;
    }

    if (usernameMask !== null && maskName === 'username')
        return usernameMask.masked.isComplete;

    if (innMask !== null && maskName === 'inn')
        return innMask.masked.isComplete;
}

export function getMaskValue(maskName) {
    if (phoneMask !== null && maskName === 'phone'){
        return phoneMask.masked.unmaskedValue;
    }

    if (usernameMask !== null && maskName === 'username')
        return usernameMask.masked.unmaskedValue;

    if (innMask !== null && maskName === 'inn')
        return innMask.masked.unmaskedValue;
}

//TODO: исправить функцию, при переключении не изменяется маска
export function updateInnMask() {
    if (currentLengthNum === 10){
        let value = innMask.value;
        innMask.destroy();
        innMask = document.getElementById('inn') !== null ? new IMask(document.getElementById('inn'), {mask : '0000-0000-0000', lazy: false}) : null;
        innMask.value = value;
        currentLengthNum = 12;
    }
    else {
        let value = innMask.value;
        innMask.destroy();
        innMask = document.getElementById('inn') !== null ? new IMask(document.getElementById('inn'), {mask : '000-000-0000', lazy: false}) : null;
        innMask.value = value;
        currentLengthNum = 10;
    }

}

 

