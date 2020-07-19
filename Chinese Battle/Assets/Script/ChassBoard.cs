using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChassBoard : MonoBehaviour
{
    [HideInInspector]public static ChassBoard instance;
    [SerializeField] private GameObject[] imagelist;
    [HideInInspector] public bool pointerOnHold = false; //Check the pointer holding
    List<GameObject> imageList = new List<GameObject>(); //The List in clip
    [HideInInspector]public Tile[,] mainTileBoard = new Tile[8, 8];
    [HideInInspector]public List<Tile> emptyList = new List<Tile>();

    //Set Singleton
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        RecordTiles();
        GenBoardImageByRandom();
        onRecordImage += RecordWording;
        onEndRecordImage += Checkwording;
    }

    void UpdateEmptyTile()
    {
        emptyList.Clear();
        foreach(Tile t in mainTileBoard)
        {
            if(t.transform.childCount == 0)
            {
                emptyList.Add(t);
            }
        }
        RefillEmptyTile();
    }

    void RefillEmptyTile()
    {
        GenBoardImageByRandom(emptyList);
    }

    void RecordTiles()
    {
        Tile[] inputTileBoard = GameObject.FindObjectsOfType<Tile>();
        foreach (Tile t in inputTileBoard)
        {
            mainTileBoard[t.rowPos, t.colPos] = t;
        }
    }

    private void GenBoardImageByRandom()
    {
        foreach(Tile t in mainTileBoard)
        {
            int dice = UnityEngine.Random.Range(0, imagelist.Length);
            Instantiate(imagelist[dice], t.transform);
        }
    }

    private void GenBoardImageByRandom(List<Tile> emptyPosList)
    {
        foreach (Tile emptyPos in emptyPosList)
        {
            int dice = UnityEngine.Random.Range(0, imagelist.Length);
            Instantiate(imagelist[dice], emptyPos.transform);
        }
    }


    //The Action set for the pointer down and drag
    event Action<GameObject> onRecordImage;
    public void RecordImage(GameObject word)
    {
        if (onRecordImage != null)
        {
            onRecordImage(word);
        }
    }
    void RecordWording(GameObject word)
    {
        imageList.Add(word);
    }

    //The Action set for the pointer up
    event Action onEndRecordImage;
    public void EndRecordImage()
    {
        if (onEndRecordImage != null)
        {
            onEndRecordImage();
        }
    }
    void Checkwording()
    {
        if (imageList[0].name == imageList[1].name)
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
        UpdateEmptyTile();
    }

    //After checking the word, if match then shoot
    void WordShoot()
    {
        foreach (GameObject word in imageList)
        {
            print(word.GetComponentInParent<Tile>().name);
            DestroyImmediate(word);
        }
        imageList.Clear();
    }

    //Force the user can only select the word next to the clip final word
    public bool CheckTileDiff(GameObject word)
    {
        int imageListCountMax = imageList.Count;
        int rowDiff = imageList[imageListCountMax - 1].GetComponentInParent<Tile>().rowPos -
            word.GetComponentInParent<Tile>().rowPos;
        int colDiff = imageList[imageListCountMax - 1].GetComponentInParent<Tile>().colPos -
            word.GetComponentInParent<Tile>().colPos;
        int check = rowDiff + colDiff;
        if (check < 0)
        {
            check *= -1;
        }
        print("check:" + check);
        if (check > 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
