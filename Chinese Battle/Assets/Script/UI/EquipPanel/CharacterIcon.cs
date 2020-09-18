using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int CharacterID;
    [SerializeField] private string CharacterName;

    public void OnPointerClick(PointerEventData eventData)
    {
        CharacterPanel.instance.ResetCharacterIcon();
        PlayerPrefs.SetInt("CharacterIndex", this.CharacterID);
        this.GetComponent<Image>().color = Color.green;
    }
}
