const SocketIo = require("socket.io");

const Enums = require('./Enums/Enum.js');
const Packet = require('./Classes/Packet.js');

const handlers = []; //handler list

handlers[Enums.Types.Intro] = require('./Handler/IntroHandler.js').handler;

const io = new SocketIo.Server(3000);

console.log(3000,'포트에서 서버열림')
io.on("connection", (socket) => {
    console.log('Client connected');
    socket.on('message',(message) => {
        packet = JSON.parse(message);
        trigger = handlers[packet.e][packet.t];
        if(typeof(trigger) === 'function')
            trigger(socket,packet.v);
    });

});