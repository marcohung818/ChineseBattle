using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ElementRoot[] equippedWords = this.gameObject.GetComponentsInChildren<ElementRoot>();
        int count = 0;
        int skillPanel = PlayerPrefs.GetInt("panelIndex");
        foreach (ElementRoot word in equippedWords)
        {
            word.Word = GetWordFunction(skillPanel, count);
            count++;
        }
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
}
