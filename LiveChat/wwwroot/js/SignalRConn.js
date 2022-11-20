globalThis.connection = new signalR
    .HubConnectionBuilder()
    .withUrl("/ws/chat")
    .build();
