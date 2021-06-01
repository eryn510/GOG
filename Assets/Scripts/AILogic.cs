using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILogic : MonoBehaviour
{
    List<GameObject> controlled = new List<GameObject>();
    List<GameObject> tileReference = new List<GameObject>();
    AIAgent agent = new AIAgent();
    BoardState currentBoard = new BoardState();
    
    public void addToList(GameObject[] arr)
    {
        this.controlled = new List<GameObject>();
        for (int i = 0; i < arr.Length; i++)
        {
            this.controlled.Add(arr[i]);
        }
    }

    public void addTilesList(GameObject[] arr)
    {
        this.tileReference = new List<GameObject>();
        for (int i = 0; i < arr.Length; i++)
        {
            this.tileReference.Add(arr[i]);
        }
    }
    public void setCurrent(BoardState board)
    {
        this.currentBoard = new BoardState();
        this.currentBoard = board;
    }

    void findInControlledList(Position pos)
    {
        //apparently for each turn, the returned objects iterate down
        foreach (var obj in this.controlled)
        {
            if (pos.PieceID == obj.GetComponent<Piece>().rank && (pos.Row != obj.GetComponent<Piece>().x || pos.Column != obj.GetComponent<Piece>().y) &&
                obj.activeSelf != false && pos.CloneIndex == obj.GetComponent<Piece>().cloneCount)
            {
                obj.GetComponent<Piece>().initX = obj.GetComponent<Piece>().x;
                obj.GetComponent<Piece>().initY = obj.GetComponent<Piece>().y;
                obj.GetComponent<Piece>().x = pos.Row;
                obj.GetComponent<Piece>().y = pos.Column;

                moveToTile(obj);
                return;
            }
        }
    }

    void moveToTile(GameObject obj)
    {
        foreach(var tile in this.tileReference)
        {
            if(obj.GetComponent<Piece>().x == tile.GetComponent<Tile>().x && obj.GetComponent<Piece>().y == tile.GetComponent<Tile>().y)
            {
                tile.GetComponent<Tile>().occupied = true;
                tile.GetComponent<Tile>().pieceOccupy = obj;
                obj.GetComponent<RectTransform>().anchoredPosition = tile.GetComponent<RectTransform>().anchoredPosition;
                obj.GetComponent<RectTransform>().localPosition = tile.GetComponent<RectTransform>().localPosition;
                obj.GetComponent<Piece>().isAttack = true;

                Debug.Log("Name: " + obj.name);
                return;
            }
        }
    }

    Position getChangedPos(BoardState bestBoard)
    {
        foreach (var item in bestBoard.getPositionList(1))
        {
            foreach (var pos in this.currentBoard.getPositionList(1))
            {
                if ((item.Column != pos.Column || item.Row != pos.Row) && item.PieceID == pos.PieceID && 
                    item.CloneIndex == pos.CloneIndex && item.PieceValue == pos.PieceValue)
                {
                    //pos.Column = item.Column;
                    //pos.Row = item.Row;
                    return item;
                }
            }
        }
        return null;
    }

    void changeCorrespondingPos(BoardState bestBoard)
    {
        Position holdPos = new Position();
        holdPos = this.getChangedPos(bestBoard);
        //Debug.Log("PieceValue: " + holdPos.PieceValue + " Position: " + holdPos.Row + ":" + holdPos.Column);
        this.findInControlledList(holdPos);
        //Debug.Log("Previous Position: " + obj.GetComponent<Piece>().initX + ":" + obj.GetComponent<Piece>().initY);
        //Debug.Log("Position: " + obj.GetComponent<Piece>().x + ":" + obj.GetComponent<Piece>().y);
    }

    public void execute()
    {
        BoardState bestBoard = new BoardState();

        bestBoard = this.currentBoard.addEvalGetBest();
        
        /*
        Debug.Log("ARRAY OF PIECE Positions");
        foreach (var item in bestBoard.getPositionList(1))
        {
            Debug.Log("PieceValue: " + item.PieceValue + " Position: " + item.Row + ":" + item.Column + "Clone :" + item.CloneIndex);
        }
        Debug.Log("BEST MOVE:" + bestBoard.UnitMovement);
        */
        Debug.Log("PIECE VALUE:" + bestBoard.getPositionList(0)[0].PieceValue);

        changeCorrespondingPos(bestBoard);
        this.setCurrent(bestBoard);
    }

}
