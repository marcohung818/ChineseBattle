using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Animations;

public class BattleField : MonoBehaviour
{
    public static BattleField instance;
    [SerializeField] GameObject HostCharacterPosition;
    [SerializeField] GameObject ClientCharacterPosition;

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
            HostCharacterPosition.GetComponent<Animator>().runtimeAnimatorController = CharacterTypeHolder.instance.Characters[hostCharacterNo].CharacterAnimator;

        }
        else
        {
           int clientCharacterNo = PlayerPrefs.GetInt("CharacterIndex");
           ClientCharacterPosition.GetComponent<Animator>().runtimeAnimatorController = CharacterTypeHolder.instance.Characters[clientCharacterNo].CharacterAnimator;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
