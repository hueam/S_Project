const { Room } = require ('../Classes/RoomManager.js');
const Enums = require('../Enums/Enum.js');
const handlers = [];

handlers[Enums.InGame.EnterP] = (socket, data)=> 
{
    socket.to(socket.roomId).emit("enterOther",socket.id);
}
handlers[Enums.InGame.Hit] = (socket, data)=> 
{
    socket.to(socket.roomId).emit('otherDamage',data);
}
handlers[Enums.InGame.Die] = (socket, data)=> 
{
    socket.to(socket.roomId).emit('otherDie',data);
}
handlers[Enums.InGame.ReSpawn] = (socket,data)=>
{
    let obj = JSON.parse(data);
    obj.id = socket.id
    socket.to(socket.roomId).emit("otherReSapwn",obj);
}
handlers[Enums.InGame.Ending] = (socket, data) =>
{
    socket.to(socket.roomId).emit("ChangeScene",Enums.SceneTypes.Ending);
    socket.emit("ChangeScene",Enums.SceneTypes.Ending);
    socket.leave(socket.roomId);
    Room.rooms[socket.roomId] = undefined;
}
handlers[Enums.InGame.Fire] = (socket,data)=>
{
    let obj = JSON.parse(data);
    obj.id = socket.id
    socket.to(socket.roomId).emit("Fire",obj);
}

exports.handler = handlers;
