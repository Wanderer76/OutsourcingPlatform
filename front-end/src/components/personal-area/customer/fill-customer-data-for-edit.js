export function fillCustomerDataForEdit(res) {
    if (res === undefined){
        console.log("нет данных");
        return
    }
    console.log(res);
    document.getElementById("surname").value = res.surname;
    document.getElementById("name").value = res.name;
    let secondName = res.secondName;
    if (secondName !== "" && secondName !== null)
        document.getElementById("second-name").value = res.secondName;
    else
        document.getElementById("no-second-name").checked = true;
    document.getElementById("company-name").value = res.customer.companyName;
    document.getElementById("address").value = res.customer.address;
    document.getElementById("inn").value = res.customer.inn;
    document.getElementById("phone").value = res.phone;
    document.getElementById("email").value = res.email;
    if (res.contacts !== null)
        document.getElementById("person-description").value = res.contacts.about;



}