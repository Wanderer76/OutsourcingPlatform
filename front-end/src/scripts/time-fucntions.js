export function getTimeFromDate(stringifyDate) {
    let date;
    if (typeof stringifyDate === "string")
        date = new Date(stringifyDate)
    else date = stringifyDate;
    return date.getHours() + ":" + (date.getMinutes()<10 ? '0':'')+date.getMinutes();
}

export function setDateOrTimeIfToday(date) {
    let now = new Date();
    if (date.getFullYear() === now.getFullYear()) {
        if (date.getMonth() === now.getMonth() && now.getDate() - date.getDate() <= 1) {
            if (date.getDate() === now.getDate()) {
                return getTimeFromDate(date);
            }
            else if (now.getDate() - date.getDate() === 1)
                return "Вчера";
        }
        else
            return date.toLocaleString("ru", {day: 'numeric', month: 'short'});
    }
    else return date.toLocaleString("ru", {year: 'numeric', month: 'numeric'})
}

export function setDate(dateForConvert) {
    let date;
    if (typeof dateForConvert === "string")
        date = new Date(dateForConvert)
    else date = dateForConvert;
    let now = new Date();
    if (date.getFullYear() === now.getFullYear()) {
        if (date.getMonth() === now.getMonth() && now.getDate() - date.getDate() <= 1) {
            if (date.getDate() === now.getDate()) {
                return "Сегодня";
            }
            else if (now.getDate() - date.getDate() === 1)
                return "Вчера";
        }
        else
            return date.toLocaleString("ru", {day: 'numeric', month: 'long'});
    }
    else return date.toLocaleString("ru", {year: 'numeric', month: 'numeric'})
}