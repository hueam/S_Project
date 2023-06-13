const { Room } = require ('../Classes/RoomManager.js');
const Enums = require('../Enums/Enum.js');
const handlers = [];

handlers[Enums.InGame.EnterP] = (socket, data)=> 
{
    socket.to(socket.roomId).emit("enterOther",socket.id);
}

exports.handler = handlers;
