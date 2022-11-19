window.singalRConnection = new signalR
    .HubConnectionBuilder()
    .withUrl("/ws/chat")
    .build();
