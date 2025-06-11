using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using NUnit.Framework;
using System;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("References")]
    [SerializeField] TMP_InputField roomInputField;
    [SerializeField] GameObject lobbyPanel;
    [SerializeField] GameObject roomPanel;
    [SerializeField] TMP_Text roomName;

    [SerializeField] RoomItem roomItemPrefab;
    List<RoomItem> roomItems = new List<RoomItem>();
    [SerializeField] Transform contentObject;

    List<PlayerItems> playerItemsList = new List<PlayerItems>();
    [SerializeField] PlayerItems playerItemsPrefab;
    [SerializeField] Transform playerItemsParent;

    [SerializeField] GameObject startButton;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnClickCreate()
    {
        if ( roomInputField.text.Length > 0 )
        {
            var roomOptions = new RoomOptions();
            roomOptions.IsVisible = true;
            roomOptions.IsOpen = true;
            roomOptions.MaxPlayers = 5;
            PhotonNetwork.CreateRoom(roomInputField.text, roomOptions);
        }
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Room : " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();

        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomItem item in roomItems)
        {
            Destroy(item.gameObject);
        }
        roomItems.Clear();

        foreach(RoomInfo room in roomList)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItems.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    void UpdatePlayerList()
    {
        foreach (PlayerItems items in playerItemsList)
        {
            Destroy(items.gameObject);
        }
        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItems playerItems = Instantiate(playerItemsPrefab, playerItemsParent);
            playerItems.SetPlayerInfo(player.Value);

            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                playerItems.ApplyLocalChanges();
            }

            playerItemsList.Add(playerItems);
        }

    }

    public void OnClickStartButton()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
