connection.on("RoomCreated", (name, created, author, id) => {
    const dateFormatter = Intl.DateTimeFormat(['default'], {
        hour: 'numeric',
        minute: 'numeric',
        second: 'numeric',
        year: 'numeric',
        month: 'numeric',
        day: 'numeric',
        hour12: false
    });

    const tr = document.createElement("tr");

    tr.innerHTML = `
        <td>${name}</td>
        <td>${dateFormatter.format(new Date(created))}</td>
        <td>${author}</td>
        <td><a href="/Room?id=${id}">Dołącz</td>
    `

    document.getElementById("roomList").prepend(tr);
});

connection.start().then(() => {
    document.getElementById("sendButton").disabled = false;
}).catch((err) => console.error(err.toString()));
