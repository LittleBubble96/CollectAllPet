


using ShareProtobuf;

public class AddPetRequestHandle : MessageRquestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        AddPetActorRequest request = await GetClientHandle().ReceiveMessage<AddPetActorRequest>(messageBuffer);
        Console.WriteLine("AddPetActorRequest playerid " + request.PlayerId);
        ResultCallBack result = await PlayerManager.Instance.AddPet(request.PlayerId,request.PetConfigId);
        AddPetActorResponse response = new AddPetActorResponse();
        if (!result.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = result.Message;
        }
        else
        {
            response.IsSuccess = true;
            response.Message = "Add pet success";
            response.UpdatePlayerData = PlayerManager.Instance.GetPlayer(request.PlayerId).PlayerData;
        }
        await GetClientHandle().SendMessage(MessageRequestType.AddPetResponse, response);
    }
}