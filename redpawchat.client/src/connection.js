import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:5123/chathub")
  .withAutomaticReconnect()
  .build();

export default connection;
