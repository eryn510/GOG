using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Vector3 preDrag;
    public Vector3 initPiecePos;

    public int rank;
    public int trueRank;
    //false is White, true is Black
    public bool color;
    public bool notClip;
    public bool move;
    public int x, y;
    public int initX, initY;
    public int cloneCount = 0;
    public bool isPlaced = false;
    public bool isAttack = false;
    public int suspectedValue = 0;
    int i;

    public int[,] boardcopy = new int[8, 9];

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        move = true;
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //No drag if move is false
        if (!move)
        {
            eventData.pointerDrag = null;
        }
        //Drag if move is true
        else
        {
            preDrag = GetComponent<RectTransform>().anchoredPosition;
            transform.parent.SetAsLastSibling();
            transform.SetAsLastSibling();
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }
    }


    /*GUIDE
     * 
     * FindObjectOfType<GridTiles>().isReady == false OR FindObjectOfType<GridTiles>().isReady == true
     * ^^ pangcheck if isReady == true/false sa GridTiles. (once na nagclick ready si player)
     * 
     */
    public void OnDrag(PointerEventData eventData)
    {
        FindObjectOfType<GridTiles>().grid[initX, initY].GetComponent<Tile>().pieceOccupy = null;
        FindObjectOfType<GridTiles>().grid[initX, initY].GetComponent<Tile>().occupied = false;
        //HORIZONTAL CLIPPING
        if ((rectTransform.anchoredPosition.x <= 381.0f && rectTransform.anchoredPosition.x >= -873.6f)
            && FindObjectOfType<GridTiles>().isReady == false)
        {
            //rectTransform.anchoredPosition += eventData.delta;
        }

        if (rectTransform.anchoredPosition.x > 381.0f)
        {
            rectTransform.localPosition = new Vector3(381.0f, rectTransform.anchoredPosition.y, 0);
        }
        else if (rectTransform.anchoredPosition.x < -873.6f)
        {
            rectTransform.localPosition = new Vector3(-873.6f, rectTransform.anchoredPosition.y, 0);
        }

        if (FindObjectOfType<GridTiles>().isReady == false)
        {
            //VERTICAL CLIPPING
            //VALUES
            //-50.0 = MIDDLE OF BOARD
            //-459.2 = BOTTOM END OF BOARD
            //464 = TOP END OF BOARD
            //83.9 = BOTTOM END OF BLACK SIDE
            if (rectTransform.anchoredPosition.y > -200.0f && color == false) //white pieces going to black
            {
                rectTransform.localPosition = new Vector3(rectTransform.anchoredPosition.x, -215.0f, 0);
                rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x, -215.0f, 0);
            }
            else if (rectTransform.anchoredPosition.y < 205.0f && color == true) //black pieces going to white
            {
                rectTransform.localPosition = new Vector3(rectTransform.anchoredPosition.x, 205.0f, 0);
            }
        }

        if(FindObjectOfType<GridTiles>().isReady == true)
        {
            if (rectTransform.anchoredPosition.y > 464 && (color == true || color == false)) //black pieces going up
            {
                rectTransform.localPosition = new Vector3(rectTransform.anchoredPosition.x, 464, 0);
            }
            if (rectTransform.anchoredPosition.y < -459.2f && (color == false || color == true)) //white pieces going down
            {
                rectTransform.localPosition = new Vector3(rectTransform.anchoredPosition.x, -429.2f, 0);
            }
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if(FindObjectOfType<GridTiles>().isReady == false)
        {
           
            if (rectTransform.localPosition.y > -200.0f && color == false) //white pieces going to black
            {
                x = initX;
                y = initY;
                //rectTransform.localPosition = new Vector3(rectTransform.anchoredPosition.x, -210.0f, 0);
                FindObjectOfType<GridTiles>().grid[x, y].GetComponent<Tile>().pieceOccupy = gameObject;
                FindObjectOfType<GridTiles>().grid[x, y].GetComponent<Tile>().occupied = true;
                //rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x, -215.0f, 0);
                rectTransform.anchoredPosition = new Vector3(initPiecePos.x, initPiecePos.y, 0);
            }
        }


        if (FindObjectOfType<GridTiles>().isReady == true && ((FindObjectOfType<GridTiles>().currentPlayer == "White") ||
            (FindObjectOfType<GridTiles>().currentPlayer == "Black")))
        {
    

            //Y IS LEFT-RIGHT, X IS UP-DOWN
            
            if ((y == initY + 1 && x == initX + 0) || (y == initY - 1 && x == initX + 0) ||
                (y == initY + 0 && x == initX + 1) || (y == initY + 0 && x == initX - 1))
            {
                SFXManager.SFXInstance.playMove();
                isAttack = true;
                //Debug.Log("piece drop"); 
                //CALL THE UPDATE BOARD COPY HERE
            }
            else if (y == initX && x == initX)
            {
                if (color == false)
                    FindObjectOfType<GridTiles>().currentPlayer = "White";
                else
                    FindObjectOfType<GridTiles>().currentPlayer = "Black";
            }
            else
            {
                //Debug.Log("invalid");
                x = initX;
                y = initY; 
                FindObjectOfType<GridTiles>().grid[initX, initY].GetComponent<Tile>().pieceOccupy = gameObject;
                FindObjectOfType<GridTiles>().grid[initX, initY].GetComponent<Tile>().occupied = true;
                rectTransform.anchoredPosition = new Vector3(initPiecePos.x, initPiecePos.y, 0);
                if (color == false)
                    FindObjectOfType<GridTiles>().currentPlayer = "White";
                else
                    FindObjectOfType<GridTiles>().currentPlayer = "Black";
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        initX = x;
        initY = y;
        initPiecePos = rectTransform.localPosition;
        //Debug.Log(FindObjectOfType<GridTiles>().GetComponent<Tile>().GetX());
        //Debug.Log("hello: " + x);
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("piece drop");
        //Debug.Log(FindObjectOfType<GridTiles>().GetComponent<Tile>().GetX());
    }

    void Start()
    {
        //ACCESSING BOARD COPY FROM GRID TILES
        //FindObjectOfType<GridTiles>().boardCopy.CopyTo(boardcopy,0);
        //boardcopy = (int[,])FindObjectOfType<GridTiles>().boardCopy;

        //Debug.Log("hello: " + FindObjectOfType<GridTiles>().boardCopy[0, 0]);

        //notClip = false;
    }
}
