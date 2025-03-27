using System.Numerics;

public class ConfigHelper
{
    public static Vector3 ConvertUnityVector3ToVector3(UnityEngine.Vector3 unityVector3)
    {
        return new Vector3(unityVector3.x, unityVector3.y, unityVector3.z);
    }
    
    public static UnityEngine.Vector3 ConvertVector3ToUnityVector3(Vector3 vector3)
    {
        return new UnityEngine.Vector3(vector3.X, vector3.Y, vector3.Z);
    }
    
}