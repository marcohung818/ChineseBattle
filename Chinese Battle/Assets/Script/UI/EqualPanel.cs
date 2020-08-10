using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EqualPanel : MonoBehaviour
{
    [SerializeField] private GameObject wordEquipPanel;
    [SerializeField] private GameObject characterEquipPanel;

    public void EnableCharacterEquipPanel()
    {
        wordEquipPanel.SetActive(false);
        characterEquipPanel.SetActive(true);
    }

    public void EnableWordEquipPanel()
    {
        wordEquipPanel.SetActive(true);
        characterEquipPanel.SetActive(false);
    }
}
