using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void MainMenu()
    {
        //Debug.Log("quit");
        //UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
        GameManager.instance.SaveState();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
