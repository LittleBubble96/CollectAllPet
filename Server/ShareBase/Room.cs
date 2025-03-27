using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Numerics;
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
     public class  GameActorInfo
     {
        [ProtoMember(1)] public string OwnerPlayerId { get; set; }
        [ProtoMember(2)] public int ActorConfigId { get; set; }
        [ProtoMember(3)] public int RefActorId { get; set; }
        [ProtoMember(4)] public string ActorName { get; set; }
        [ProtoMember(5)] public int ActorRoleType { get; set; }
     }

    //详细房间信息
    [ProtoContract]
    public class  RoomDetailInfo
    {
        [ProtoMember(1)] public int RoomId { get; set; }
        [ProtoMember(2)] public string RoomName { get; set; }
        [ProtoMember(3)] public int PlayerCount { get; set; }
        [ProtoMember(4)] public int MaxPlayerCount { get; set; }
        //世界Actor 列表
        [ProtoMember(5)] public List<GameActorInfo> WorldActors { get; set; }
    }
    
    //客户端请求 详细房间信息
    [ProtoContract]
    public class  GetRoomDetailRequest
    {
        [ProtoMember(1)] public string PlayerId { get; set; }
        [ProtoMember(2)] public int RoomId { get; set; }
    }
    
    [ProtoContract]
    public class  GetRoomDetailResponse
    {
        [ProtoMember(1)] public bool IsSuccess { get; set; }
        [ProtoMember(2)] public string Message { get; set; }
        [ProtoMember(3)] public RoomDetailInfo RoomDetailInfo { get; set; }
        [ProtoMember(4)] public int RefActorId { get; set; } //客户端对应的ActorId
    }
}