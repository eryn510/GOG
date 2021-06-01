using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackPieces : MonoBehaviour
{

    public GameObject[] BlackPiece = new GameObject[21];
    public GameObject parent;
    public GameObject flag, privates, spy, sergeant, seclieu,
        firstlieu, capt, major, lcol, col, gen1, gen2, gen3, gen4, gen5;
    int pNum = 6, spies = 2;
    int i;
    int currentObject;
    //public string currentPlayer = FindObjectOfType<GridTiles>().currentPlayer;


    // Start is called before the first frame update
    void Start()
    {
        BlackPiece[0] = Instantiate(flag, transform);
        for (i = 0; i < pNum; i++)
        {
            BlackPiece[1 + i] = Instantiate(privates, transform);
            BlackPiece[1 + i].GetComponent<Piece>().cloneCount = i;
        }
        for (i = 0; i < spies; i++)
        {
            BlackPiece[7 + i] = Instantiate(spy, transform);
            BlackPiece[7 + i].GetComponent<Piece>().cloneCount = i;
        }
        BlackPiece[9] = Instantiate(sergeant, transform);
        BlackPiece[10] = Instantiate(seclieu, transform);
        BlackPiece[11] = Instantiate(firstlieu, transform);
        BlackPiece[12] = Instantiate(capt, transform);
        BlackPiece[13] = Instantiate(major, transform);
        BlackPiece[14] = Instantiate(lcol, transform);
        BlackPiece[15] = Instantiate(col, transform);
        BlackPiece[16] = Instantiate(gen1, transform);
        BlackPiece[17] = Instantiate(gen2, transform);
        BlackPiece[18] = Instantiate(gen3, transform);
        BlackPiece[19] = Instantiate(gen4, transform);
        BlackPiece[20] = Instantiate(gen5, transform);


        for (i = 0; i < BlackPiece.Length; i++)
        {
            BlackPiece[i].GetComponent<Piece>().color = true;
        }

        transform.SetAsLastSibling();
        //transform.SetSiblingIndex(3);

    }

    // Update is called once per frame
    void Update()
    {
        
        /*
        if(currentPlayer == "Black")
        {
            BlackPiece[20].GetComponent<Piece>().y -= 100; 
        }
        */
    }
}