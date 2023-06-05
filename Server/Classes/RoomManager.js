class Room
{
    
    static rooms = [];
    static Create(socket, roomId)
    {
        if(Room.rooms[roomId] != undefined)
        {
            socket.emit("message","이미 존재하는 방입니다");
            return;
        }
        Room.rooms[roomId] = [];
        socket.join(roomId);
        socket.roomId = roomId;
        Room.rooms[roomId].push(socket);
    }
    static Join(socket, roomId)
    {
        if(Room.rooms[roomId] != undefined)
        {
            socket.join(roomId);
            socket.to(roomId).emit('enterOther',socket.id);
            Room.rooms[roomId].forEach(s => {
                socket.emit('enterOther',s.id);
            });
        }
        else
        {
            console.error("없는방입니다");
        }
    }
}
exports.Room = Room;