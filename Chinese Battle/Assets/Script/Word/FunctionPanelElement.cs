using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class FunctionPanelElement : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPos;

    private void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        originalPos = this.gameObject.transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        ChassBoard.instance.onDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        ChassBoard.instance.onDrag = false;
        ResumeOriginalPos();
    }

    //Back to the OrginialPos, Called after EndDrag
    private void ResumeOriginalPos()
    {
        this.gameObject.transform.position = originalPos;
    }
}
