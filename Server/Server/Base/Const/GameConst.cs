public class GameConst
{ 
    public const int RoomIdStart = 0;
    // 房间ID的最大值 
    public const int RoomIdEnd = 1000;
    //60帧
    public const int FrameRate = 60;
    //每帧的时间间隔
    public const double FrameInterval = 1f / FrameRate;
    //
    public static ShareProtobuf.Vector3 ZeroVector3 = new ShareProtobuf.Vector3() { X = 0, Y = 0, Z = 0 };
}