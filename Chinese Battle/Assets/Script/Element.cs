using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Element : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler 
{
    [HideInInspector]public bool onSelected = false; //Not allow reselected again
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
    [SerializeField]private string word;

    //When the pointer is down
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        SetSelected();
        ChassBoard.instance.pointerOnHold = true;
        ChassBoard.instance.RecordImage(this.gameObject);
    }

    //When the pointer is down and drag 
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (ChassBoard.instance.pointerOnHold && !onSelected && ChassBoard.instance.CheckTileDiff(this.gameObject))
        {
            SetSelected();
            ChassBoard.instance.RecordImage(this.gameObject);
        }
        else if(onSelected && ChassBoard.instance.CheckPopClip(this.gameObject)){
            ChassBoard.instance.PopClip();
        }
    }

    //When the pointer is up, then count the word
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        ChassBoard.instance.pointerOnHold = false;
        ChassBoard.instance.EndRecordImage();
    }

    //Gray out the selected word
    public void SetSelected()
    {
        onSelected = true;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    //unGray the selected word
    public void SetAvailable()
    {
        onSelected = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    //apply the style from the from holder
    void ApplyImageFromHolder(int index)
    {
        transform.GetComponent<Image>().color = WordTypeHolder.instance.wordTypeList[index].word_image.color;
    }

    void ApplyImage(string word)
    {
        if (word != "s")
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