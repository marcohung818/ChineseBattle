using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonConnection : MonoBehaviourPunCallbacks
{
    [SerializeField]GameObject LoadingPanel;
    const string PlayerPrefsNameKey = "PlayerName";
    bool Connecting = false;
    const string GameVersion = "1";
    const int MaxPlayersPerRoom = 2;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster()
    {
        print("Connected To Master");
        if (Connecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected due to: " + cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("No Clients are waiting for an opponent, creating a new room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
    }
    
    //For the user just get into the room
    public override void OnJoinedRoom()
    {
        print("Client successfully joined a room");
        if(PhotonNetwork.CurrentRoom.PlayerCount != MaxPlayersPerRoom)
        {
            print("Client is waiting for an opponent");
        }
        else
        {
            print("Opponent found, Matching is ready to begin");
        }
    }

    //for the user are waiting in the room
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        print("A player was joined the room, Matching is ready to begin");
        PhotonNetwork.LoadLevel("MainGamePlay");
    }

    public void FindingOpponent()
    {
        Connecting = true;
        LoadingPanel.SetActive(true);
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    void CheckSaveAndGetName()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) 
        { 
            return; 
        }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
        SetPlayerName(defaultName);
    }

    void SetPlayerName(string name)
    {
        PhotonNetwork.NickName = name;
        PlayerPrefs.SetString(PlayerPrefsNameKey, name);
    }

    public void ButtonConnectMarco()
    {
        string name = "Marco";
        SetPlayerName(name);
    }

    public void ButtonConnectHung()
    {
        string name = "Hung";
        SetPlayerName(name);
    }

}
