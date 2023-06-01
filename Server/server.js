import { Server } from "socket.io";

const io = new Server(3000);

const rooms = [];
io.on("connection", (socket) => {
    console.log('Client connected');
    socket.emit('connection', {text : "서버열렸어요"});
    socket.on('message',(message) => {
        packet = JSON.parse(message);
        trigger = handlers[packet.e][packet.t];
        if(typeof(trigger) === 'function')
            trigger(socket,packet.v);
    });

});