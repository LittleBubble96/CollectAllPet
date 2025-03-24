using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;


namespace ShareProtobuf
{
    [ProtoContract]
    public class JoinRoomRequest
    {
        [ProtoMember(1)] public string PlayerId { get; set; }
        [ProtoMember(2)] public int RoomId { get; set; }
    }

    [ProtoContract]
    public class JoinRoomResponse
    {
        [ProtoMember(1)] public bool IsSuccess { get; set; }
        [ProtoMember(2)] public string Message { get; set; }

        [ProtoMember(3)] public int RoomId { get; set; }
    }

    [ProtoContract]
    public class CreateRoomRequest
    {
        [ProtoMember(1)] public string PlayerId { get; set; }
        [ProtoMember(2)] public string RoomName { get; set; }

        [ProtoMember(3)] public int MaxPlayerCount { get; set; }
    }

    [ProtoContract]
    public class CreateRoomResponse
    {
        [ProtoMember(1)] public bool IsSuccess { get; set; }
        [ProtoMember(2)] public string Message { get; set; }

        [ProtoMember(3)] public int RoomId { get; set; }
    }

    [ProtoContract]
    public class  SimpleRoomInfo
    {
        [ProtoMember(1)] public int RoomId { get; set; }
        [ProtoMember(2)] public string RoomName { get; set; }
        [ProtoMember(3)] public int PlayerCount { get; set; }
        [ProtoMember(4)] public int MaxPlayerCount { get; set; }
        //SimplePlayerInfo 列表
        [ProtoMember(5)] public List<SimplePlayerInfo> Players { get; set; }
    }

    [ProtoContract]
    public class  RefreshRoomList
    {
        
    }

    [ProtoContract]
    public class  RefreshRoomListResponse
    {
        [ProtoMember(1)] public List<SimpleRoomInfo> RoomInfos { get; set; }
    }

    //游戏玩家信息
    [ProtoContract]
     public class  GamePlayerInfo
     {
        [ProtoMember(1)] public string PlayerId { get; set; }
        [ProtoMember(2)] public string PlayerName { get; set; }
        [ProtoMember(3)] public int RefActorId { get; set; }
     }

    //详细房间信息
    [ProtoContract]
    public class  RoomDetailInfo
    {
        [ProtoMember(1)] public int RoomId { get; set; }
        [ProtoMember(2)] public string RoomName { get; set; }
        [ProtoMember(3)] public int PlayerCount { get; set; }
        [ProtoMember(4)] public int MaxPlayerCount { get; set; }
        //SimplePlayerInfo 列表
        [ProtoMember(5)] public List<GamePlayerInfo> Players { get; set; }
    }
}