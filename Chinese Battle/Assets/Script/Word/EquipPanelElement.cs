using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipPanelElement : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerClickHandler
{
    private bool pointerDown;
    private float pointerDownTimer;
    private bool introPoped = false;
    private float requireHoldTime = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (pointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requireHoldTime)
            {
                WordEquipPanel.instance.EnableAndSetWordIntroPanel(this.gameObject);
                introPoped = true;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerDownReset();
    }

    public void PointerDownReset()
    {
        pointerDown = false;
        pointerDownTimer = 0;
        introPoped = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (introPoped == false)
        {
            WordEquipPanel.instance.selecting = true;
            WordEquipPanel.instance.selectingWord = this.gameObject.GetComponent<ElementRoot>().Word;
        }
        else
        {
            PointerDownReset();
        }
    }
}
