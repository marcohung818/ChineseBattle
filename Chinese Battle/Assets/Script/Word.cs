using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Word : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler 
{
    [HideInInspector]public bool onSelected = false; //Not allow reselected again
    public string word;

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
}
