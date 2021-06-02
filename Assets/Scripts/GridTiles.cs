using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GridTiles : MonoBehaviour
{
    public GameObject[,] grid = new GameObject[8, 9];
    public GameObject[] tiles = new GameObject[72];
    public GameObject[] player = new GameObject[22];
    public GameObject[] enemy = new GameObject[22];
    public GameObject[] dead = new GameObject[22];
    public GameObject[] deadBlack = new GameObject[22];
    public bool isReady = false;
    public GameObject Black, White;
    GameObject BlackWin, WhiteWin;
    GameObject[] textObjects;
    GameObject whiteIndicator;
    GameObject blackIndicator;
    AILogic AI;
    
    List<Position> whitePieces = new List<Position> { };
    List<Position> blackPieces = new List<Position> { };

    //ENEMY SETUP POSITIONS ARRAYS
    int[,] setupPositions1, setupPositions2, setupPositions3, setupPositions4, setupPositions5, setupPositions6;

    List<int[,]> setupBank = new List<int[,]> { };
    Position holder;

    BoardState boardState = new BoardState();

    int i, j, k;
    int x, y;
    int tileset = 0;
    int players = 0;
    string tileTag = "Tile";
    string playerTag = "Player";
    string enemyTag = "Enemy";
    public string currentPlayer;
    bool WhiteEmpty = false, BlackEmpty = false;

    public int[,] boardCopy = new int[8,9];


    private bool isCoroutineExecuting = false;

    void loadWhiteWin()
    {
        SFXManager.SFXInstance.playSFX(SFXManager.SFXInstance.Win);
        SFXManager.SFXInstance.Audio.volume = 0.05f;
        SceneManager.LoadScene("WhiteWin");
    }
    void loadBlackWin()
    {
        SFXManager.SFXInstance.playSFX(SFXManager.SFXInstance.Lose);
        SFXManager.SFXInstance.Audio.volume = 0.05f;
        SceneManager.LoadScene("BlackWin");
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        if (isCoroutineExecuting)
            yield break;

        isCoroutineExecuting = true;

        yield return new WaitForSeconds(time);

        // Code to execute after the delay

        isCoroutineExecuting = false;
    }

    bool checkWhite()
    {
        for (i = 0; i < player.Length; i++)
        {
            if (player[i].activeSelf)
                return false;
        }

        return true;
    }
    bool checkBlack()
    {
        for (i = 0; i < enemy.Length; i++)
        {
            if (enemy[i].activeSelf)
                return false;
        }

        return true;
    }

    void checkWin()
    {
        WhiteEmpty = checkWhite();
        BlackEmpty = checkBlack();

        if (player[0].activeSelf == false || WhiteEmpty || enemy[0].GetComponent<Piece>().x == 7)
        {
            for (i = 0; i < enemy.Length; i++)
            {
                currentPlayer = "None";
                player[i].GetComponent<Piece>().move = false;
                enemy[i].GetComponent<Piece>().move = false;
                //BlackWin.SetActive(true);
                //BlackWin.transform.SetAsLastSibling();
                loadBlackWin();
                //SceneManager.LoadScene("Results");
            }
        }
        else if (enemy[0].activeSelf == false || BlackEmpty || player[0].GetComponent<Piece>().x == 0)
        {
            for (i = 0; i < enemy.Length; i++)
            {
                currentPlayer = "None";
                player[i].GetComponent<Piece>().move = false;
                enemy[i].GetComponent<Piece>().move = false;
                Debug.Log("White Wins");
                //WhiteWin.SetActive(true);
                //WhiteWin.transform.SetAsLastSibling();
                loadWhiteWin();
                //SceneManager.LoadScene("Results");
            }
        }
    }

    void turn()
    {
        if (currentPlayer == "White")
        {
            boardState.WhoseTurnToMove = 0;
            whiteIndicator.SetActive(true);
            blackIndicator.SetActive(false);
            for (i = 0; i < enemy.Length; i++)
            {
                player[i].GetComponent<Piece>().move = true;
                enemy[i].GetComponent<Piece>().move = false;
            }
        }

        else if (currentPlayer == "Black")
        {
            boardState.WhoseTurnToMove = 1;
            whiteIndicator.SetActive(false);
            blackIndicator.SetActive(true);
            AI.execute();
            FindObjectOfType<GridTiles>().currentPlayer = "White";
            for (i = 0; i < player.Length; i++)
            {
                player[i].GetComponent<Piece>().move = false;
                enemy[i].GetComponent<Piece>().move = true;
            }
        }

        //transform.SetSiblingIndex(13);
    }

    void setInitialPosition()
    {
        for (i = 0; i < player.Length; i++)
        {
            holder = new Position();

            holder.PieceID = player[i].GetComponent<Piece>().rank;
            holder.PieceValue = player[i].GetComponent<Piece>().suspectedValue;
            holder.Row = player[i].GetComponent<Piece>().x;
            holder.Column = player[i].GetComponent<Piece>().y;
            holder.PlayerIndex = 0;
            holder.CloneIndex = player[i].GetComponent<Piece>().cloneCount;
            whitePieces.Add(holder);
        }
        for (i = 0; i < enemy.Length; i++)
        {
            holder = new Position();

            holder.PieceID = enemy[i].GetComponent<Piece>().rank;
            holder.PieceValue = enemy[i].GetComponent<Piece>().trueRank;
            holder.Row = enemy[i].GetComponent<Piece>().x;
            holder.Column = enemy[i].GetComponent<Piece>().y;
            holder.PlayerIndex = 1;
            holder.CloneIndex = enemy[i].GetComponent<Piece>().cloneCount;
            blackPieces.Add(holder);
        }
    }

    void updatePosition()
    {
        boardState = new BoardState();
        whitePieces.Clear();
        blackPieces.Clear();
        for (i = 0; i < player.Length; i++)
        {
            if (player[i].activeSelf != false)
            {
                holder = new Position();

                holder.PieceID = player[i].GetComponent<Piece>().rank;
                if(player[i].GetComponent<Piece>().rank == 0)
                {
                    for (j = 1; j < player.Length; j++)
                    {
                        player[i].GetComponent<Piece>().suspectedValue = 1;
                        if (player[j].activeSelf != false)
                        {
                            if(player[j].GetComponent<Piece>().suspectedValue == 0)
                            {
                                player[i].GetComponent<Piece>().suspectedValue = 0;
                                break;
                            }
                            else
                                player[i].GetComponent<Piece>().suspectedValue = 1;
                        }
                    }
                }
                holder.PieceValue = player[i].GetComponent<Piece>().suspectedValue;
                holder.Row = player[i].GetComponent<Piece>().x;
                holder.Column = player[i].GetComponent<Piece>().y;
                holder.PlayerIndex = 0;
                holder.CloneIndex = player[i].GetComponent<Piece>().cloneCount;
                player[i].GetComponent<Piece>().isAttack = false;
                //Debug.Log("Clone Index: " + holder.CloneIndex);
                whitePieces.Add(holder);
            }
        }
        for (i = 0; i < enemy.Length; i++)
        {
            if (enemy[i].activeSelf != false)
            {
                holder = new Position();
                holder.PieceID = enemy[i].GetComponent<Piece>().rank;
                holder.PieceValue = enemy[i].GetComponent<Piece>().trueRank;
                holder.Row = enemy[i].GetComponent<Piece>().x;
                holder.Column = enemy[i].GetComponent<Piece>().y;
                holder.PlayerIndex = 1;
                holder.CloneIndex = enemy[i].GetComponent<Piece>().cloneCount;
                enemy[i].GetComponent<Piece>().isAttack = false;
                blackPieces.Add(holder);
            }
        }
        boardState.addFromPosList(0, whitePieces);
        boardState.addFromPosList(1, blackPieces);

        AI.addToList(enemy);
        AI.addTilesList(tiles);
        AI.setCurrent(boardState);
    }

    void checkEat()
    {
        for (i = 0; i < player.Length; i++)
        {
            for (j = 0; j < enemy.Length; j++)
            {
                if (player[i].GetComponent<Piece>().x == enemy[j].GetComponent<Piece>().x && player[i].GetComponent<Piece>().y == enemy[j].GetComponent<Piece>().y && player[i].activeSelf == true && enemy[j].activeSelf)
                {
                    SFXManager.SFXInstance.playSFX(SFXManager.SFXInstance.Kill);

                    //Same Rank
                    if (player[i].GetComponent<Piece>().rank == enemy[j].GetComponent<Piece>().rank)
                    {
                        if(player[i].GetComponent<Piece>().rank == 0 && enemy[j].GetComponent<Piece>().rank == 0)
                        {
                            if (player[0].GetComponent<Piece>().isAttack == true && enemy[0].GetComponent<Piece>().isAttack == false)
                            {
                                deadBlack[0].gameObject.SetActive(true);
                                enemy[0].gameObject.SetActive(false);
                            }
                            else if (player[0].GetComponent<Piece>().isAttack == false && enemy[0].GetComponent<Piece>().isAttack == true)
                            {
                                dead[0].gameObject.SetActive(true);
                                player[0].gameObject.SetActive(false);
                            }
                        }
                        
                        else
                        {
                            for (k = 0; k < dead.Length; k++)
                            {
                                if (dead[k].GetComponent<Piece>().rank == player[i].GetComponent<Piece>().rank)
                                    dead[k].gameObject.SetActive(true);
                                if (deadBlack[k].GetComponent<Piece>().rank == enemy[j].GetComponent<Piece>().rank)
                                    deadBlack[k].gameObject.SetActive(true);
                            }
                            player[i].gameObject.SetActive(false);
                            enemy[j].gameObject.SetActive(false);
                        }
                    }
                    else //Ranks are different
                    {
                        //Player is Spy
                        if (player[i].GetComponent<Piece>().rank == 2)
                        {
                            //Enemy is not Private
                            if (enemy[j].GetComponent<Piece>().rank != 1)
                            {
                                for (k = 0; k < deadBlack.Length; k++)
                                {
                                    if (deadBlack[k].GetComponent<Piece>().rank == enemy[j].GetComponent<Piece>().rank)
                                        deadBlack[k].gameObject.SetActive(true);
                                }
                                if (player[i].GetComponent<Piece>().suspectedValue < enemy[j].GetComponent<Piece>().trueRank)
                                {
                                    player[i].GetComponent<Piece>().suspectedValue = enemy[j].GetComponent<Piece>().trueRank + 1;
                                }
                                enemy[j].gameObject.SetActive(false);
                            }
                            //Enemy is Private
                            else
                            {
                                for (k = 0; k < dead.Length; k++)
                                {
                                    if (dead[k].GetComponent<Piece>().rank == player[i].GetComponent<Piece>().rank)
                                        dead[k].gameObject.SetActive(true);
                                }
                                player[i].gameObject.SetActive(false);
                            }
                        }
                        //Enemy is Spy
                        else if (enemy[j].GetComponent<Piece>().rank == 2)
                        {
                            //Player is not Private
                            if (player[i].GetComponent<Piece>().rank != 1)
                            {
                                for (k = 0; k < dead.Length; k++)
                                {
                                    if (dead[k].GetComponent<Piece>().rank == player[i].GetComponent<Piece>().rank)
                                        dead[k].gameObject.SetActive(true);
                                }
                                player[i].gameObject.SetActive(false);
                            }
                            //Player is Private
                            else
                            {
                                player[i].GetComponent<Piece>().suspectedValue = 2;
                                for (k = 0; k < deadBlack.Length; k++)
                                {
                                    if (deadBlack[k].GetComponent<Piece>().rank == enemy[j].GetComponent<Piece>().rank)
                                        deadBlack[k].gameObject.SetActive(true);
                                }
                                enemy[j].gameObject.SetActive(false);
                            }
                        }
                        //Player Rank is lower than Enemy Rank
                        else if (player[i].GetComponent<Piece>().rank < enemy[j].GetComponent<Piece>().rank)
                        {
                            for (k = 0; k < dead.Length; k++)
                            {
                                if (dead[k].GetComponent<Piece>().rank == player[i].GetComponent<Piece>().rank)
                                    dead[k].gameObject.SetActive(true);
                            }
                            player[i].gameObject.SetActive(false);
                        }
                        //Enemy Rank is lower than Player Rank
                        else if (player[i].gameObject.GetComponent<Piece>().rank > enemy[j].GetComponent<Piece>().rank)
                        {
                            if (player[i].GetComponent<Piece>().suspectedValue < enemy[j].GetComponent<Piece>().trueRank)
                            {
                                player[i].GetComponent<Piece>().suspectedValue = enemy[j].GetComponent<Piece>().trueRank + 1;
                            }
                            for (k = 0; k < deadBlack.Length; k++)
                            {
                                if (deadBlack[k].GetComponent<Piece>().rank == enemy[j].GetComponent<Piece>().rank)
                                    deadBlack[k].gameObject.SetActive(true);
                            }
                            enemy[j].gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    void initializeBoard()
    {
        //INITIALIZE BOARD COPY
        for (i = 0; i < 8; i++)
        {
            for (j = 0; j < 9; j++)
            {
                boardCopy[i, j] = 0;
            }
        }

        whiteIndicator.SetActive(false);
        blackIndicator.SetActive(false);

        tiles = tiles.OrderBy(go => go.name).ToArray();

        while (tileset != tiles.Length)
        {
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 9; j++)
                {
                    grid[i, j] = tiles[tileset];
                    grid[i, j].gameObject.GetComponent<Tile>().x = i;
                    grid[i, j].gameObject.GetComponent<Tile>().y = j;
                    tileset++;
                }
            }
        }
    }

    void bankSetup()
    {
        setupPositions1 = new int[3, 9] {
            {0,0,2,2,5,2,0,0,0},
            {3,4,6,14,1,15,9,10,11},
            {2,7,13,2,15,12,2,8,0}
        };

        setupPositions2 = new int[3, 9] {
            {4,0,2,2,0,2,0,10,0},
            {3,0,6,14,0,13,9,12,11},
            {2,7,5,15,1,15,2,8,2}
        };

        setupPositions3 = new int[3, 9] {
            {4,0,3,0,1,0,2,10,0},
            {5,0,12,2,15,2,13,7,11},
            {2,15,2,9,14,8,6,0,2}
        };

        setupPositions4 = new int[3, 9] {
            {0,0,3,13,1,2,0,10,0},
            {4,5,2,15,14,15,2,7,0},
            {0,6,9,2,2,8,2,12,11}
        };

        setupPositions5 = new int[3, 9] {
            {1,2,3,4,0,0,0,2,15},
            {15,14,2,6,5,0,0,0,2},
            {7,2,9,10,11,8,2,13,12}
        };

        setupPositions6 = new int[3, 9] {
            {1,2,0,3,4,11,0,2,15},
            {15,14,2,6,5,12,2,13,2},
            {0,0,9,10,7,8,2,0,0}
        };

        setupBank.Add(setupPositions1);
        setupBank.Add(setupPositions2);
        setupBank.Add(setupPositions3);
        setupBank.Add(setupPositions4);
        setupBank.Add(setupPositions5);
        setupBank.Add(setupPositions6);
    }

    void deactivateConstruction()
    {
        Black.SetActive(false);
        for (i = 0; i < 36; i++)
        {
            tiles[i].gameObject.SetActive(false);
        }
        for (i = 0; i < dead.Length; i++)
        {
            dead[i].GetComponent<Piece>().move = false;
            dead[i].gameObject.SetActive(false);
        }

        for (i = 0; i < deadBlack.Length; i++)
        {
            deadBlack[i].GetComponent<Piece>().move = false;
            deadBlack[i].gameObject.SetActive(false);
        }
    }

    void pieceSetup()
    {
        //RANDOM PICKER OF ENEMY SETUP
        int setupIndexPicked = Random.Range(0, setupBank.Count);

        //ENEMY PIECE PLACEMENTS
        //KNOWN BUGS = privates and spies are not spawning properly
        for (i = 0; i < 3; i++)
        {
            for (j = 0; j < 9; j++)
            {
                foreach (var indivEnemy in enemy)
                {
                    if (indivEnemy.GetComponent<Piece>().trueRank == setupBank[setupIndexPicked][i, j] &&
                        !indivEnemy.GetComponent<Piece>().isPlaced)//setupPositions1[i, j])
                    {
                        indivEnemy.GetComponent<RectTransform>().localPosition = grid[i, j].GetComponent<RectTransform>().localPosition;
                        indivEnemy.GetComponent<Piece>().x = i;
                        indivEnemy.GetComponent<Piece>().y = j;
                        indivEnemy.GetComponent<Piece>().isPlaced = true;
                        break;
                    }
                }
            }
        }


        //PIECE PLACEMENTS
        for (i = 0; i < 3; i++)
        {
            if (i != 2)
            {
                for (j = 0; j < 9; j++)
                {
                    //PLAYER SETUP
                    player[players].GetComponent<RectTransform>().localPosition = grid[i + 5, j].GetComponent<RectTransform>().localPosition;
                    player[players].GetComponent<Piece>().x = i + 5;
                    player[players].GetComponent<Piece>().y = j;

                    boardCopy[i + 5, j] = player[players].GetComponent<Piece>().rank;

                    players++;
                }
            }
            else
            {
                for (j = 0; j < 3; j++)
                {
                    //PLAYER SETUP LAST ROW
                    player[players].GetComponent<RectTransform>().localPosition = grid[i + 5, j].GetComponent<RectTransform>().localPosition;
                    player[players].GetComponent<Piece>().x = i + 5;
                    player[players].GetComponent<Piece>().y = j;
                    boardCopy[i + 5, j] = player[players].GetComponent<Piece>().rank;
                    players++;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        tiles = GameObject.FindGameObjectsWithTag(tileTag);
        player = GameObject.FindGameObjectsWithTag(playerTag);
        enemy = GameObject.FindGameObjectsWithTag(enemyTag);
        dead = GameObject.FindGameObjectsWithTag("Dead");
        Black = GameObject.FindGameObjectWithTag("Black");
        White = GameObject.FindGameObjectWithTag("White");
        textObjects = GameObject.FindGameObjectsWithTag("Text");
        whiteIndicator = GameObject.FindGameObjectWithTag("Indicator");
        blackIndicator = GameObject.FindGameObjectWithTag("IndicatorBlack");
        deadBlack = GameObject.FindGameObjectsWithTag("DeadBlack");
        AI = gameObject.AddComponent<AILogic>();
        boardState = new BoardState();

        bankSetup();

        initializeBoard();

        pieceSetup();        

        setInitialPosition();

        boardState.addFromPosList(0, whitePieces);
        boardState.addFromPosList(1, blackPieces);

        AI.addToList(enemy);
        AI.addTilesList(tiles);
        AI.setCurrent(boardState);

        deactivateConstruction();

    }

    // Update is called once per frame
    void Update()
    {
        //updatePosition();


        if (isReady)
        {
            //Debug.Log("VALUE: " + whitePieces[0].PieceValue);

            checkEat();

            updatePosition();

            checkWin();

            turn();

        }
        
    }
}

