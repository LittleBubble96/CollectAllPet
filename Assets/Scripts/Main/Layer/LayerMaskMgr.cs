using UnityEngine;

public class LayerMaskMgr
{
    public static string PetLayerName = "Pet";
    public static string BreakInteractiveLayerName = "Break";
    public static LayerMask PetLayerMask = LayerMask.GetMask(PetLayerName);
    public static LayerMask BreakInteractiveLayerMask = LayerMask.GetMask(BreakInteractiveLayerName);
}