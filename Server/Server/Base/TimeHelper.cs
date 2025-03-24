public class TimeHelper
{
    public static long GetTimeStamp()
    {
        return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
    }
}