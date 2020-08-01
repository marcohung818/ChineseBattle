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

    public void AssignTileAsParent(string TileNumber)
    {
        string parent = "Tile (" + TileNumber + ")";
        print(parent);
        this.gameObject.transform.SetParent(GameObject.Find(parent).GetComponent<Transform>(), false);
        EnableScript();
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
