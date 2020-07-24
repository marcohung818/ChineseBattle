using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    //Record the Tile position
    public int rowPos;
    public int colPos;
    Color32 originalColor;

    void Awake()
    {
        originalColor = this.gameObject.GetComponent<Image>().color;
    }

    public void ResumeOriginalColor()
    {
        this.gameObject.GetComponent<Image>().color = originalColor;
    }
}
