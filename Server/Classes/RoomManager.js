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
            socket.join(roomId);
            socket.roomId = roomId;
            socket.IsReady = false;
            socket.to(roomId).emit('enterOther',socket.id);
            socket.emit("ChangeScene",Enums.SceneTypes.Room);
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
        if(socket?.roomId != undefined)
        {
            Room.rooms[socket.roomId][socket] == undefined;
            if(Room.rooms[socket.roomId].length < 2)
            {
                Room.rooms[socket.roomId] = undefined;  
                socket.to(socket.roomId).emit("exitOther",socket.id);
                Room.rooms[socket.roomId].Foreach(s=>
                {
                        s.leave(s.roomId);
                })
                socket.to(socket.roomId).emit("ChangeScene",Enums.SceneTypes.Intro);
            }
            else
            {
                socket.to(socket.roomId).emit("exitOther",socket.id);
            }
            socket.leave(socket.roomId);
        }
        }
        catch(e)
        {
            console.log(e);
        }
    }
}
exports.Room = Room;