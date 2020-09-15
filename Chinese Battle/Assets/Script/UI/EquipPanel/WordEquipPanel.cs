using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordEquipPanel : MonoBehaviour
{
    public static WordEquipPanel instance;
    [SerializeField] private GameObject wordIntroPanel;
    [SerializeField] private GameObject equipPanel_1;
    [SerializeField] private GameObject equipPanel_2;
    [SerializeField] private GameObject eqedSet_1_Button;
    [SerializeField] private GameObject eqedSet_2_Button;

    [HideInInspector]public bool selecting = false;
    [HideInInspector]public string selectingWord;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("panelIndex") == 1)
        {
            EnableEquipPanel_1();
        }
        else
        {
            EnableEquipPanel_2();
        }
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
        PlayerPrefs.SetInt("panelIndex", 1);
        ElementRoot[] equippedWords = equipPanel_1.GetComponentsInChildren<ElementRoot>();
        int count = 0;
        foreach(ElementRoot word in equippedWords)
        {
            word.Word = GetWordFunction(1, count);
            count++;
        }
    }

    public void EnableEquipPanel_2()
    {
        equipPanel_1.SetActive(false);
        equipPanel_2.SetActive(true);
        eqedSet_1_Button.GetComponent<Image>().color = Color.white;
        eqedSet_2_Button.GetComponent<Image>().color = Color.black;
        PlayerPrefs.SetInt("panelIndex", 2);
        ElementRoot[] equippedWords = equipPanel_2.GetComponentsInChildren<ElementRoot>();
        int count = 0;
        foreach (ElementRoot word in equippedWords)
        {
            word.Word = GetWordFunction(2, count);
            count++;
        }
    }

    public void SetWordFunction(int equipPanel, int position, string word)
    {
        PlayerPrefs.SetString("equipPanel_" + equipPanel.ToString() + "_" + position.ToString(), word);
    }

    private string GetWordFunction(int equipPanel, int position)
    {
        if (PlayerPrefs.GetString("equipPanel_" + equipPanel.ToString() + "_" + position.ToString()) != "")
        {
            return PlayerPrefs.GetString("equipPanel_" + equipPanel.ToString() + "_" + position.ToString());
        }
        else
        {
            return "k";
        }
    }

    private void ClearWordFunction()
    {
        PlayerPrefs.DeleteAll();
    }
}
