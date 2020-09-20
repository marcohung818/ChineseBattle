using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Animations;

public class BattleField : MonoBehaviour
{
    public static BattleField instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int hostCharacterNo = PlayerPrefs.GetInt("CharacterIndex");
            GameObject hostcharacter = CharacterTypeHolder.instance.Characters[hostCharacterNo].CharacterGameObject;
            PhotonNetwork.Instantiate(hostcharacter.name, new Vector3(-2.38f, 3.12f, 0f), Quaternion.identity, 0);
        }
        else
        {
            int clientCharacterNo = PlayerPrefs.GetInt("CharacterIndex");
            GameObject clientcharacter = CharacterTypeHolder.instance.Characters[clientCharacterNo].CharacterGameObject;
            PhotonNetwork.Instantiate(clientcharacter.name, new Vector3(2.35f, 3.15f, 0f), Quaternion.Euler(0, 180, 0), 0);
        }

    }
}
