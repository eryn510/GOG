using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState
{
    public enum Player { PlayerOne = 0, PlayerTwo }

    public static string TAG = "BoardState";
    public static int MAX_ROW = 8;
    public static int MAX_COL = 9;

    private double MAX_INFINITY = 99999.0d;

    private List<Position> playerOnePosition = new List<Position>();
    private List<Position> playerTwoPosition = new List<Position>();

    private int whoseTurnToMove; //0 - player1, 1 - player2

    private List<double> evalScoreList = new List<double>();

    public Position movePosition = new Position();

    public int unitMovement = 0; //(0-left, 1-right, 2-up, 3-down)

    private int winCount = 0;

    public int UnitMovement
    {
        get { return unitMovement; }
        set { unitMovement = value; }
    }

    //function for copying board states
    public static void boardCopy(BoardState bs1, BoardState copy)
    {
        foreach (var item in bs1.getPositionList(0))
        {
            copy.getPositionList(0).Add(item);
        }
        foreach (var item in bs1.getPositionList(1))
        {
            copy.getPositionList(1).Add(item);
        }
        copy.WhoseTurnToMove = bs1.whoseTurnToMove;
        copy.UnitMovement = bs1.UnitMovement;
    }

    //function for evaluating boards and getting the best among the boards
    //eval is stored in this reference class
    public BoardState addEvalGetBest()
    {
        BoardState hold = new BoardState();
        boardCopy(this, hold);

        List<BoardState> newBoards = AIAgent.exploreNextMoves(hold, whoseTurnToMove);
        
        //WE CAN IMPLEMENT THE AB PRUNING ALGO HERE, reference nalang sa connect4 code

        for (int i = 0; i < newBoards.Count; i++)
        {
            BoardState hold1 = new BoardState();
            boardCopy(newBoards[i], hold1);
            BoardEvaluator boardEval = new BoardEvaluator(hold1);
            this.evalScoreList.Add(boardEval.evaluate());
        }

        double bestMove = -MAX_INFINITY;
        int index = 0;

        for (int i = 0; i < evalScoreList.Count; i++)
        {
            //Debug.Log("Score: " + evalScoreList[i]);
            if(evalScoreList[i] > bestMove)
            {
                bestMove = evalScoreList[i];
                index = i;
            }
        }
        Debug.Log("Best: " + bestMove);
        return newBoards[index];
    }

    //function for deleting a certain piece in the position list by passing position object itself and the pplayer it belongs to
    public void deletePosition(Position pos, int player)
    {
        if (player == (int)Player.PlayerOne)
        {
            foreach (var item in this.getPositionList(0))
            {
                if (item.Row == pos.Row && item.Column == pos.Column)
                {
                    this.getPositionList(0).Remove(item);
                    break;
                }
            }
        }
        else
        {
            foreach (var item in this.getPositionList(1))
            {
                if (item.Row == pos.Row && item.Column == pos.Column)
                {
                    this.getPositionList(1).Remove(item);
                    break;
                }
            }
        }
    }

    //function for deleting a certain piece in the position by using its pieceID, value, and the player it belongs to
    public void deletePosition(int pieceId, int pieceValue, int player)
    {
        if (player == (int)Player.PlayerOne)
        {
            foreach (var pos in playerOnePosition)
            {
                if (pos.PieceID == pieceId && pos.PieceValue == pieceValue)
                {
                    playerOnePosition.Remove(pos);
                    return;
                }
            }
        }
        else
        {
            foreach (var pos in playerTwoPosition)
            {
                if (pos.PieceID == pieceId && pos.PieceValue == pieceValue)
                {
                    playerTwoPosition.Remove(pos);
                    return;
                }
            }
        }
    }

    //for checking 
    public bool isThereAPosition(int player, int row, int col)
    {
        if (player == (int)Player.PlayerOne)
        {
            foreach (var pos in playerOnePosition)
            {
                if (pos.Row == row && pos.Column == col)
                {
                    return true;
                }
            }
        }
        else
        {
            foreach (var pos in playerTwoPosition)
            {
                if (pos.Row == row && pos.Column == col)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //for checking or setting the turn
    public int WhoseTurnToMove
    {
        get { return whoseTurnToMove; }
        set { whoseTurnToMove = value; }
    }

    //for getting the pos obj in the list by passing its row and col
    public Position getPositionAtPlace(int row, int col)
    {
        foreach (var pos in playerOnePosition)
        {
            if (pos.Row == row && pos.Column == col)
            {
                return pos;
            }
        }
        foreach (var pos in playerTwoPosition)
        {
            if (pos.Row == row && pos.Column == col)
            {
                return pos;
            }
        }
        return null;
    }

    //for getting the pos obj details
    public Position getPositionAt(int player, int i)
    {
        if (player == (int)Player.PlayerOne)
        {
            return playerOnePosition[i];
        }
        else
        {
            return playerTwoPosition[i];
        }
    }

    //for getting the pos list of a player
    public List<Position> getPositionList(int player)
    {
        if (player == (int)Player.PlayerOne)
        {
            return playerOnePosition;
        }
        else
        {
            return playerTwoPosition;
        }
    }

    //for getiing the size of pos of a certain player
    public int getPositionSize(int player)
    {
        if (player == (int)Player.PlayerOne)
        {
            return playerOnePosition.Count;
        }
        else
        {
            return playerTwoPosition.Count;
        }
    }

    //add pos obj to list
    public void addFromPosList(int player, Position position)
    {
        if (player == (int)Player.PlayerOne)
        {
            playerOnePosition.Add(position);
        }
        else
        {
            playerTwoPosition.Add(position);
        }
    }

    //add pos list to this class' pos list
    public void addFromPosList(int player, List<Position> positions)
    {
        if (player == (int)Player.PlayerOne)
        {
            foreach (var position in positions)
            {
                playerOnePosition.Add(position);
            }
        }
        else
        {
            foreach (var position in positions)
            {
                playerTwoPosition.Add(position);
            }
        }
    }

    public void addFromScoList(double eval)
    {
        evalScoreList.Add(eval);
    }

    public void addFromScoList(List<double> evals)
    {
        foreach (var eval in evals)
        {
            evalScoreList.Add(eval);
        }
    }

    public void incWinCount()
    {
        winCount++;
    }

}
