
using ShareProtobuf;

public class ConfigHelper
{
    public static Vector3 ZeroVector3 = new Vector3() {X = 0, Y = 0, Z = 0};
    public static ShareProtobuf.Vector3 ConvertUnityVector3ToVector3(UnityEngine.Vector3 unityVector3)
    {
        ShareProtobuf.Vector3 vector3 = new ShareProtobuf.Vector3();
        vector3.X = unityVector3.x;
        vector3.Y = unityVector3.y;
        vector3.Z = unityVector3.z;
        return vector3;
    }
    
    public static UnityEngine.Vector3 ConvertVector3ToUnityVector3(ShareProtobuf.Vector3 vector3)
    {
        if (vector3 == null)
        {
            return UnityEngine.Vector3.zero;
        }
        return new UnityEngine.Vector3(vector3.X, vector3.Y, vector3.Z);
    }
    
}