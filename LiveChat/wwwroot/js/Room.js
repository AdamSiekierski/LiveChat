"use strict";

document.getElementById("sendButton").disabled = true;

function deleteMessage(e) {
    const messageId = e.target.dataset.messageId;

    console.log('dupa', Number(messageId), Number(roomId))

    connection.invoke('DeleteMessage', Number(messageId), Number(window.roomId));
}

function createDeleteButtonHandlers() {
    document.querySelectorAll('.deleteMessageButton').forEach((el) => {
        el.removeEventListener('click', deleteMessage);

        el.addEventListener('click', deleteMessage);
    })
}

connection.on("MessageSent", (author, content, id) => {
    const li = document.createElement("li");

    li.dataset.messageId = id;

    if (author === window.userName || window.userName === window.roomAuthor) {
        li.innerHTML = `<b>${author}</b>: ${content} <button class="deleteMessageButton" data-message-id="${id}">X</button>`;
    } else {
        li.innerHTML = `<b>${author}</b>: ${content}`;
    }

    document.getElementById("messageList").prepend(li);

    createDeleteButtonHandlers();
});

connection.on("MessageRemoved", (messageId) => {
    const messageEl = document.querySelector(`li[data-message-id="${messageId}"]`);

    messageEl?.remove();
}) 

connection.start().then(() => {
    document.getElementById("sendButton").disabled = false;
    connection.invoke("JoinRoom", Number(document.getElementById("roomId").value))
}).catch((err) => console.error(err.toString()));

document.getElementById("messageForm").addEventListener("submit", (e) => {
    e.preventDefault();

    const formData = new FormData(e.target);

    e.target.reset();

    connection.invoke("SendMessage", Number(formData.get("roomId")), formData.get("message"));
});

createDeleteButtonHandlers();