using ShareProtobuf;
using TMPro;
using UnityEngine;

public class RoomUserItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _usernameText;

    public void UpdateUser(SimplePlayerInfo playInfo)
    {
        _usernameText.text = playInfo.UserName;
    }
}