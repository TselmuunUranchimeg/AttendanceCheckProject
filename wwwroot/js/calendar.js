let today = new Date();
let currentYear = today.getFullYear();
let currentMonth = today.getMonth();

const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

const daysInMonth = (year, month) => {
    return 32 - new Date(year, month, 32).getDate();
}

const next = () => {
    currentMonth = currentMonth === 11 ? 0 : currentMonth+1;
    currentYear = currentMonth === 11 ? currentYear + 1 : currentYear;
    showCalendar(currentYear, currentMonth);
}

const back = () => {
    currentMonth = currentMonth === 0 ? 11 : currentMonth - 1;
    currentYear = currentMonth === 0 ? currentYear - 1 : currentYear;
    showCalendar(currentYear, currentMonth);
}

const showCalendar = async (year, month) => {
    let tableBody = document.getElementById("calendar-body");
    tableBody.innerHTML = "";
    let employeeId = window.location.pathname.split("/")[3];
    let datesState = {};
    let data = await (await fetch(`/AttendanceData/${employeeId}/${currentYear}/${currentMonth+1}`, { method: "Get" })).json();
    data.forEach((val) => {
        let date = new Date(val.date);
        let state = false;
        if (data.status === "Checked in" || data.status === "Checked out") {
            state = true;
        }
        if (datesState[date.getDate()] !== undefined) {
            if (!state && datesState[date.getDate()] === true) {
                datesState[date.getDate()] = state;
            }
        } else {
            datesState[date.getDate()] = state;
        }
    });

    let firstDay = new Date(year, month).getDay();
    let monthAndYear = document.getElementById("monthAndYear");
    monthAndYear.innerText = months[currentMonth] + ", " + currentYear;
    currentYear = year;
    currentMonth = month;
    let date = 1;
    for (let i = 0; i < 6; i++)
    {
        let row = document.createElement("tr");
        for (let j = 0; j < 7; j++) {
            if (i === 0 && j < firstDay) {
                let cell = document.createElement("td");
                cell.append(document.createTextNode(""));
                row.appendChild(cell);
            } else if (date > daysInMonth(year, month)) {
                break;
            } else {
                let cell = document.createElement("td");
                if (datesState[date] !== undefined) {
                    let link = document.createElement("a");
                    link.href = `/Employee/CheckDay/${employeeId}/${currentYear}/${currentMonth+1}/${date}`;
                    link.innerText = date;
                    link.classList.add("text-decoration-none", "text-white");
                    cell.classList.add(`${datesState[date] ? "bg-success" : "bg-danger"}`);
                    cell.appendChild(link);
                } else {
                    cell.appendChild(document.createTextNode(date));
                }
                row.appendChild(cell);
                date++;
            }
        }
        tableBody.appendChild(row);
    }
}

showCalendar(currentYear, currentMonth);