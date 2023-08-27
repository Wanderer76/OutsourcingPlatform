import IMask from "imask";

let usernameMask;
document.onreadystatechange = () => {
    usernameMask = document.getElementById('user-email') !== null ?
        new IMask(document.getElementById('user-email'), {mask: '{@}####[#][#][#][#][#][#][#][#][#][#][#][#][#][#][#][#]', definitions: {'#': /[a-zA-Z0-9_]/}}) : null;
}

document.onchange = () => {
    usernameMask = document.getElementById('user-email') !== null ?
        new IMask(document.getElementById('user-email'), {mask: '{@}####[#][#][#][#][#][#][#][#][#][#][#][#][#][#][#][#]', definitions: {'#': /[a-zA-Z0-9_]/}}) : null;
}
export function validateSigninData() {
    let isRight = true;
    if (usernameMask !== null && !usernameMask.masked.isComplete) {
        isRight = false;
        document.getElementById("login-field").classList.add("error");
    }

    let pass = document.getElementById("user-password");
    if (pass.value.length < 6) {
        document.getElementById("password-field").classList.add("error");
    }

    return isRight;


}