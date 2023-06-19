const Enums = require('./Enums/Enum.js');
const Room = require('./Classes/RoomManager.js').Room;
const Packet = require('./Classes/Packet.js');
const express = require('express');
const app = express();
const server = require('http').Server(app);
const io = require('socket.io')(server);

const handlers = []; //handler list

handlers[Enums.Types.Intro] = require('./Handler/IntroHandler.js').handler;
handlers[Enums.Types.Room] = require('./Handler/RoomHandler.js').handler;
handlers[Enums.Types.InGame] = require('./Handler/InGameHandler.js').handler;
handlers[Enums.Types.Ending] = require('./Handler/EndingHandle.js').handler;


io.on("connection", (socket) => {
    socket.emit('connection',true);
    console.log(socket.id);
    socket.on('message',(message) => {
        packet = JSON.parse(message);
        trigger = handlers[packet.e][packet.t];
        if(typeof(trigger) === 'function')
            trigger(socket,packet.v);
    });
    socket.on('disconnect', function () {
        console.log("한명나감");
        Room.Exit();
    });
});
server.listen(3000, () => {
    console.log(`Server listening on port ${3000}`);
});