using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector2 panelLocation;
    public float percentThrehold = 0.2f;
    public float easing = 0.5f;
    public void OnDrag(PointerEventData eventData)
    {
        float posDiff = eventData.pressPosition.x - eventData.position.x;
        transform.position = panelLocation - new Vector2(posDiff, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float percentage = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
        if(Mathf.Abs(percentage) >= percentThrehold)
        {
            Vector2 newLocation = panelLocation;
            if(percentage > 0)
            {
                newLocation += new Vector2(-Screen.width, 0);
            }
            else if(percentage < 0)
            {
                newLocation += new Vector2(Screen.width, 0);
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        panelLocation = transform.position;
    }

    IEnumerator SmoothMove(Vector2 startPos, Vector2 endPos, float seconds)
    {
        float t = 0f;
        while(t <= 1.0f)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector2.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

}
