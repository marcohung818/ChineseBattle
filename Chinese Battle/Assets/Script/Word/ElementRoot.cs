using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementRoot : MonoBehaviour
{
    public string Word
    {
        get
        {
            return word;
        }
        set
        {
            word = value;
            ApplyImage(value);
        }
    }
    [SerializeField] private string word;
    // Update is called once per frame
    void Start()
    {
        if (gameObject.transform.parent.tag == "ChassBoard")
        {
            print("Chassboard");
            this.gameObject.GetComponent<ChassBoardElement>().enabled = true;
            this.gameObject.GetComponent<FunctionPanelElement>().enabled = false;
        }
        else if (gameObject.transform.parent.tag == "FunctionPanel")
        {
            print("functionpanel");
            this.gameObject.GetComponent<ChassBoardElement>().enabled = false;
            this.gameObject.GetComponent<FunctionPanelElement>().enabled = true;
        }
    }
    void Update()
    {
        
    }

    //apply the style from the from holder
    void ApplyImageFromHolder(int index)
    {
        transform.GetComponent<Image>().color = WordTypeHolder.instance.wordTypeList[index].word_image.color;
    }

    void ApplyImage(string word)
    {
        if (word != "k")
        {
            int wordPos = -1;
            for (int i = 0; i < WordTypeHolder.instance.wordTypeList.Length; i++)
            {
                if (WordTypeHolder.instance.wordTypeList[i].s_word == word)
                {
                    wordPos = i;
                    break;
                }
            }
            if (wordPos != -1)
            {
                ApplyImageFromHolder(wordPos);
            }
        }
        else
        {
            transform.GetComponent<Image>().color = WordTypeHolder.instance.emptyImage.color;
        }
    }
}
