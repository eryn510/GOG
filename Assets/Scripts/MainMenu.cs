using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
        SFXManager.SFXInstance.playSFX(SFXManager.SFXInstance.ButtonClick);
    }
    public void ExitApplication()
    {
        Application.Quit();
        SFXManager.SFXInstance.playSFX(SFXManager.SFXInstance.ButtonClick);
    }
}
