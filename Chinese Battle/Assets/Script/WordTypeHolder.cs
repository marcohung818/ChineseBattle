using UnityEngine;
using UnityEngine.UI;

//The Class for Record the Word Type
[System.Serializable]
public class WordType
{
    public string s_word;
    public Image word_image;
    public int cd;
}

public class WordTypeHolder : MonoBehaviour
{
    [HideInInspector] public static WordTypeHolder instance;
    public WordType[] wordTypeList;
    void Awake()
    {
        instance = this;
    }

}
