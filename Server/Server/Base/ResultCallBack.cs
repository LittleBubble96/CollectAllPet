public struct ResultCallBack
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
}

public struct CreateRoomResultCallBack
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public int RoomId { get; set; }
}
