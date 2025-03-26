using System.Collections.Generic;
using ShareProtobuf;
using ProtoBuf;
using System.Numerics;
public class ClientRequestFunc
{
    public static async void SendLoginRequest(string username, string password)
    {
        PlayerLogin playerLogin = new PlayerLogin();
        playerLogin.Account = username;
        playerLogin.Password = password;
        await GameManager.GetNetworkManager().SendRequest(MessageRequestType.PlayerLogin, playerLogin);
    }
    
    public static async void SendCreateRoomRequest(string roomName, int maxPlayer)
    {
        CreateRoomRequest createRoom = new CreateRoomRequest();
        createRoom.RoomName = roomName;
        createRoom.MaxPlayerCount = maxPlayer;
        createRoom.PlayerId = CharacterManager.Instance.PlayerInfo.PlayerId;
        await GameManager.GetNetworkManager().SendRequest(MessageRequestType.CreateRoomRequest, createRoom);
    }
    
    public static async void RefreshRoomListRequest()
    {
        RefreshRoomList refreshRoomList = new RefreshRoomList();
        await GameManager.GetNetworkManager().SendRequest(MessageRequestType.RefreshRoomList, refreshRoomList);
    }
    
    public static async void SendJoinRoomRequest(int roomId)
    {
        JoinRoomRequest joinRoom = new JoinRoomRequest();
        joinRoom.RoomId = roomId;
        joinRoom.PlayerId = CharacterManager.Instance.PlayerInfo.PlayerId;
        await GameManager.GetNetworkManager().SendRequest(MessageRequestType.JoinRoomRequest, joinRoom);
    }
    
    public static async void GetRoomDetailRequest(int roomId, string playerId)
    {
        GetRoomDetailRequest getRoomDetail = new GetRoomDetailRequest();
        getRoomDetail.RoomId = roomId;
        getRoomDetail.PlayerId = playerId;
        await GameManager.GetNetworkManager().SendRequest(MessageRequestType.GetRoomDetailRequest, getRoomDetail);
    }
    
    public static async void SendCreatePlayerActorRequest()
    {
        CreatePlayerActorRequest createPlayer = new CreatePlayerActorRequest();
        createPlayer.PlayerId = CharacterManager.Instance.PlayerInfo.PlayerId;
        createPlayer.RoomId = RoomManager.Instance.GetEnterRoomId();
        await GameManager.GetNetworkManager().SendRequest(MessageRequestType.CreateActorRequest, createPlayer);
    }

    public static async void SyncActorDeltaRequest(List<DeltaActorSyncData> actors, List<int> inViewActorIds)
    {
        DeltaActorSyncRequest syncActorDelta = new DeltaActorSyncRequest();
        syncActorDelta.PlayerId = CharacterManager.Instance.PlayerInfo.PlayerId;
        syncActorDelta.RoomId = RoomManager.Instance.GetEnterRoomId();
        syncActorDelta.Actors = actors;
        syncActorDelta.InViewActorIds.AddRange(inViewActorIds);
        await GameManager.GetNetworkManager().SendRequest(MessageRequestType.SyncActorDetailRequest, syncActorDelta);
    }
}
