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
}