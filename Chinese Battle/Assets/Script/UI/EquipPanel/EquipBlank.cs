using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipBlank : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int wordArray;
    [SerializeField] private int wordLocation;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (WordEquipPanel.instance.selecting)
        {
            this.gameObject.GetComponent<ElementRoot>().Word = WordEquipPanel.instance.selectingWord;
            if(wordArray == 1)
            {
                WordEquipPanel.instance.skillWordArray_1[wordLocation] = WordEquipPanel.instance.selectingWord;
            }
            else
            {
                WordEquipPanel.instance.skillWordArray_2[wordLocation] = WordEquipPanel.instance.selectingWord;
            }
            WordEquipPanel.instance.selecting = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
