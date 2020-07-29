using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class PlayerSpawner : MonoBehaviourPun
{
    [SerializeField] GameObject PlayerController;
    private void Start()
    {
        PhotonNetwork.Instantiate(PlayerController.name, Vector2.zero, Quaternion.identity);
    }
}
