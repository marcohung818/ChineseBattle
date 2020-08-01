using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GenManager : MonoBehaviourPun
{
    [SerializeField] GameObject ChassBoard;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(ChassBoard.name, Vector3.zero, Quaternion.identity);
        }
    }


}
