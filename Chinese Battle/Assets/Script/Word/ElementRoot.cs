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
            EnableScript();
        }
    }
    [SerializeField] private string word;

    private void Awake()
    {
        //Save the Word Position from Tile, for the ShowOtherSelected Purpose
        EnableScript();
    }

    private void Start()
    {
        ApplyImage(word);
    }

    private void Update()
    {
        
    }

    //apply the style from the from holder
    private void ApplyImage(string word)
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
                transform.GetComponent<Image>().color = WordTypeHolder.instance.wordTypeList[wordPos].word_image.color;
            }
        }
        else
        {
            transform.GetComponent<Image>().color = Color.black;
        }
    }

    public void EnableScript()
    {
        if (gameObject.transform.parent.tag == "ChassBoard")
        {
            this.gameObject.GetComponent<ChassBoardElement>().enabled = true;
            this.gameObject.GetComponent<FunctionPanelElement>().enabled = false;
        }
        else if (gameObject.transform.parent.tag == "FunctionPanel")
        {
            this.gameObject.GetComponent<ChassBoardElement>().enabled = false;
            this.gameObject.GetComponent<FunctionPanelElement>().enabled = true;
        }
    }

}
