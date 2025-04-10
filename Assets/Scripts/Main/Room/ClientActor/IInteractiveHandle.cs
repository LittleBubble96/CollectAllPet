using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public interface IInteractiveHandle
{
    public void HandleInteractive();
    
    public Vector3 GetInteractivePosition();
    
    public string GetInteractiveText();
}