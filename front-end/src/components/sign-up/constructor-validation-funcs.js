import { checkMask, getMaskValue } from "./masks-checker";

function checkEmpty(form, minLenght = 1) {
    if (form.value.length < minLenght){
        form.parentElement.classList.add("error");
        return false;
    }
    return true;
}

function checkRepeatPass(pass, repeatPass) {
    if (pass.value !== repeatPass.value) {
        repeatPass.parentElement.classList.add("error");
        return false;
    }
    return true;
}

function checkEmptyWithChecker(form, checker) {
    if ((form.value === '' && !checker.checked) || (form.value !== '' && checker.checked)){
        form.parentElement.classList.add("error");
        return false;
    }
    
    return true;
}

function checkMasked(input, maskName, errorElement) {
    if (!checkMask(maskName)) {
        input.parentElement.classList.add("error");
        console.log(input.value === '' || input.value === null);
        if (input.value === '' || input.value === null)
            errorElement.textContent = "Заполните поле";
        else
            errorElement.textContent = "Некорректно заполнено поле";

        return false;
    }
    return true;
}

function removeErrorClass() {
    this.parentElement.classList.remove("error");
}

function removeErrorClassOtherElement(form) {
    form.parentElement.classList.remove("error");
}

function checkEmail(form) {
    const regex = new RegExp("[a-zA-Z0-9_-]{2,}@[a-zA-Z0-9_-]{2,}\.[a-zA-Z]{2,}");
    if (form.value==='') {
        form.parentElement.classList.add("error");
        document.getElementById('email-error').textContent = "Заполните поле";
        return false;
    }

    if (!regex.test(form.value)) {
        form.parentElement.classList.add("error");
        document.getElementById('email-error').textContent = "Введите правильный E-mail";
        return false;
    }

    return true;
}

function toogleType(inputForm) {
    if (inputForm.type === 'password') {
        inputForm.type = 'text';
    } else {
        inputForm.type = 'password';
    }
}

export function showPassword() {
    toogleType(document.getElementById("user-password"));
    toogleType(document.getElementById("user-password-repeat"));
}

function checkInn(input, errorElement) {
    if (!checkMasked(input, 'inn', errorElement))
        return false;

    let inn = getMaskValue('inn');
    console.log(inn);
    if ( inn.length === 10 )
    {
        console.log("result: " + String(((2*inn[0] + 4*inn[1] + 10*inn[2] +
            3*inn[3] + 5*inn[4] +  9*inn[5] +
            4*inn[6] + 6*inn[7] +  8*inn[8]
            ) % 11) % 10));
        if( inn[9] !== String(((2*inn[0] + 4*inn[1] + 10*inn[2] +
                                3*inn[3] + 5*inn[4] +  9*inn[5] +
                                4*inn[6] + 6*inn[7] +  8*inn[8]
                                ) % 11) % 10)){
            input.parentElement.classList.add("error");
            errorElement.textContent = "Некорректно заполнено поле";
            return false
        }
    }
    else if ( inn.length === 12 )
    {
        if (inn[10] !== String(((
             7*inn[0] + 2*inn[1] + 4*inn[2] +
            10*inn[3] + 3*inn[4] + 5*inn[5] +
             9*inn[6] + 4*inn[7] + 6*inn[8] +
             8*inn[9]
        ) % 11) % 10) && inn[11] === String(((
            3*inn[0] +  7*inn[1] + 2*inn[2] +
            4*inn[3] + 10*inn[4] + 3*inn[5] +
            5*inn[6] +  9*inn[7] + 4*inn[8] +
            6*inn[9] +  8*inn[10]
        ) % 11) % 10)) {
            input.parentElement.classList.add("error");
            errorElement.textContent = "Некорректно заполнено поле";
            return false
        };
    }
    return true;
}

export function checkAllData() {
    let result_data = {}
    let all_right = true;

    console.clear();
    let surnameForm = document.getElementById('surname');
    let nameForm = document.getElementById('name');
    let secondNameForm = document.getElementById('second-name');
    let birthdateForm = document.getElementById('birth-date');
    let city = document.getElementById('city');
    let phone = document.getElementById("phone");
    let username = document.getElementById("username");
    let email = document.getElementById("email");
    let pass = document.getElementById("user-password");
    let passRepeat = document.getElementById("user-password-repeat");
    let companyName = document.getElementById("company-name");
    let address = document.getElementById("address");
    let inn = document.getElementById("inn");

    if (!checkEmpty(surnameForm)){
        surnameForm.oninput = removeErrorClass;
        all_right = false;
        console.log('surname error');
    }
    else {
        result_data['surname'] = surnameForm.value;
    }

    if (!checkEmpty(nameForm)){
        nameForm.oninput = removeErrorClass;
        all_right = false;
        console.log('name error');
    }
    else {
        result_data['name'] = nameForm.value;
    }

    if (!checkEmptyWithChecker(secondNameForm, document.getElementById('no-second-name'))){
        secondNameForm.oninput = removeErrorClass;
        document.getElementById('no-second-name').onchange = () => {removeErrorClassOtherElement(secondNameForm)};
        all_right = false;
        console.log('second name error');
    }
    else {
        result_data['secondName'] = secondNameForm.value;
    }

    if (!(checkMasked(phone, "phone", document.getElementById('phone-error')))){
        phone.oninput = removeErrorClass;
        all_right = false;
        console.log('phone error');
    }
    else {
        result_data['phone'] = phone.value;
    }

    if (!checkMasked(username, "username", document.getElementById('username-error'))){
        username.oninput = removeErrorClass;
        all_right = false;
        console.log('username error');
    }
    else {
        result_data['username'] = username.value;
    }

    if (!checkEmail(email)){
        email.oninput = removeErrorClass;
        all_right = false;
        console.log('email error');
    }
    else {
        result_data['email'] = email.value;
    }

    if (!checkEmpty(pass, 6)){
        pass.parentElement.parentElement.classList.add("error")
        document.getElementById("password-error").classList.remove("visually-hidden")
        pass.oninput = () => {
            pass.parentElement.parentElement.classList.remove("error");
            document.getElementById("password-error").classList.add("visually-hidden")
        }
        all_right = false;
        console.log('pass error');
    }
    else {
        result_data['password'] = pass.value;
    }

    if (!checkRepeatPass(pass, passRepeat)){
        passRepeat.parentElement.parentElement.classList.add("error")
        document.getElementById("password-repeat-error").classList.remove("visually-hidden")
        passRepeat.oninput = () => {
            passRepeat.parentElement.parentElement.classList.remove("error");
            document.getElementById("password-repeat-error").classList.add("visually-hidden")
        }
        all_right = false;
        console.log('pass repeat error');
    }

    //поля для исполнителя
    if (birthdateForm !== null && !checkEmpty(birthdateForm)){
        birthdateForm.onchange = removeErrorClass;
        all_right = false;
        console.log('birthdate error');
    }
    else {
        if (birthdateForm !== null) result_data['birthDate'] = birthdateForm.value;
    }

    if (city  !== null && !checkEmpty(city)){
        console.log('city checked')
        city.oninput = removeErrorClass;
        all_right = false;
        console.log('city error');
    }
    else {
        if (city !== null) result_data['city'] = city.value;
    }

    //поля для заказчика
    if (address  !== null && !checkEmpty(address)){
        address.oninput = removeErrorClass;
        all_right = false;
        console.log('address error');
    }
    else {
        if (address !== null) result_data['address'] = address.value;
    }

    if (companyName  !== null && !checkEmpty(companyName)){
        companyName.oninput = removeErrorClass;
        all_right = false;
        console.log('company error');
    }
    else {
        if (companyName !== null) result_data['organizationName'] = companyName.value;
    }

    if (inn !== null && !checkInn(inn, document.getElementById('inn-error'))){
        inn.oninput = removeErrorClass;
        all_right = false;
        console.log('inn error');
    }
    else {
        if (inn !== null) result_data['inn'] = inn.value.replace('-', '').replace('-', '');
    }


    if (all_right)
        return result_data;

    return null;

}

