using ProtoBuf;

namespace ShareProtobuf
{
    [ProtoContract]
    public class FindPetTargetActorIdRequest
    {
        [ProtoMember(1)] public int RoomId { get; set; }
        [ProtoMember(2)] public int PetActorId { get; set; }
        [ProtoMember(3)] public int LastTargetActorId { get; set; }
    }
    
    [ProtoContract]
    public class FindPetTargetActorIdResponse
    {
        [ProtoMember(1)] public bool IsSuccess { get; set; }
        [ProtoMember(2)] public int TargetActorId { get; set; }
        [ProtoMember(3)] public int PetActorId { get; set; }
    }
    
    //攻击
    [ProtoContract]
    public class PetAttackTargetActorRequest
    {
        [ProtoMember(1)] public int RoomId { get; set; }
        [ProtoMember(2)] public int PetActorId { get; set; }
        [ProtoMember(3)] public int TargetActorId { get; set; }
        //TODo 攻击类型  技能啊
    }
    
    [ProtoContract]
    public class PetAttackTargetActorResponse
    {
        [ProtoMember(1)] public bool IsSuccess { get; set; }
        [ProtoMember(4)] public string Message { get; set; }
    }
    
    //
    
}