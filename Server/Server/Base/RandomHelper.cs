public class RandomHelper
{
    private static System.Random random = new System.Random();
    
    public static int GetRandom(int min, int max)
    {
        return random.Next(min, max);
    }
    
    public static float GetRandom(float min, float max)
    {
        return (float)random.NextDouble() * (max - min) + min;
    }
}