using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 */

public class Tile : MonoBehaviour, IDropHandler
{
    public bool occupied = false;
    public int x , y;
    public GameObject pieceOccupy;
    GameObject[] player = new GameObject[21];
    GameObject[] enemy = new GameObject[21];
    GameObject dragged;
    public Vector2 dropPos;

    int i = 0;

    public int GetX()
    {
        return x;
    }
    public int GetY()
    {
        return y;
    }

    /* KNOWN BUGS:
     * If Black reaches White Tiles, 2 Black Pieces can occupy one white tile. Same as vice versa
     * So far yun lang nakita ko
     */
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log(eventData.pointerDrag.GetComponent<Piece>().x);
        //Debug.Log(eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition);
        //Debug.Log(GetX());

        dropPos.x = GetX();
        dropPos.y = GetY();




        //Debug.Log(eventData.pointerDrag.GetComponent<RectTransform>().localPosition);
        /*
        if (eventData.pointerDrag.GetComponent<RectTransform>().position.x == eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition.x &&
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition.y <= -79.0f && 
            eventData.pointerDrag.GetComponent<Piece>().color == false)
        {
            Debug.Log("valid");
        }
        */



        if (eventData.pointerDrag != null)
        {
            if (occupied)
            {
                /*
                if (pieceOccupy.GetComponent<Piece>().color == eventData.pointerDrag.gameObject.GetComponent<Piece>().color || eventData.pointerDrag.gameObject.GetComponent<Piece>().color == pieceOccupy.GetComponent<Piece>().color)
                {
                    if (pieceOccupy.gameObject.activeSelf == false)
                    {
                        Debug.Log(pieceOccupy.gameObject.GetComponent<Piece>().x);
                        Debug.Log(pieceOccupy.gameObject.GetComponent<Piece>().y);
                        Debug.Log(x);
                        Debug.Log(y);
                        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                        pieceOccupy = eventData.pointerDrag.gameObject;
                        occupied = true;
                        eventData.pointerDrag.GetComponent<Piece>().x = x;
                        eventData.pointerDrag.GetComponent<Piece>().y = y;

                        if (eventData.pointerDrag.GetComponent<Piece>().color == false)
                        {
                            FindObjectOfType<GridTiles>().currentPlayer = "Black";
                        }
                        else
                        {
                            FindObjectOfType<GridTiles>().currentPlayer = "White";
                        }
                    }
                    else if (pieceOccupy.gameObject.activeSelf == true)
                    {
                        Debug.Log(pieceOccupy.gameObject.GetComponent<Piece>().x);
                        Debug.Log(pieceOccupy.gameObject.GetComponent<Piece>().y);
                        Debug.Log(x);
                        Debug.Log(y);
                        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                        pieceOccupy = eventData.pointerDrag.gameObject;
                        occupied = true;
                        eventData.pointerDrag.GetComponent<Piece>().x = x;
                        eventData.pointerDrag.GetComponent<Piece>().y = y;

                        if (eventData.pointerDrag.GetComponent<Piece>().color == false)
                        {
                            FindObjectOfType<GridTiles>().currentPlayer = "Black";
                        }
                        else
                        {
                            FindObjectOfType<GridTiles>().currentPlayer = "White";
                        }
                        
                    }
                }
                */

                if (pieceOccupy.GetComponent<Piece>().color != eventData.pointerDrag.gameObject.GetComponent<Piece>().color || eventData.pointerDrag.gameObject.GetComponent<Piece>().color != pieceOccupy.gameObject.GetComponent<Piece>().color)
                {
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                    eventData.pointerDrag.GetComponent<Piece>().x = x;
                    eventData.pointerDrag.GetComponent<Piece>().y = y;
                    pieceOccupy = eventData.pointerDrag.gameObject;
                    occupied = true;

                    if (eventData.pointerDrag.GetComponent<Piece>().color == false)
                    {
                        FindObjectOfType<GridTiles>().currentPlayer = "Black";
                    }
                    else
                    {
                        FindObjectOfType<GridTiles>().currentPlayer = "White";
                    }
                }
                
            }
            else
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                pieceOccupy = eventData.pointerDrag.gameObject;
                occupied = true;
                eventData.pointerDrag.GetComponent<Piece>().x = x;
                eventData.pointerDrag.GetComponent<Piece>().y = y;

                if (eventData.pointerDrag.GetComponent<Piece>().color == false)
                {
                    FindObjectOfType<GridTiles>().currentPlayer = "Black";
                }
                else
                {
                    FindObjectOfType<GridTiles>().currentPlayer = "White";
                }
            }

           


        }
        //}
       // else
        //    Debug.Log(pieceOccupy.name);

    }

   void updatePieceOccupy()
    {
        for (i = 0; i < player.Length; i++)
        {
            if (player[i].GetComponent<Piece>().x == x && player[i].GetComponent<Piece>().y == y)
            {
                //Debug.Log(player[i].GetComponent<Piece>().x);

                pieceOccupy = player[i];
                occupied = true;
                break;
            }
            else
            {
                occupied = false;
                pieceOccupy = null;
            }
        }
        for (i = 0; i < enemy.Length; i++)
        {
            if (enemy[i].GetComponent<Piece>().x == x && enemy[i].GetComponent<Piece>().y == y)
            {
                pieceOccupy = enemy[i];
                occupied = true;
                break;
            }
            else
            {
                occupied = false;
                pieceOccupy = null;
            }
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player");
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        gameObject.tag = "Tile";

        updatePieceOccupy();
        
    }

    void Update()
    {
        pieceOccupy = null;
        occupied = false;

        updatePieceOccupy();

        if (pieceOccupy != null)
        {
            if (pieceOccupy.gameObject.activeSelf == false)
            {
                pieceOccupy = null;
                occupied = false;
            }
        }
    }
}
