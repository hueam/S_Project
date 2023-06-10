const { Room } = require ('../Classes/RoomManager.js');
const Enums = require('../Enums/Enum.js');
const handlers = [];

handlers[Enums.Room.Ready] = (socket, data)=> 
{
    let b = data.toLowerCase() === "true";
    console.log(b);
    socket.IsReady = b;
    socket.to(socket.roomId).emit("otherReady",b);
}
handlers[Enums.Room.Start] = (socket, data)=> 
{
    Room.Play(socket);
}
handlers[Enums.Room.Move] = (socket,data)=>
{
    let obj = JSON.parse(data);
    obj.id = socket.id
    socket.to(socket.roomId).emit("MoveOther",obj);
}

exports.handler = handlers;
