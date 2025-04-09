public struct ResultCallBack
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    
    public static ResultCallBack Success(string message = "")
    {
        return new ResultCallBack
        {
            IsSuccess = true,
            Message = message
        };
    }
    
    public static ResultCallBack Failed(string message = "")
    {
        return new ResultCallBack
        {
            IsSuccess = false,
            Message = message
        };
    }
}

public struct CreateRoomResultCallBack
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public int RoomId { get; set; }
}


public struct CreateActorResultCallBack
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public int ActorId { get; set; }
}


public struct LoginUIdResultCallBack
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public int UId { get; set; }
}