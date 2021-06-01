using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhitePieces : MonoBehaviour
{

    GameObject[] WhitePiece = new GameObject[21];
    public GameObject parent;
    public GameObject flag, privates, spy, sergeant, seclieu, 
    firstlieu, capt, major, lcol, col, gen1, gen2, gen3, gen4, gen5;
    int pNum = 6, spies = 2;
    int i;
    int currentObject;
    // Start is called before the first frame update
    void Start()
    {
        WhitePiece[0] = Instantiate(flag, transform);
        for (i = 0; i < pNum; i++)
        {
            WhitePiece[1 + i] = Instantiate(privates, transform);
            WhitePiece[1 + i].GetComponent<Piece>().cloneCount = i;
        }
        for (i = 0; i < spies; i++)
        {
            WhitePiece[7 + i] = Instantiate(spy, transform);
            WhitePiece[7 + i].GetComponent<Piece>().cloneCount = i;
        }
        WhitePiece[9] = Instantiate(sergeant, transform);
        WhitePiece[10] = Instantiate(seclieu, transform);
        WhitePiece[11] = Instantiate(firstlieu, transform);
        WhitePiece[12] = Instantiate(capt, transform);
        WhitePiece[13] = Instantiate(major, transform);
        WhitePiece[14] = Instantiate(lcol, transform);
        WhitePiece[15] = Instantiate(col, transform);
        WhitePiece[16] = Instantiate(gen1, transform);
        WhitePiece[17] = Instantiate(gen2, transform);
        WhitePiece[18] = Instantiate(gen3, transform);
        WhitePiece[19] = Instantiate(gen4, transform);
        WhitePiece[20] = Instantiate(gen5, transform);


        for (i = 0; i < WhitePiece.Length; i++)
        {
            WhitePiece[i].GetComponent<Piece>().color = false;
        }

        transform.SetAsLastSibling();
        //transform.SetSiblingIndex(4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
