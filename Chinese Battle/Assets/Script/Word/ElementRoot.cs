using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementRoot : MonoBehaviour
{
    public int[] wordPos = new int[2];
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
    // Update is called once per frame
    private void Awake()
    {
        wordPos[0] = this.gameObject.GetComponentInParent<Tile>().rowPos;
        wordPos[1] = this.gameObject.GetComponentInParent<Tile>().colPos;
    }
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
