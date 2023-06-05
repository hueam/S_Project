exports.Types = {
    Intro : 0,
    InGame : 1,
    Interact : 2,
    Chat : 3,
    Error : 4,
    ETC : 5,
};

exports.Intro = {
    Create : 0,
    Enter : 1,
    Quit : 2,
    Remove : 3,
    OtherJoin : 4,
    Back2Lobby : 5,
    OtherQuit : 6,
};

exports.GameManagerEvents = {
    MatchMakingStart : 0,
    Ready : 1,
    Start : 2,
    SetStage : 3,
    Fight : 4,
    Finish : 5,
    MatchMakingStop : 6,
};

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