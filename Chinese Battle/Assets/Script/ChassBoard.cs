using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

public class ChassBoard : MonoBehaviourPun
{
    //Public variable List 
    [HideInInspector] public static ChassBoard instance;
    [HideInInspector] public bool pointerOnHold = false; //Check the pointer holding
    [HideInInspector] public Tile[,] mainTileBoard = new Tile[8, 8];
    [HideInInspector] public List<Tile> emptyList = new List<Tile>();
    [HideInInspector] public bool onDrag = false;
    public GameObject bullet;
    public string[] stringboard = new string[64];

    //Private variable List
    private List<GameObject> imageList = new List<GameObject>(); //The List in clip
    private List<Tile[]> column = new List<Tile[]>();
    [SerializeField] private GameObject wordObject;

    //Set Singleton
    private void Awake()
    {
        instance = this;
    }
    
    private void Start()
    {
        RecordTiles();
        //For Synchronize the Master and Client
        if (PhotonNetwork.IsMasterClient)
        {
            GenBoardImageByRandom();
            int transferBoardCount = 0;
            foreach (Tile t in mainTileBoard)
            {
                stringboard[transferBoardCount] = t.GetComponentInChildren<ElementRoot>().Word;
                transferBoardCount++;
            }
            this.GetComponent<PhotonView>().RPC("GenBoardImageByMasterOrder", RpcTarget.Others, (string[])stringboard);
        }
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

    //call fill up, and
    private void UpdateEmptyTile()
    {
        DoReFillTile();
        foreach(Tile t in mainTileBoard)
        {
            if (t.transform.GetComponentInChildren<ElementRoot>().Word == "k")
            {
                emptyList.Add(t);
            }
        }
        GenBoardImageByRandom(emptyList);
        emptyList.Clear();
    }

    //let the emptyed space filled up by the upper word
    private void DoReFillTile()
    {
        for (int i = 0; i < column.Count; i++)
        {
            while(ReFillTile(column[i]));
        }
    }

    //key function for the fill up
    private bool ReFillTile(Tile[] colOfTiles)
    {
        for(int i = colOfTiles.Length - 1; i > 0; i--)
        {
            if(colOfTiles[i].GetComponentInChildren<ElementRoot>().Word == "k" && colOfTiles[i - 1].GetComponentInChildren<ElementRoot>().Word != "k")
            {
                colOfTiles[i].GetComponentInChildren<ElementRoot>().Word = colOfTiles[i - 1].GetComponentInChildren<ElementRoot>().Word;
                colOfTiles[i - 1].GetComponentInChildren<ElementRoot>().Word = "k";
                return true;
            }
        }
        return false;
    }

    //Record the Tile to form a TileBoard Array
    private void RecordTiles()
    {
        Tile[] inputTileBoard = GameObject.FindObjectsOfType<Tile>();
        foreach (Tile t in inputTileBoard)
        {
            mainTileBoard[t.rowPos, t.colPos] = t;
        }
    }

    #region BoardImageGenerator
    //called once, for in game first generate
    private void GenBoardImageByRandom()
    {
        foreach(Tile t in mainTileBoard)
        {
            int dice = UnityEngine.Random.Range(0, WordTypeHolder.instance.wordTypeList.Length);
            var word = Instantiate(wordObject, t.transform);
            word.GetComponent<ElementRoot>().Word = WordTypeHolder.instance.wordTypeList[dice].s_word;
        }
    }

    //regenerate the word for empty space 
    private void GenBoardImageByRandom(List<Tile> emptyPosList)
    {
        foreach (Tile emptyPos in emptyPosList)
        {
            int dice = UnityEngine.Random.Range(0, WordTypeHolder.instance.wordTypeList.Length);
            emptyPos.transform.GetChild(0).GetComponent<ElementRoot>().Word = WordTypeHolder.instance.wordTypeList[dice].s_word;
        }
    }

    private void GenBoardImageByMasterRandom(string [] masterBoardOrder)
    {
        int masterBoardCount = 0;
        foreach(Tile t in mainTileBoard)
        {
            var word = Instantiate(wordObject, t.transform);
            word.GetComponent<ElementRoot>().Word = masterBoardOrder[masterBoardCount];
            masterBoardCount++;
        }
    }

    public void RefreshBoardImageByRandom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            print("this is master");
            int transferBoardCount = 0;
            foreach (Tile t in mainTileBoard)
            {
                int dice = UnityEngine.Random.Range(0, WordTypeHolder.instance.wordTypeList.Length);
                t.transform.GetChild(0).GetComponent<ElementRoot>().Word = WordTypeHolder.instance.wordTypeList[dice].s_word;
                stringboard[transferBoardCount] = WordTypeHolder.instance.wordTypeList[dice].s_word;
                transferBoardCount++;
            }
            this.GetComponent<PhotonView>().RPC("RequestRefreshBoard", RpcTarget.Others, (string[])stringboard);
        }
    }
    
    private void RefreshBoardImageByMasterRandom(string[] masterBoardOrder)
    {
        int masterBoardCount = 0;
        foreach (Tile t in mainTileBoard)
        {
            t.GetComponentInChildren<ElementRoot>().Word = masterBoardOrder[masterBoardCount];
            masterBoardCount++;
        }
    }
    #endregion

    #region OnRecordImage
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
    #endregion

    //The Action set for the pointer up
    #region OnEndRecordImage
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
            if (imageList[0].GetComponent<ElementRoot>().Word == "a")
            {
                WordShoot();
            }
            else
            {
                RPCShowAllAvailable(imageList[imageListCountMax - 1].GetComponent<ElementRoot>().wordPos);
                imageList.Clear();
            }
        }
        else
        {
            if (imageList[0].GetComponent<ElementRoot>().Word == imageList[1].GetComponent<ElementRoot>().Word) //this have to change to real checking situation
            {
                WordShoot();
            }
            else
            {
                foreach (GameObject word in imageList)
                {
                    RPCShowAllAvailable(word.GetComponent<ElementRoot>().wordPos);
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
            word.GetComponent<ElementRoot>().Word = "k";
            RPCShowAllAvailable(word.GetComponent<ElementRoot>().wordPos);
        }
        imageList.Clear();
        UpdateEmptyTile();
    }
    #endregion

    #region CheckTileLocation
    //Force the user can only select the word next to the clip final word
    public bool CheckTileDiff(GameObject word)
    {
        int imageListCountMax = imageList.Count;
        int rowDiff = imageList[imageListCountMax - 1].GetComponentInParent<Tile>().rowPos -
            word.GetComponentInParent<Tile>().rowPos;
        int colDiff = imageList[imageListCountMax - 1].GetComponentInParent<Tile>().colPos -
            word.GetComponentInParent<Tile>().colPos;
        int check = rowDiff + colDiff;
        if ((check == 1 || check == -1) && (rowDiff >= -1 && rowDiff <= 1) && (colDiff >= -1 && colDiff <= 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region Unselect Tile
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
        if(imageListCountMax  == 0)
        {
            return;
        }
        imageList[imageListCountMax - 1].GetComponent<ChassBoardElement>().SetAvailable();
        RPCShowAllAvailable(imageList[imageListCountMax - 1].GetComponent<ElementRoot>().wordPos);
        imageList.RemoveAt(imageListCountMax - 1);
    }
    #endregion
    
    #region RPCRelated
    public void RPCShowAllSelected(int[] SelectedArray)
    {
        this.GetComponent<PhotonView>().RPC("ShowAllSelected", RpcTarget.All, (int[])SelectedArray);
    }

    public void RPCShowAllAvailable(int[] SelectedArray)
    {
        this.GetComponent<PhotonView>().RPC("ShowAllAvailable", RpcTarget.All, (int[])SelectedArray);
    }

    public void RPCCahngeAllEmptyImage(int[] SelectedArray)
    {
        this.GetComponent<PhotonView>().RPC("ChangeAllEmptyImage", RpcTarget.All, (int[])SelectedArray);
    }

    [PunRPC]
    public void GenBoardImageByMasterOrder(string[] masterBoardOrder)
    {
        GenBoardImageByMasterRandom(masterBoardOrder);
    }

    [PunRPC]
    public void ShowAllSelected(int[] wordPos)
    {
        mainTileBoard[wordPos[0], wordPos[1]].gameObject.GetComponentInChildren<ChassBoardElement>().SetSelected();
    }

    [PunRPC]
    public void ShowAllAvailable(int[] wordPos)
    {
        mainTileBoard[wordPos[0], wordPos[1]].gameObject.GetComponentInChildren<ChassBoardElement>().SetAvailable();
    }

    [PunRPC]
    public void RequestRefreshBoard(string[] masterBoardOrder)
    {
        RefreshBoardImageByMasterRandom(masterBoardOrder);
    }

    [PunRPC]
    public void ChangeAllEmptyImage(int[] wordPos)
    {

    }
    #endregion
}
