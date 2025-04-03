

using ShareProtobuf;

public class PetAttackTargetRequestHandle : MessageRquestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        PetAttackTargetActorRequest request = await GetClientHandle().ReceiveMessage<PetAttackTargetActorRequest>(messageBuffer);
        Console.WriteLine("PetAttackTargetActorRequest RoomName: {0}", request.RoomId);
        GameRoom room = GameRoomManager.Instance.GetGameRoom(request.RoomId);
        if (room == null)
        {
            PetAttackTargetActorResponse response = new PetAttackTargetActorResponse
            {
                IsSuccess = false,
                Message = "Room not found",
            };
            await GetClientHandle().SendMessage(MessageRequestType.PetAttackTargetResponse, response);
            return;
        }
        PetActor petActor = room.RoomWorld.GetPet(request.PetActorId);
        if (petActor == null)
        {
            PetAttackTargetActorResponse response = new PetAttackTargetActorResponse
            {
                IsSuccess = false,
                Message = "Pet not found"
            };
            await GetClientHandle().SendMessage(MessageRequestType.PetAttackTargetResponse, response);
            return;
        }
        BreakInteractiveActor breakInteractiveActor = room.RoomWorld.GetBreakInteractive(request.TargetActorId);
        if (breakInteractiveActor == null)
        {
            PetAttackTargetActorResponse response = new PetAttackTargetActorResponse
            {
                IsSuccess = false,
                Message = "breakInteractiveActor not found"
            };
            await GetClientHandle().SendMessage(MessageRequestType.PetAttackTargetResponse, response);
            return;
        }
        breakInteractiveActor.Damage(petActor.ActorId,petActor.GetAttackDamage());
        if (breakInteractiveActor.IsDestroy)
        {
            room.GetEffectController().PlayEffect(breakInteractiveActor.ActorId, breakInteractiveActor.DestroyEffectName, breakInteractiveActor.Pos, breakInteractiveActor.Rot, false);
        }
    }
}