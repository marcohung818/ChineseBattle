using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class Launcher : MonoBehaviourPunCallbacks
{

    // 遊戲版本的編碼, 可讓 Photon Server 做同款遊戲不同版本的區隔.
    string gameVersion = "1";
    [Tooltip("遊戲室玩家人數上限. 當遊戲室玩家人數已滿額, 新玩家只能新開遊戲室來進行遊戲.")]
    [SerializeField] private byte maxPlayersPerRoom = 2;

    void Awake()
    {
        // 確保所有連線的玩家均載入相同的遊戲場景
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Start()
    {
        //Connect();
    }
    // 與 Photon Cloud 連線
    public void Connect()
    {
        // 檢查是否與 Photon Cloud 連線
        if (PhotonNetwork.IsConnected)
        {
            // 已連線, 嚐試隨機加入一個遊戲室
            PhotonNetwork.JoinRandomRoom();
            print("hi");
        }
        else
        {
            // 未連線, 嚐試與 Photon Cloud 連線
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
            print("bye");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN 呼叫 OnConnectedToMaster(), 已連上 Photon Cloud.");

        // 確認已連上 Photon Cloud
        // 隨機加入一個遊戲室
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN 呼叫 OnDisconnected() {0}.", cause);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN 呼叫 OnJoinRandomFailed(), 隨機加入遊戲室失敗.");

        // 隨機加入遊戲室失敗. 可能原因是 1. 沒有遊戲室 或 2. 有的都滿了.    
        // 好吧, 我們自己開一個遊戲室.
        PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = maxPlayersPerRoom});
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("PUN 呼叫 OnJoinedRoom(), 已成功進入遊戲室中.");
    }
}
