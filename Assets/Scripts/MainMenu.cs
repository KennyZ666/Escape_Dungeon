using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //public void NewStart()
    //{
    //    GameManager.instance.player.Restart();
    //    GameManager.instance.RestartState();
    //    //SceneManager.LoadScene("Main");
    //}

    public void ResumeGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
