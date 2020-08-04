using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    //Record the Tile position
    public int rowPos;
    public int colPos;
    private Color32 originalColor;

    //Record the OriginalColor
    private void Awake()
    {
        originalColor = this.gameObject.GetComponent<Image>().color;
    }

    //Resume Color from the original
    public void ResumeOriginalColor()
    {
        this.gameObject.GetComponent<Image>().color = originalColor;
    }
}
