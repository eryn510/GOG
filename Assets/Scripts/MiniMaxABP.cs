using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaxABP : MonoBehaviour
{
    List<Tile> occupiedTiles = new List<Tile>();
    List<Piece> blackPieces = new List<Piece>();
    List<Piece> whitePieces = new List<Piece>();
    List<MoveInfo> moves = new List<MoveInfo>();
    Stack<MoveInfo> moveStack = new Stack<MoveInfo>();

    MoveInfo bestMove;

    //MoveScore score = new MoveScore();



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
