using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EqualPanel : MonoBehaviour
{
    public static EqualPanel instance;
    [SerializeField] private GameObject wordEquipPanel;
    [SerializeField] private GameObject characterEquipPanel;

    private void Awake()
    {
        instance = this;
    }

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
