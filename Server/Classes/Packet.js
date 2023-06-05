class Packet
{
    constructor(type, event, value)
    {
        this.type = type;
        this.event = event;
        this.value = value;
    }
    asPacket()
    {
        return JSON.stringify({
            t:this.type,
            e:this.event,
            v:this.value
        });
    }

}
exports.Packet = Packet;