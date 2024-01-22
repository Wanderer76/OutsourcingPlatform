export function addElement(selectorId, arr, setArr) {

    let i = document.getElementById(selectorId)
    let element = i.value;
    let id = i.options[i.selectedIndex].name;
    if (element !== "" && checkArray(arr, element))
        setArr([{'id': -1, 'name': element}, ...arr]);
}

export function checkArray(arr, value) {
    for (let element of arr)
        if (!('name' in element) || element.name === value)
            return false
    return true
}

export function deleteElement(element, arr, setArr) {
    setArr(arr.filter(useElement => useElement !== element));
}