using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WordType
{
    public string s_word;
    public Image word_image;
}

public class WordTypeHolder : MonoBehaviour
{
    [HideInInspector] public static WordTypeHolder instance;
    [HideInInspector] public Image emptyImage;
    public WordType[] wordTypeList;
    void Awake()
    {
        instance = this;
    }

}
