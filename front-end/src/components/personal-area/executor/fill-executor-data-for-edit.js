export function fillExecutorDataForEdit(data) {
    document.getElementById("surname").value = data.surname;
    document.getElementById("name").value = data.name;
    if (data.secondName === "" || data.secondName === null) {
        document.getElementById('no-second-name').checked = true;
        document.getElementById('second-name').classList.toggle('visually-hidden');
    }
    else
        document.getElementById("second-name").value = data.secondName;

    document.getElementById("birth-date").value = data.executor.birthDate.split('T')[0];
    document.getElementById("city").value = data.executor.city;
    document.getElementById("phone").value = data.phone;
    document.getElementById("email").value = data.email;
    if (data.contacts !== null)
        document.getElementById('person-description').value = data.contacts.about
}

