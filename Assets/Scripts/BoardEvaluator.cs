using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardEvaluator
{
    public enum Player { PlayerOne = 0, PlayerTwo = 1 }
    public static string Tag = "BoardEvaluator";
    private BoardState boardState;

    public static float OFFENSE_MULTIPLIER = 10.0f; //if you want to be aggressive
    public static float DEFENSE_DEDUCTION = 5.0f; //if you want to be more defensive
    public static float OPENNESS_VALUE = 25.0f; //if you want to explore more
    public static float WIN_LOSS_VALUE = 90000.0f;

    //constructor
    public BoardEvaluator(BoardState boardState) { this.boardState = boardState; }

    public BoardState boardstate
    {
        get { return boardState; }
    }

    //simulation; evaluate each position's score in the boardState
    public double evaluate()
    {
        checkOverlapandEat();

        double computerScore = 0.0f;
        for (int i = 0; i < this.boardState.getPositionSize((int)Player.PlayerTwo); i++)
        {
            Position position = new Position();
            Position.copyPosition(this.boardState.getPositionAt((int)Player.PlayerTwo, i), position);
            float positionScore = this.computeOffensiveness(position, (int)Player.PlayerOne) + this.computeOpenness(position, (int)Player.PlayerOne) - this.computeDefensiveness(position, (int)Player.PlayerOne);
            computerScore += positionScore;
        }

        double humanScore = 0.0f;
        BoardState temp = new BoardState();
        BoardState.boardCopy(this.boardState, temp);
        List<BoardState> counterBoards = AIAgent.exploreNextMoves(temp, (int)Player.PlayerOne);
        for (int x = 0; x < counterBoards.Count; x++)
        {
            for (int i = 0; i < counterBoards[x].getPositionSize((int)Player.PlayerOne); i++)
            {
                //checkOverlapandEat(counterBoards[x]);
                Position position = new Position();
                Position.copyPosition(this.boardState.getPositionAt((int)Player.PlayerOne, i), position);
                float positionScore = this.computeOpenness(position, (int)Player.PlayerTwo) - this.computeDefensiveness(position, (int)Player.PlayerTwo);
                humanScore += positionScore;
            }
        }

        //if CPU flag is at risk
        BoardState temp1 = new BoardState();
        BoardState.boardCopy(this.boardState, temp1);
        if (this.isFlagAtRisk(temp1, (int)Player.PlayerTwo))
        {
            computerScore = -WIN_LOSS_VALUE;
            Debug.LogError("Computer Flag at Risk");
        }

        //if Human flag is at risk
        BoardState temp2 = new BoardState();
        BoardState.boardCopy(this.boardState, temp2);
        if (this.isFlagAtRisk(temp2, (int)Player.PlayerOne))
        {
            computerScore = WIN_LOSS_VALUE;
            Debug.LogError("Human Flag at Risk");
        }

        BoardState temp3 = new BoardState();
        BoardState.boardCopy(this.boardState, temp3);
        if (this.isFlagDead(temp3, (int)Player.PlayerTwo))
        {
            computerScore = -WIN_LOSS_VALUE * 2;
            Debug.LogWarning("Computer Flag is Dead");
        }

        BoardState temp5 = new BoardState();
        BoardState.boardCopy(this.boardState, temp5);
        if (this.isFlagNearEnd(temp5, (int)Player.PlayerTwo))
        {
            computerScore = WIN_LOSS_VALUE * 2;
            Debug.LogWarning("Computer Flag near the END");
        }

        BoardState temp6 = new BoardState();
        BoardState.boardCopy(this.boardState, temp6);
        if (this.isFlagNearEnd(temp6, (int)Player.PlayerOne))
        {
            computerScore = -WIN_LOSS_VALUE * 2;
            Debug.LogWarning("Human Flag near the END");
        }

        BoardState temp4 = new BoardState();
        BoardState.boardCopy(this.boardState, temp4);
        if (this.isFlagDead(temp4, (int)Player.PlayerOne))
        {
            computerScore = WIN_LOSS_VALUE * 3;
            Debug.LogWarning("Human Flag is Dead");
        }

        return computerScore - (humanScore / counterBoards.Count);
    }

    private bool isFlagNearEnd(BoardState boardState, int player)
    {
        Position flagPos = new Position();
        foreach (var item in player == (int)Player.PlayerTwo ? boardState.getPositionList((int)Player.PlayerTwo) : boardState.getPositionList((int)Player.PlayerOne))
        {
            //Debug.Log(item.PieceID);
            if (item.PieceValue == 1)
            {
                flagPos = Position.copyPosition(item, flagPos);
            }
        }
        if (flagPos.Row == 7 && flagPos.PlayerIndex == 1)
            return true;
        else if (flagPos.Row == 0 && flagPos.PlayerIndex == 0)
            return true;
        else
            return false;
    }

    private bool isFlagDead(BoardState boardState, int player)
    {
        foreach (var item in player == (int)Player.PlayerTwo ? boardState.getPositionList((int)Player.PlayerTwo) : boardState.getPositionList((int)Player.PlayerOne))
        {
            //Debug.Log("ID: " + item.PieceID);
            if (item.PieceID == 0)
            {
                return false;
            }
        }
        return true;
    }

    private bool isFlagAtRisk(BoardState boardState, int player)
    {
        /*
        Debug.Log("ARRAY OF PIECE Positions");
        foreach (var item in boardState.getPositionList(1))
        {
            Debug.Log("PieceValue: " + item.PieceValue + " Position: " + item.Row + ":" + item.Column);
        }
        foreach (var item in boardState.getPositionList(0))
        {
            Debug.Log("PieceValue: " + item.PieceValue + " Position: " + item.Row + ":" + item.Column);
        }
        */
        Position flagPos = new Position();
        foreach (var item in player == (int)Player.PlayerTwo ? boardState.getPositionList((int)Player.PlayerTwo) : boardState.getPositionList((int)Player.PlayerOne))
        {
            if (item.PieceValue == 1)
            {
               //CREATED A COPY OF FLAGPOS, does not update yet
                flagPos = Position.copyPosition(item, flagPos);
                break;
   
            }
        }
        if (flagPos.Row == -1 && flagPos.Column == -1)
        {
            return false;
        }
        //far by 2 square distance
        foreach (var item in player == (int)Player.PlayerTwo ? boardState.getPositionList((int)Player.PlayerOne) : boardState.getPositionList((int)Player.PlayerTwo))
        {
            if (Mathf.Abs(item.Row - flagPos.Row) < 2 && Mathf.Abs(item.Column - flagPos.Column) < 2)
            {
                if (player == (int)Player.PlayerOne)
                {
                    //Debug.Log("Human: " + item.Row + ":" + item.Column);
                    //Debug.Log("CPU: " + flagPos.Row + ":" + flagPos.Column);
                }
                return true;
            }
        }
        return false;
    }

    private float computeOffensiveness(Position computingPos, int opposingPlayer)
    {
        float offensiveness = 0;
        int overTerritory = 0;

        Position positionAtBottom = new Position();
        Position.copyPosition(this.boardState.getPositionAtPlace(computingPos.Row - 1, computingPos.Column), positionAtBottom);
        if (positionAtBottom.PlayerIndex == opposingPlayer && (positionAtBottom.PieceValue < computingPos.PieceValue ||
            (positionAtBottom.PieceValue == 15 && computingPos.PieceValue == 2)) && positionAtBottom != null)
        {
            offensiveness += AIAgent.getInitialHeuristic(positionAtBottom.PieceValue) + 1;
        }

        Position positionAtTop = new Position();
        Position.copyPosition(this.boardState.getPositionAtPlace(computingPos.Row + 1, computingPos.Column), positionAtTop);
        if (positionAtTop.PlayerIndex == opposingPlayer && (positionAtBottom.PieceValue < computingPos.PieceValue ||
            (positionAtBottom.PieceValue == 15 && computingPos.PieceValue == 2)) && positionAtTop != null)
        {
            offensiveness += AIAgent.getInitialHeuristic(positionAtTop.PieceValue) + 1;
        }

        Position positionAtRight = new Position();
        Position.copyPosition(this.boardState.getPositionAtPlace(computingPos.Row, computingPos.Column + 1), positionAtTop);
        if (positionAtRight.PlayerIndex == opposingPlayer && (positionAtBottom.PieceValue < computingPos.PieceValue ||
            (positionAtBottom.PieceValue == 15 && computingPos.PieceValue == 2)) && positionAtRight != null)
        {
            offensiveness += AIAgent.getInitialHeuristic(positionAtRight.PieceValue) + 1;
        }

        Position positionAtLeft = new Position();
        Position.copyPosition(this.boardState.getPositionAtPlace(computingPos.Row, computingPos.Column - 1), positionAtLeft);
        if (positionAtLeft.PlayerIndex == opposingPlayer && (positionAtBottom.PieceValue < computingPos.PieceValue ||
            (positionAtBottom.PieceValue == 15 && computingPos.PieceValue == 2)) && positionAtLeft != null)
        {
            offensiveness += AIAgent.getInitialHeuristic(positionAtLeft.PieceValue) + 1;
        }

        if (computingPos.Row > 3)
        {
            overTerritory++;
        }

        float offenseScore = AIAgent.getInitialHeuristic(computingPos.PieceValue) + (OFFENSE_MULTIPLIER * offensiveness) + 
            (1 * this.boardState.getPositionList((int)Player.PlayerTwo).Count) + (OFFENSE_MULTIPLIER * overTerritory);

        return offenseScore;
    }

    private float computeOpenness(Position computingPos, int opposingPlayer)
    {
        int numAdjacentPieces = 0;

        Position positionAtBottom = this.boardState.getPositionAtPlace(computingPos.Row - 1, computingPos.Column);
        if (positionAtBottom == null || (positionAtBottom != null && positionAtBottom.PlayerIndex == opposingPlayer))
        {
            numAdjacentPieces++;
        }

        Position positionAtTop = this.boardState.getPositionAtPlace(computingPos.Row + 1, computingPos.Column);
        if (positionAtTop == null || (positionAtBottom != null && positionAtBottom.PlayerIndex == opposingPlayer))
        {
            numAdjacentPieces++;
        }

        Position positionAtRight = this.boardState.getPositionAtPlace(computingPos.Row, computingPos.Column + 1);
        if (positionAtRight == null || (positionAtBottom != null && positionAtBottom.PlayerIndex == opposingPlayer))
        {
            numAdjacentPieces++;
        }

        Position positionAtLeft = this.boardState.getPositionAtPlace(computingPos.Row, computingPos.Column - 1);
        if (positionAtLeft == null || (positionAtBottom != null && positionAtBottom.PlayerIndex == opposingPlayer))
        {
            numAdjacentPieces++;
        }

        float opennessScore = AIAgent.getInitialHeuristic(computingPos.PieceValue) + (OPENNESS_VALUE * numAdjacentPieces);

        return opennessScore;
    }

    private float computeDefensiveness(Position computingPos, int opposingPlayer)
    {
        int numAdjacentPieces = 0;

        Position positionAtBottom = this.boardState.getPositionAtPlace(computingPos.Row - 1, computingPos.Column);
        if (positionAtBottom != null && positionAtBottom.PlayerIndex == opposingPlayer)
        {
            numAdjacentPieces++;
        }

        Position positionAtTop = this.boardState.getPositionAtPlace(computingPos.Row + 1, computingPos.Column);
        if (positionAtTop != null && positionAtTop.PlayerIndex == opposingPlayer)
        {
            numAdjacentPieces++;
        }

        Position positionAtRight = this.boardState.getPositionAtPlace(computingPos.Row, computingPos.Column + 1);
        if (positionAtRight != null && positionAtRight.PlayerIndex == opposingPlayer)
        {
            numAdjacentPieces++;
        }

        Position positionAtLeft = this.boardState.getPositionAtPlace(computingPos.Row, computingPos.Column - 1);
        if (positionAtLeft != null && positionAtLeft.PlayerIndex == opposingPlayer)
        {
            numAdjacentPieces++;
        }

        float defensiveScore = AIAgent.getInitialHeuristic(computingPos.PieceValue) - (DEFENSE_DEDUCTION * numAdjacentPieces);

        return defensiveScore;
    }

    private void checkOverlapandEat()
    {
        for (int i = 0; i < this.boardState.getPositionSize((int)Player.PlayerOne); i++)
        {
            for(int j = 0; j < this.boardState.getPositionSize((int)Player.PlayerTwo); j++)
            {
                if (this.boardState.getPositionAt((int)Player.PlayerOne, i).Row == this.boardState.getPositionAt((int)Player.PlayerTwo, j).Row && this.boardState.getPositionAt((int)Player.PlayerOne, i).Column == this.boardState.getPositionAt((int)Player.PlayerTwo, j).Column)
                {
                    //Same Rank
                    if (this.boardState.getPositionAt((int)Player.PlayerOne, i).PieceID == this.boardState.getPositionAt((int)Player.PlayerTwo, j).PieceID)
                    {
                        if (this.boardState.getPositionAt((int)Player.PlayerOne, i).PieceID == 0 && this.boardState.getPositionAt((int)Player.PlayerTwo, j).PieceID == 0)
                        {
                            this.boardState.deletePosition(this.boardState.getPositionAt((int)Player.PlayerOne, i), (int)Player.PlayerOne);
                        }
                        else
                        {
                            this.boardState.deletePosition(this.boardState.getPositionAt((int)Player.PlayerOne, i), (int)Player.PlayerOne);
                            this.boardState.deletePosition(this.boardState.getPositionAt((int)Player.PlayerTwo, j), (int)Player.PlayerTwo);
                        }
                    }
                    else //Ranks are different
                    {
                        //Player is Spy
                        if (this.boardState.getPositionAt((int)Player.PlayerOne, i).PieceID == 2)
                        {
                            //Enemy is not Private
                            if (this.boardState.getPositionAt((int)Player.PlayerTwo, j).PieceID != 1)
                            {
                                if (this.boardState.getPositionAt((int)Player.PlayerOne, i).PieceValue < this.boardState.getPositionAt((int)Player.PlayerTwo, j).PieceValue)
                                {
                                    this.boardState.getPositionAt((int)Player.PlayerOne, i).PieceValue = this.boardState.getPositionAt((int)Player.PlayerTwo, j).PieceValue + 1;
                                }
                                this.boardState.deletePosition(this.boardState.getPositionAt((int)Player.PlayerTwo, j), (int)Player.PlayerTwo);
                            }
                            //Enemy is Private
                            else
                            {
                                this.boardState.deletePosition(this.boardState.getPositionAt((int)Player.PlayerOne, i), (int)Player.PlayerOne);
                            }
                        }
                        //Enemy is Spy
                        else if (this.boardState.getPositionAt((int)Player.PlayerTwo, j).PieceID == 2)
                        {
                            //Player is not Private
                            if (this.boardState.getPositionAt((int)Player.PlayerOne, i).PieceID != 1)
                            {
                                this.boardState.deletePosition(this.boardState.getPositionAt((int)Player.PlayerOne, i), (int)Player.PlayerOne);
                            }
                            //Player is Private
                            else
                            {
                                this.boardState.getPositionAt((int)Player.PlayerOne, i).PieceValue = 2;
                                this.boardState.deletePosition(this.boardState.getPositionAt((int)Player.PlayerTwo, j), (int)Player.PlayerTwo);
                            }
                        }
                        //Player Rank is lower than Enemy Rank
                        else if (this.boardState.getPositionAt((int)Player.PlayerOne, i).PieceID < this.boardState.getPositionAt((int)Player.PlayerTwo, j).PieceID)
                        {
                            this.boardState.deletePosition(this.boardState.getPositionAt((int)Player.PlayerOne, i), (int)Player.PlayerOne);
                        }
                        //Enemy Rank is lower than Player Rank
                        else if (this.boardState.getPositionAt((int)Player.PlayerOne, i).PieceID > this.boardState.getPositionAt((int)Player.PlayerTwo, j).PieceID)
                        {
                            if (this.boardState.getPositionAt((int)Player.PlayerOne, i).PieceValue < this.boardState.getPositionAt((int)Player.PlayerTwo, j).PieceValue)
                            {
                                this.boardState.getPositionAt((int)Player.PlayerOne, i).PieceValue = this.boardState.getPositionAt((int)Player.PlayerTwo, j).PieceValue + 1;
                            }
                            this.boardState.deletePosition(this.boardState.getPositionAt((int)Player.PlayerTwo, j), (int)Player.PlayerTwo);
                        }
                    }
                }
            }
        }
    }
}
