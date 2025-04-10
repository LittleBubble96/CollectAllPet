using UnityEngine;

public class UIHelper
{
    //世界坐标 转化 为 UI坐标
    public static Vector2 WorldToUIPos(Vector3 worldPos)
    {
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
            GameManager.GetUIManager().GetUICamera(), worldPos);
        return screenPos;
    }
}