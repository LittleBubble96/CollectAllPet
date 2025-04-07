
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
    
    //Dot
    public static float Dot(Vector3 a, Vector3 b)
    {
        return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
    }
    
    //Cross
    public static Vector3 Cross(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X
        );
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