using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;

public class UISwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector2 panelLocation;
    private Vector2 subSelectedLocation;
    private GameObject subSelected;
    private GameObject subSpecialPanel;
    private GameObject subFDPanel;
    private GameObject subMainPanel;
    private GameObject subDrawPanel;
    private GameObject subShopPanel;
    private float percentThrehold = 0.2f;
    private float easing = 0.5f;
    private int totalPages = 5;
    private int currentPages = 3;

    public void OnDrag(PointerEventData eventData)
    {
        float posDiff = eventData.pressPosition.x - eventData.position.x;
        transform.position = panelLocation - new Vector2(posDiff, 0);
        subSelected.transform.position = subSelectedLocation + new Vector2(posDiff / 5, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float percentage = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
        if(Mathf.Abs(percentage) >= percentThrehold)
        {
            Vector2 newLocation = panelLocation;
            Vector2 subSelectedNewLocation = subSelectedLocation;
            if(percentage > 0 && currentPages < totalPages)
            {
                currentPages++;
                subSelectedNewLocation += new Vector2(Screen.width / 5, 0);
                newLocation += new Vector2(-Screen.width, 0);
            }
            else if(percentage < 0 && currentPages > 1)
            {
                currentPages--;
                subSelectedNewLocation += new Vector2(-Screen.width / 5, 0);
                newLocation += new Vector2(Screen.width, 0);
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
            switch (currentPages)
            {
                case 5:
                    StartCoroutine(SubSmoothMove(subSelected.transform.position, subSpecialPanel.transform.position, easing));
                    subSelectedLocation = subSpecialPanel.transform.position;
                    break;
                case 4:
                    StartCoroutine(SubSmoothMove(subSelected.transform.position, subFDPanel.transform.position, easing));
                    subSelectedLocation = subFDPanel.transform.position;
                    break;
                case 3:
                    StartCoroutine(SubSmoothMove(subSelected.transform.position, subMainPanel.transform.position, easing));
                    subSelectedLocation = subMainPanel.transform.position;
                    break;
                case 2:
                    StartCoroutine(SubSmoothMove(subSelected.transform.position, subDrawPanel.transform.position, easing));
                    subSelectedLocation = subDrawPanel.transform.position;
                    break;
                case 1:
                    StartCoroutine(SubSmoothMove(subSelected.transform.position, subShopPanel.transform.position, easing));
                    subSelectedLocation = subShopPanel.transform.position;
                    break;
            }
        }
        else
        {
            StartCoroutine(SubSmoothMove(subSelected.transform.position, subSelectedLocation, easing));
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }
    }

    // Start is called before the first frame update
   private void Start()
    {
        panelLocation = transform.position;
        subSelected = GameObject.Find("Sub_Selected");
        subSpecialPanel = GameObject.Find("Sub_UISpecialEventPanel");
        subFDPanel = GameObject.Find("Sub_UIFriendPanel");
        subMainPanel = GameObject.Find("Sub_UIMain");
        subDrawPanel = GameObject.Find("Sub_UIDrawPanel");
        subShopPanel = GameObject.Find("Sub_UIShopPanel");
        subSelectedLocation = subSelected.GetComponent<RectTransform>().position;
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

    IEnumerator SubSmoothMove(Vector2 startPos, Vector2 endPos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime / seconds;
            subSelected.transform.position = Vector2.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

}
