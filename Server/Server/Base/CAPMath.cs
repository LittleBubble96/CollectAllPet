
using ShareProtobuf;

public class CAPMath
{
    public static float GetAngle(Vector3 a, Vector3 b)
    {
        float angle = (float)System.Math.Atan2(b.Y - a.Y, b.X - a.X) * 180 / (float)System.Math.PI;
        if (angle < 0)
        {
            angle += 360;
        }
        return angle;
    }
    
    public static float GetDistance(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b);
    }
    
    public static Vector3 RotateY(Vector3 a ,float angle)
    {
        float radian = angle * (float)System.Math.PI / 180;
        float x = a.X * (float)System.Math.Cos(radian) - a.Z * (float)System.Math.Sin(radian);
        float z = a.X * (float)System.Math.Sin(radian) + a.Z * (float)System.Math.Cos(radian);
        return new Vector3(x, a.Y, z);
    }
    
}