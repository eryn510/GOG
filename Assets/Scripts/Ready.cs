using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ready : MonoBehaviour
{
    GameObject ready;
    GameObject[] text = new GameObject[2];
    public GameObject Exit;
    public GameObject TurnIndicator;
    public GameObject TurnIndicatorBlack;

    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
        SFXManager.SFXInstance.playSFX(SFXManager.SFXInstance.ButtonClick);
    }

    public void ReadyGame()
    {
        SFXManager.SFXInstance.playSFX(SFXManager.SFXInstance.ButtonClick);
        Debug.Log("ready is set");
        FindObjectOfType<GridTiles>().isReady = true;
        FindObjectOfType<GridTiles>().Black.SetActive(true);
        FindObjectOfType<GridTiles>().currentPlayer = "White";

        for (int i = 0; i < FindObjectOfType<GridTiles>().tiles.Length; i++)
        {
            FindObjectOfType<GridTiles>().tiles[i].SetActive(true);
        }

        text = GameObject.FindGameObjectsWithTag("Text");
        text[0].gameObject.SetActive(false);
        text[1].gameObject.SetActive(false);
        Exit.SetActive(true);
        TurnIndicator.SetActive(true);
        TurnIndicatorBlack.SetActive(false);
    }
}