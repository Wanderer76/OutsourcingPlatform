import {setDate} from "../../scripts/time-fucntions";

export function sortMessages(messages) {
    // console.log(messages)
    let messages_dict = {}
    for (let message of messages) {
        let date = new Date(message.createdAt);
        date.setHours(0, 0, 0, 0);
        if (date in messages_dict)
            messages_dict[date].push(message)
        else {
            messages_dict[date] = []
            messages_dict[date].push(message)
        }
    }
    let sorted = Object.entries(messages_dict)
    sorted.sort((a, b) => {
        let firstDate = new Date(a[0])
        let secondDate = new Date(b[0])
            return new Date(a[0]) - new Date(b[0])
    })
    for (let i=0; i<sorted.length; i++) {
        sorted[i][1].sort((a, b) => {
            // console.log(a)
            // console.log(b)

            return a['id'] - b['id'];
        })
        sorted[i][0] = setDate(sorted[i][0]);
    }
    // console.log(sorted);
    return sorted;
}