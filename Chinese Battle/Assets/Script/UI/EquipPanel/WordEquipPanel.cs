using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS.Extensions.Common;
using UnityEngine;
using UnityEngine.UI;

public class WordEquipPanel : MonoBehaviour
{
    public static WordEquipPanel instance;
    public GameObject wordIntroPanel;
    public GameObject equipPanel_1;
    public GameObject equipPanel_2;
    [SerializeField] private GameObject eqedSet_1_Button;
    [SerializeField] private GameObject eqedSet_2_Button;
    public bool selecting = false;
    public string selectingWord;
    public string[] skillWordArray_1 = new string[8];
    public string[] skillWordArray_2 = new string[8];

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        EnableEquipPanel_1();
    }

    public void DisableWordIntroPanelByChild_X()
    {
        wordIntroPanel.SetActive(false);
    }

    public void EnableAndSetWordIntroPanel(GameObject word)
    {
        wordIntroPanel.transform.GetChild(1).GetComponent<Image>().color = word.GetComponent<Image>().color;
        wordIntroPanel.transform.GetChild(2).GetComponent<Text>().text = "<b>字詞: " + word.GetComponent<ElementRoot>().Word + "</b>";
        wordIntroPanel.transform.GetChild(3).GetComponent<Text>().text = "<b>CD: " + word.GetComponent<ElementRoot>().cd.ToString() + "</b>";
        wordIntroPanel.SetActive(true);
    }

    public void EnableEquipPanel_1()
    {
        equipPanel_1.SetActive(true);
        equipPanel_2.SetActive(false);
        eqedSet_1_Button.GetComponent<Image>().color = Color.black;
        eqedSet_2_Button.GetComponent<Image>().color = Color.white;
    }

    public void EnableEquipPanel_2()
    {
        equipPanel_1.SetActive(false);
        equipPanel_2.SetActive(true);
        eqedSet_1_Button.GetComponent<Image>().color = Color.white;
        eqedSet_2_Button.GetComponent<Image>().color = Color.black;
    }
}
