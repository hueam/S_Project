const { Room } = require ('../Classes/RoomManager.js');
const Enums = require('../Enums/Enum.js');
const handlers = [];

handlers[Enums.Intro.Create] = (socket, data)=> 
{
    Room.Create(socket,data);
}
handlers[Enums.Intro.Enter] = (socket, data)=> 
{
    Room.Join(socket,data);
}

exports.handler = handlers;
