using UnityEngine;

public class RecycleObject : MonoBehaviour, IRecycle
{
    public string ObjectName { get; set; }

    public void Recycle()
    {
        // Implement the recycling logic here
        Debug.Log("Object recycled");
    }
}