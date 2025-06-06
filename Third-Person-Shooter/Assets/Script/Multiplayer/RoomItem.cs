using UnityEngine;
using TMPro;

public class RoomItem : MonoBehaviour
{
    public TMP_Text roomName;
    LobbyManager manager;

    private void Start()
    {
        manager = FindAnyObjectByType<LobbyManager>();
    }

    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }

    public void OnClickItem()
    {
        manager.JoinRoom(roomName.text);
    }
}
