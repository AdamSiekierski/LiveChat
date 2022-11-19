"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/ws/chat").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", (user, message) => {
    const li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);

    li.textContent = `${user}: ${message}`;
});

connection.start().then(() => {
    document.getElementById("sendButton").disabled = false;
}).catch((err) => console.error(err.toString()));

document.getElementById("sendButton").addEventListener("click", (event) => {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch((err) => console.error(err.toString()));
    event.preventDefault();
});
