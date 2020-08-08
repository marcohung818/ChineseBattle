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
    public string[] masterBoardOrder = new string[64];
    public int refreashCount;


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
                masterBoardOrder[transferBoardCount] = t.GetComponentInChildren<ElementRoot>().Word;
                transferBoardCount++;
            }
            this.GetComponent<PhotonView>().RPC("GenBoardImageByMasterOrder", RpcTarget.Others, (string[])masterBoardOrder);
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
        emptyList.Clear();
        foreach (Tile t in mainTileBoard)
        {
            if (t.transform.GetComponentInChildren<ElementRoot>().Word == "k")
            {
                emptyList.Add(t);
            }
        }
        GenBoardImageByRandom(emptyList);
    }

    #region DoReFillTile
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
    #endregion

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
    private void GenBoardImageByRandom(List<Tile> emptyList)
    {
        foreach (Tile emptyPos in emptyList)
        {
            int dice = UnityEngine.Random.Range(0, WordTypeHolder.instance.wordTypeList.Length);
            //emptyPos.transform.GetChild(0).GetComponent<ElementRoot>().Word = WordTypeHolder.instance.wordTypeList[dice].s_word;
            this.GetComponent<PhotonView>().RPC("ChangeDirectPos_All", RpcTarget.All, emptyPos.GetComponent<Tile>().rowPos, emptyPos.GetComponent<Tile>().colPos, WordTypeHolder.instance.wordTypeList[dice].s_word);
        }
    }

    //For the non master client generate its init board image
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
        if (imageList[0].GetComponent<ElementRoot>().Word == imageList[1].GetComponent<ElementRoot>().Word) //this have to change to real checking situation
        {
            WordShoot();
            UpdateEmptyTile();
        }
        else
        {
            foreach (GameObject word in imageList)
            {
                RPCShowAllAvailable(word.GetComponent<ChassBoardElement>().wordPos[0], word.GetComponent<ChassBoardElement>().wordPos[1]);
            }
            imageList.Clear();
        }
    }

    //After checking the word, if match then shoot, doing damage
    void WordShoot()
    {
        foreach (GameObject word in imageList)
        {
            RPCShowAllAvailable(word.GetComponent<ChassBoardElement>().wordPos[0], word.GetComponent<ChassBoardElement>().wordPos[1]);
            RPCDestoryWord(word.GetComponent<ChassBoardElement>().wordPos[0], word.GetComponent<ChassBoardElement>().wordPos[1]);
        }
        imageList.Clear();
    }
    #endregion

    //The user can only select the word, next to their final selected word
    //No Bug
    #region CheckTileLocation
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

    //Check the user point on the last 2 tile for remove the last tile in the clip array
    #region Unselect Tile
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
        RPCShowAllAvailable(imageList[imageListCountMax - 1].GetComponent<ChassBoardElement>().wordPos[0], imageList[imageListCountMax - 1].GetComponent<ChassBoardElement>().wordPos[1]);
        imageList.RemoveAt(imageListCountMax - 1);
    }
    #endregion
    
    //Show all user the block was selected
    #region RPCshowSelected
    public void RPCShowAllSelected(int row, int col)
    {
        this.GetComponent<PhotonView>().RPC("ShowAllSelected", RpcTarget.All, (int)row, (int)col);
    }

    [PunRPC]
    public void ShowAllSelected(int row, int col)
    {
        mainTileBoard[row, col].gameObject.GetComponentInChildren<ChassBoardElement>().SetSelected();
    }
    #endregion

    //Show all user the block was available
    #region RPCshowAvailable
    public void RPCShowAllAvailable(int row, int col)
    {
        this.GetComponent<PhotonView>().RPC("ShowAllAvailable", RpcTarget.All, (int)row, (int)col);
    }

    [PunRPC]
    public void ShowAllAvailable(int row, int col)
    {
        mainTileBoard[row, col].gameObject.GetComponentInChildren<ChassBoardElement>().SetAvailable();
    }
    #endregion

    //Change all user board image on the specific position
    #region RPCDestoryAlluserWord
    public void RPCDestoryWord(int row, int col)
    {
        this.GetComponent<PhotonView>().RPC("DestoryWord", RpcTarget.All, (int) row, (int) col);
        this.GetComponent<PhotonView>().RPC("ForceAllDoRefillTile", RpcTarget.All);
    }

    [PunRPC]
    public void DestoryWord(int row, int col)
    {
        mainTileBoard[row, col].gameObject.GetComponentInChildren<ElementRoot>().Word = "k";
    }

    [PunRPC]
    public void ForceAllDoRefillTile()
    {
        DoReFillTile();
    }

    [PunRPC]
    public void ForceAllUpdateEmptyTile()
    {
        UpdateEmptyTile();
    }

    [PunRPC]
    public void ChangeDirectPos_All(int row, int col, string word)
    {
        mainTileBoard[row, col].gameObject.GetComponentInChildren<ElementRoot>().Word = word;
    }
    #endregion

    [PunRPC]
    public void GenBoardImageByMasterOrder(string[] masterBoardOrder)
    {
        GenBoardImageByMasterRandom(masterBoardOrder);
    }

    #region RPCRefreshBoardforAll

    public void RaiseRefresh()
    {
        if(refreashCount == 0)
        {
            this.gameObject.GetComponent<PhotonView>().RPC("AddRefreshCount", RpcTarget.All);
            StartCoroutine(CountDown(5));
        }
    }

    [PunRPC]
    public void AddRefreshCount()
    {
        refreashCount++;
    }

    IEnumerator CountDown(int second)
    {
        int counter = second;
        while(counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
        }
    }
    #endregion
}
