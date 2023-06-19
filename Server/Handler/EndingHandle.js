const Enums = require('../Enums/Enum.js');
const handlers = [];

handlers[Enums.EndingEvnet.GotoIntro] = (socket,data) => 
{
    socket.emit("ChangeScene",Enums.SceneTypes.Intro);
}

exports.handler = handlers;