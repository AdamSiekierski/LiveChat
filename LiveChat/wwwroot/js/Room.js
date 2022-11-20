"use strict";

document.getElementById("sendButton").disabled = true;

connection.on("MessageSent", (author, content) => {
    const li = document.createElement("li");
    document.getElementById("messageList").prepend(li);

    li.innerHTML = `<b>${author}</b>: ${content}`;
});

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
