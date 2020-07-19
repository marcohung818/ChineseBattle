using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChassBoard : MonoBehaviour
{
    [HideInInspector]public static ChassBoard instance;
    [SerializeField] private GameObject[] imagelist;
    public Tile[,] mainTileBoard = new Tile[8, 8];
    public List<Tile> emptyList = new List<Tile>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        RecordTiles();
        GenBoardImageByRandom();
    }

    public void updateEmptyTile()
    {
        emptyList.Clear();
        foreach(Tile t in mainTileBoard)
        {
            if(t.transform.childCount == 0)
            {
                emptyList.Add(t);
            }
        }
        refillEmptyTile();
    }

    public void refillEmptyTile()
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

    void GenBoardImageByRandom()
    {
        foreach(Tile t in mainTileBoard)
        {
            int dice = Random.Range(0, imagelist.Length);
            Instantiate(imagelist[dice], t.transform);
        }
    }

    void GenBoardImageByRandom(List<Tile> emptyPosList)
    {
        foreach (Tile emptyPos in emptyPosList)
        {
            int dice = Random.Range(0, imagelist.Length);
            Instantiate(imagelist[dice], emptyPos.transform);
        }
    }
}
