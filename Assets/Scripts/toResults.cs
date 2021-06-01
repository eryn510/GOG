using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class toResults : MonoBehaviour
{

    void loadResults()
    {
        SceneManager.LoadScene("Results");
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("loadResults", 4);
    }

    
}
