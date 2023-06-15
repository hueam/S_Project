exports.Types = {
    Intro : 0,
    Room :1,
    InGame : 2,
    Interact : 3,
    Chat : 4,
    Error : 5,
    ETC : 6,
};

exports.Intro = {
    Create : 0,
    Enter : 1,
    ChangeScene : 2,
    Remove : 3,
    OtherJoin : 4,
    Back2Lobby : 5,
    OtherQuit : 6,
};

exports.Room = {
    Ready : 0,
    Start : 1,
    ChangeScene : 2,
    Move : 3,
    InitP : 4
};
exports.InGame = {
    EnterP : 0,
    Hit : 1
}

exports.InteractEvents = {
    Damage : 0,
    PlayerMove : 1,
    Spawn : 2,
    BoolAnim : 3,
    Ride : 4,
    TriggerAnim : 5,
    Fire : 6,
};

exports.ErrorEvents = {
    ErrorMessage : 0,
}
exports.SceneTypes = 
{
    Intro : 0,
    Room : 1,
    InGame : 2
}