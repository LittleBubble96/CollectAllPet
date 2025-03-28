using System;
using System.Collections.Generic;

public class RegisterUIHelper
{
    public static Dictionary<Type,UIInfo> UIInfos = new Dictionary<Type, UIInfo>()
    {
        {typeof(Login_UI), new UIInfo {ResPath = "UI/Login_UI", LayerType = UILayerType.Main}},
        {typeof(RoomMain_UI), new UIInfo {ResPath = "UI/RoomMain_UI", LayerType = UILayerType.Main}},
        {typeof(RoomGame_UI), new UIInfo {ResPath = "UI/RoomGame_UI", LayerType = UILayerType.Main}},
        {typeof(Loading_UI), new UIInfo {ResPath = "UI/Loading_UI", LayerType = UILayerType.Loading}},
        {typeof(Lock_UI ), new UIInfo {ResPath = "UI/Lock_UI", LayerType = UILayerType.Lock}},
        {typeof(CreateRoom_PopupUI), new UIInfo {ResPath = "UI/Room/CreateRoom_PopupUI", LayerType = UILayerType.Pop}},
    };
}