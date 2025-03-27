using System.Collections.Generic;
using System.Numerics;
using ProtoBuf;

namespace ShareProtobuf
{
    //创建Actor请求
    [ProtoContract]
    public class CreatePlayerActorRequest
    {
        [ProtoMember(1)] public string PlayerId { get; set; }
        [ProtoMember(2)] public int RoomId { get; set; }
        [ProtoMember(3)] public Vector3 SpawnPos { get; set; }
        [ProtoMember(4)] public Vector3 SpawnRot { get; set; }
    }

    //创建Actor响应
    [ProtoContract]
    public class CreatePlayerActorResponse
    {
        [ProtoMember(1)] public bool IsSuccess { get; set; }
        [ProtoMember(2)] public string Message { get; set; }
        [ProtoMember(4)] public int RefActorId { get; set; } //客户端对应的ActorId
    }

    [ProtoContract]
    public class CreateRoomActorToClientRequest
    {
        [ProtoMember(1)] public int RoomId { get; set; }
        [ProtoMember(2)] public List<GameActorInfo> Actors { get; set; }
    }

    //更新 位置旋转信息
    [ProtoContract]
    public class DeltaActorSyncData
    {
        [ProtoMember(1)] public int ActorId { get; set; }
        [ProtoMember(2)] public Vector3 Pos { get; set; }
        [ProtoMember(3)] public Vector3 Rot { get; set; }
        [ProtoMember(4)] public Vector3 Speed { get; set; }
        
        [ProtoMember(4)] public long SyncTime { get; set; }

    }

    [ProtoContract]
    public class DeltaActorSyncRequest
    {
        [ProtoMember(1)] public string PlayerId { get; set; }
        [ProtoMember(2)] public int RoomId { get; set; }
        [ProtoMember(3)] public List<DeltaActorSyncData> Actors { get; set; }
        [ProtoMember(4)] public List<int> InViewActorIds { get; set; }
    }
    
    [ProtoContract]
    public class DeltaActorSyncResponse
    {
        [ProtoMember(1)] public bool IsSuccess { get; set; }
        [ProtoMember(2)] public string Message { get; set; }
        [ProtoMember(3)] public List<DeltaActorSyncData> Actors { get; set; }
    }

}