using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position
{
    public enum Player { PlayerOne = 0, PlayerTwo }

    public static string TAG = "Position";

    /*{ "Flag", "Spy", "Private", "Sergeant", "2nd Lieutenant", "1st Lieutenant", 
        "Captain", "Major", "Lieutenant Colonel", "Colonel", "One-Star General", 
        "Two-Star General", "Three-Star General", "Four-Star General", "Five-Star General"} 1 - 15 piece index */
    private int pieceID = 0; //piece index - 1-15 flag,spy, .. 5 general
    private int pieceValue = 0; //piece value 1 - 15; 15-spy, 1-flag
    private int row = -1;
    private int column = -1;
    private int playerIndex = 0; //0 - player 1, 1 - player 2
    private int cloneIndex = 0;


    public static Position copyPosition(Position ps1, Position copy)
    {
        if (ps1 == null)
            return null;
        copy.PieceID = ps1.PieceID;
        copy.PieceValue = ps1.PieceValue;
        copy.Row = ps1.Row;
        copy.Column = ps1.Column;
        copy.PlayerIndex = ps1.PlayerIndex;
        copy.CloneIndex = ps1.CloneIndex;

        return copy;
    }

    public int PieceID
    {
        get { return pieceID; }
        set { pieceID = value; }
    }
    public int PieceValue
    {
        get { return pieceValue; }
        set { pieceValue = value; }
    }
    public int Row
    {
        get { return row; }
        set { row = value; }
    }
    public int Column
    {
        get { return column; }
        set { column = value; }
    }
    public int PlayerIndex
    {
        get { return playerIndex; }
        set { playerIndex = value; }
    }
    public int CloneIndex
    {
        get { return cloneIndex; }
        set { cloneIndex = value; }
    }
}
