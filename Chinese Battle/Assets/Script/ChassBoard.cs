using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.Linq;

public class ChassBoard : MonoBehaviour
{
    //Public variable List 
    [HideInInspector]public static ChassBoard instance;
    [HideInInspector]public bool pointerOnHold = false; //Check the pointer holding
    [HideInInspector]public Tile[,] mainTileBoard = new Tile[8, 8];
    [HideInInspector]public List<Tile> emptyList = new List<Tile>();
    public GameObject word;

    //Private variable List
    List<GameObject> imageList = new List<GameObject>(); //The List in clip
    List<Tile[]> column = new List<Tile[]>();

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
        column.Add(new Tile[] { mainTileBoard[0, 0], mainTileBoard[1, 0], mainTileBoard[2, 0], mainTileBoard[3, 0], mainTileBoard[4, 0], mainTileBoard[5, 0], mainTileBoard[6, 0], mainTileBoard[7, 0] });
        column.Add(new Tile[] { mainTileBoard[0, 1], mainTileBoard[1, 1], mainTileBoard[2, 1], mainTileBoard[3, 1], mainTileBoard[4, 1], mainTileBoard[5, 1], mainTileBoard[6, 1], mainTileBoard[7, 1] });
        column.Add(new Tile[] { mainTileBoard[0, 2], mainTileBoard[1, 2], mainTileBoard[2, 2], mainTileBoard[3, 2], mainTileBoard[4, 2], mainTileBoard[5, 2], mainTileBoard[6, 2], mainTileBoard[7, 2] });
        column.Add(new Tile[] { mainTileBoard[0, 3], mainTileBoard[1, 3], mainTileBoard[2, 3], mainTileBoard[3, 3], mainTileBoard[4, 3], mainTileBoard[5, 3], mainTileBoard[6, 3], mainTileBoard[7, 3] });
        column.Add(new Tile[] { mainTileBoard[0, 4], mainTileBoard[1, 4], mainTileBoard[2, 4], mainTileBoard[3, 4], mainTileBoard[4, 4], mainTileBoard[5, 4], mainTileBoard[6, 4], mainTileBoard[7, 4] });
        column.Add(new Tile[] { mainTileBoard[0, 5], mainTileBoard[1, 5], mainTileBoard[2, 5], mainTileBoard[3, 5], mainTileBoard[4, 5], mainTileBoard[5, 5], mainTileBoard[6, 5], mainTileBoard[7, 5] });
        column.Add(new Tile[] { mainTileBoard[0, 6], mainTileBoard[1, 6], mainTileBoard[2, 6], mainTileBoard[3, 6], mainTileBoard[4, 6], mainTileBoard[5, 6], mainTileBoard[6, 6], mainTileBoard[7, 6] });
        column.Add(new Tile[] { mainTileBoard[0, 7], mainTileBoard[1, 7], mainTileBoard[2, 7], mainTileBoard[3, 7], mainTileBoard[4, 7], mainTileBoard[5, 7], mainTileBoard[6, 7], mainTileBoard[7, 7] });

    }

    void UpdateEmptyTile()
    {
        DoReFillTile();
        foreach(Tile t in mainTileBoard)
        {
            if (t.transform.GetComponentInChildren<Element>().Word == "k")
            {
                emptyList.Add(t);
                print(t.name);
            }
        }
        GenBoardImageByRandom(emptyList);
        emptyList.Clear();
    }

    void DoReFillTile()
    {
        for (int i = 0; i < column.Count; i++)
        {
            while(ReFillTile(column[i]));
        }
    }

    bool ReFillTile(Tile[] colOfTiles)
    {
        for(int i = colOfTiles.Length - 1; i > 0; i--)
        {
            if(colOfTiles[i].GetComponentInChildren<Element>().Word == "k" && colOfTiles[i - 1].GetComponentInChildren<Element>().Word != "k")
            {
                colOfTiles[i].GetComponentInChildren<Element>().Word = colOfTiles[i - 1].GetComponentInChildren<Element>().Word;
                colOfTiles[i - 1].GetComponentInChildren<Element>().Word = "k";
                return true;
            }
        }
        return false;
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
            int dice = UnityEngine.Random.Range(0, WordTypeHolder.instance.wordTypeList.Length);
            Instantiate(word, t.transform);
            t.transform.GetChild(0).GetComponent<Element>().Word = WordTypeHolder.instance.wordTypeList[dice].s_word;
        }
    }

    private void GenBoardImageByRandom(List<Tile> emptyPosList)
    {
        foreach (Tile emptyPos in emptyPosList)
        {
            int dice = UnityEngine.Random.Range(0, WordTypeHolder.instance.wordTypeList.Length);
            emptyPos.transform.GetChild(0).GetComponent<Element>().Word = WordTypeHolder.instance.wordTypeList[dice].s_word;
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
        int imageListCountMax = imageList.Count;
        if (imageListCountMax == 1)
        {
            if (imageList[0].GetComponent<Element>().Word == "a")
            {
                WordShoot();
            }
            else
            {
                imageList[imageListCountMax - 1].GetComponent<Element>().SetAvailable();
                imageList.Clear();
            }
        }
        else
        {
            if (imageList[0].GetComponent<Element>().Word == imageList[1].GetComponent<Element>().Word) //this have to change to real checking situation
            {
                WordShoot();
            }
            else
            {
                foreach (GameObject word in imageList)
                {
                    word.GetComponent<Element>().SetAvailable();
                }
                imageList.Clear();
            }
        }
        UpdateEmptyTile();
    }

    //After checking the word, if match then shoot, doing damage
    void WordShoot()
    {
        foreach (GameObject word in imageList)
        {
            print(word.GetComponentInParent<Tile>().name);
            //DestroyImmediate(word);
            word.GetComponent<Element>().Word = "k";
            word.GetComponent<Element>().SetAvailable();
        }
        imageList.Clear();
        UpdateEmptyTile();
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
        if (check != 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //Check the user point on the last 2 tile for remove the last tile in the clip array
    public bool CheckPopClip(GameObject word)
    {
        int imageListCountMax = imageList.Count;
        if(imageList[imageListCountMax - 2].GetComponentInParent<Tile>().rowPos == word.GetComponentInParent<Tile>().rowPos &&
            imageList[imageListCountMax - 2].GetComponentInParent<Tile>().colPos == word.GetComponentInParent<Tile>().colPos)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Remove the last element of the imagelist
    public void PopClip()
    {
        int imageListCountMax = imageList.Count;
        imageList[imageListCountMax - 1].GetComponent<Element>().SetAvailable();
        imageList.RemoveAt(imageListCountMax - 1);
    }

}
