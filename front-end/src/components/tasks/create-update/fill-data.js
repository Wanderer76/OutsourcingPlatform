export function fillData(data, setViewParticipantCount, clearCompetence) {

    document.getElementById("project-name").value = data.name;
    document.getElementById("project-description").value = data.description;
    let date = new Date(data.deadline);
    console.log(date.toLocaleDateString())
    document.getElementById("project-deadline").value = data.deadline;
    if (data.maxWorkers !== -1) {
        document.getElementById("project-participant-number").value = data.maxWorkers;
    }
    else {
        setViewParticipantCount(false);
        document.getElementById("project-participant-number").checked = true;
    }

    if (data.orderSkills === null || data.orderSkills.length === 0) {
        document.getElementById("no-competencies").checked = true;
        clearCompetence(document.getElementById("no-competencies"));
    }
}