using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{
    const string playerNamePrefKey = "PlayerName";

    void Start()
    {
        string defaultName = string.Empty;
        InputField _inputField = this.GetComponent<InputField>();
        if (_inputField != null)
        {
            print("hi");
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                print("hi2");
                defaultName = PlayerPrefs.
                    GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
        }
        // 設定遊戲玩家的名稱
        PhotonNetwork.NickName = defaultName;
    }

    public void SetPlayerName(string value)
    {
        print(value);
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        print(value);
        // 設定遊戲玩家的名稱
        PhotonNetwork.NickName = value;
        PlayerPrefs.SetString(playerNamePrefKey, value);
    }
}
