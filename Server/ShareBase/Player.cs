using ProtoBuf;
using System;

namespace ShareProtobuf
{
    [ProtoContract]
    public class PlayerLogin
    {

        [ProtoMember(1)] public string Account { get; set; }

        [ProtoMember(2)] public string Password { get; set; }
    }

    [ProtoContract]
    public class SimplePlayerInfo
    {
        [ProtoMember(1)] public string UserId { get; set; }
        [ProtoMember(2)] public string UserName { get; set; }
    }

    [ProtoContract]
    public class PlayerData
    {
        [ProtoMember(1)] public string userId { get; set; }
        [ProtoMember(2)] public string userName { get; set; }
        [ProtoMember(3)] public int playerConfigId { get; set; }
    }

    [ProtoContract]
    public class PlayerLoginResponse
    {
        [ProtoMember(1)] public bool IsSuccess { get; set; }
        [ProtoMember(2)] public string Message { get; set; }
        [ProtoMember(3)] public PlayerData PlayerData { get; set; }
    }

    [ProtoContract]
    public class  Heartbeat
    {
        //时间戳
        [ProtoMember(1)] public long Timestamp { get; set; }
    }

    [ProtoContract]
    public class HeartbeatAck
    {
        [ProtoMember(1)] public long Timestamp { get; set; }
    }
}

