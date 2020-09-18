using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    public static CharacterPanel instance;
    private CharacterIcon[] Characters;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        int playerPrefs_Character = PlayerPrefs.GetInt("CharacterIndex");
        Characters = this.gameObject.GetComponentsInChildren<CharacterIcon>();
        Characters[playerPrefs_Character].gameObject.GetComponent<Image>().color = Color.green;
    }

    public void ResetCharacterIcon()
    {
        foreach (CharacterIcon icon in Characters)
        {
            icon.gameObject.GetComponent<Image>().color = Color.white;
        }
    }


}
