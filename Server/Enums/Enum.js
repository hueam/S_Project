exports.Types = {
    Intro : 0,
    Room :1,
    InGame : 2,
    Ending : 3,
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
    Hit : 1,
    Die : 2,
    ReSpawn :3,
    Ending : 4,
    Fire : 5,
}

exports.SceneTypes = 
{
    Intro : 0,
    Room : 1,
    InGame : 2,
    Ending : 3,
}
exports.EndingEvnet = {
    GotoIntro : 0,
}