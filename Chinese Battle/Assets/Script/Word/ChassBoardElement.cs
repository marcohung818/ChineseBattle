using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ChassBoardElement : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler, IDropHandler, IPointerExitHandler
{
    [HideInInspector] public bool onSelected = false; //Not allow reselected again

    private void Start()
    {

    }
    private void Update()
    {
        
    }

    //When the pointer is down
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        ChassBoard.instance.RPCShowAllSelected(this.gameObject.GetComponent<ElementRoot>().wordPos);
        ChassBoard.instance.pointerOnHold = true;
        ChassBoard.instance.RecordImage(this.gameObject);
    }

    //When the pointer is down and drag 
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (ChassBoard.instance.pointerOnHold && !onSelected && ChassBoard.instance.CheckTileDiff(this.gameObject))
        {
            ChassBoard.instance.RPCShowAllSelected(this.gameObject.GetComponent<ElementRoot>().wordPos);
            ChassBoard.instance.RecordImage(this.gameObject);
        }
        else if(onSelected && ChassBoard.instance.CheckPopClip(this.gameObject)){
            ChassBoard.instance.PopClip();
        }
        else if(ChassBoard.instance.onDrag == true) //Change the outside frame color when replace the word
        {
            this.gameObject.transform.parent.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        }
    }

    //When the pointer is up, then count the word
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        ChassBoard.instance.pointerOnHold = false;
        ChassBoard.instance.EndRecordImage();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ChassBoard.instance.onDrag == true)
        {
            this.gameObject.transform.parent.GetComponent<Tile>().ResumeOriginalColor();
        }
    }

    //When doing the replace wording
    public void OnDrop(PointerEventData eventData)
    {
       if(eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<FunctionPanelElement>().isActiveAndEnabled)
        {
            this.gameObject.GetComponent<ElementRoot>().Word = eventData.pointerDrag.GetComponent<ElementRoot>().Word;
            this.gameObject.transform.parent.GetComponent<Tile>().ResumeOriginalColor();
            ChassBoard.instance.onDrag = false;
        }
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