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
        UpdateRectTransform();
        UpdateCanvasGroup();
        RecordOriginalPos();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        ChassBoard.instance.onDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("ondrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("enddrag");
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        ChassBoard.instance.onDrag = false;
        ResumeOriginalPos();
    }

    void UpdateRectTransform()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    void UpdateCanvasGroup()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    void ResumeOriginalPos()
    {
        this.gameObject.transform.position = originalPos;
    }

    void RecordOriginalPos()
    {
        originalPos = this.gameObject.transform.position;
    }
}
