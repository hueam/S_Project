const Enums = require('../Enums/Enum.js');
class Room
{
    
    static rooms = [];
    static Create(socket, roomId)
    {
        console.log(roomId);
        if(Room.rooms[roomId] != undefined)
        {
            socket.emit("message","이미 존재하는 방입니다");
            return;
        }
        Room.rooms[roomId] = [];
        socket.join(roomId);
        socket.roomId = roomId;
        socket.IsReady = false;
        Room.rooms[roomId].push(socket);
        socket.emit("ChangeScene",Enums.SceneTypes.Room);
    }
    static Join(socket, roomId)
    {
        if(Room.rooms[roomId] != undefined)
        {
            if(Room.rooms[roomId].length == 6)
            {
                socket.emit("message","가득 찬 방입니다");
                console.error("가득찬 방입니다");
            }
            socket.emit("ChangeScene",Enums.SceneTypes.Room);
            socket.join(roomId);
            socket.roomId = roomId;
            socket.IsReady = false;
            socket.to(roomId).emit('enterOther',socket.id);
            Room.rooms[roomId].forEach(s => {
                socket.emit('enterOther',s.id);
                console.log(s.IsReady == true);
                if(s.IsReady == true) socket.emit("otherReady",s.IsReady);
            });
            Room.rooms[roomId].push(socket);
        }
        else
        {
            socket.emit("message","존재하지 않는 방입니다");
            console.error("없는방입니다");
        }
    }
    static Play(socket)
    {
        Room.rooms[socket.roomId].forEach(s => {
            s.emit("ChangeScene",Enums.SceneTypes.InGame);
        });
    }
    static Exit(socket)
    {
        try{
        if(socket.roomId != undefined)
        {
            let eventName = "";
            Room.rooms[socket.roomId].splice(Room.rooms[socket.roomId].indexOf(socket.id),1);
            if(Room.rooms[socket.roomId].length < 2)
            {
                Room.rooms.splice(Room.rooms.indexOf(socket.roomId),1);  
                socket.to(socket.roomId).emit("deleteRoom",null);
            }
            else
            {
                socket.to(socket.roomId).emit("deleteRoom",null);
            }
        }
        }
        catch(e)
        {
            console.log(e);
        }
    }
}
exports.Room = Room;