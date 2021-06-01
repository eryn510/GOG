using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject tiles;
    public Transform parent;


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(tiles, transform.parent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
