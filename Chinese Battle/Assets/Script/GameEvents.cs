using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    [HideInInspector] public static GameEvents instance;
    [HideInInspector] public bool pointerOnHold = false; //Check the pointer holding
    List<GameObject> imageList = new List<GameObject>(); //The List in clip

    //Set Singleton
    private void Awake()
    {
        instance = this;
    }

    //Set Action
    private void Start()
    {
        onRecordImage += RecordWording;
        onEndRecordImage += Checkwording;
    }

    //The Action set for the pointer down and drag
    private event Action<GameObject> onRecordImage;
    public void RecordImage(GameObject word)
    {
        if (onRecordImage != null)
        {
            onRecordImage(word);
        }
    }
    private void RecordWording(GameObject word)
    {
        imageList.Add(word);
    }

    //The Action set for the pointer up
    private event Action onEndRecordImage;
    public void EndRecordImage()
    {
        if (onEndRecordImage != null)
        {
            onEndRecordImage();
        }
    }
    private void Checkwording()
    {
        if(imageList[0].name == imageList[1].name)
        {
            WordShoot();
        }
        else
        {
            foreach (GameObject word in imageList)
            {
                word.GetComponent<WordMaster>().SetAvailable();
            }
            imageList.Clear();
        }
        ChassBoard.instance.updateEmptyTile();
    }

    //After checking the work, if match then shoot
    private void WordShoot()
    {
        foreach (GameObject word in imageList)
        {
            print(word.GetComponentInParent<Tile>().name);
            DestroyImmediate(word);
        }
        imageList.Clear();
    }
}
