using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordMaster : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    [HideInInspector]public bool onSelected = false; //Not allow reselected again

    //When the pointer is down
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        SetSelected();
        GameEvents.instance.pointerOnHold = true;
        GameEvents.instance.RecordImage(this.gameObject);
    }

    //When the pointer is down and drag 
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (GameEvents.instance.pointerOnHold && !onSelected)
        {
            SetSelected();
            GameEvents.instance.RecordImage(this.gameObject);
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        GameEvents.instance.pointerOnHold = false;
        GameEvents.instance.EndRecordImage();
    }

    public void SetSelected()
    {
        onSelected = true;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void SetAvailable()
    {
        onSelected = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
