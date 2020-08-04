using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonConnection : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject LoadingPanel;
    private const string PlayerPrefsNameKey = "PlayerName";
    private bool Connecting = false;
    private const string GameVersion = "1";
    private const int MaxPlayersPerRoom = 2;

    private void Awake()
    {
        //When the Master Changed Scene, the Client will follow
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster()
    {
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
        LoadArena();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        print("Opponent was surrender");
        LeaveRoom();
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

    public void SetPlayerName(string name)
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

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("UI");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void LoadArena()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            print("I am master");
            PhotonNetwork.LoadLevel("MainGamePlay");
        }
    }
}
