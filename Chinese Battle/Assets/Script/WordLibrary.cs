using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WordDictionary
{
    public string word;
    public int damage;
    //public GameObject wwordObject;
}

public class WordLibrary : MonoBehaviour
{
    public static WordLibrary instance;
    [SerializeField] private WordDictionary[] Dictionary;
    private void Awake()
    {
        instance = this;
    }
}
