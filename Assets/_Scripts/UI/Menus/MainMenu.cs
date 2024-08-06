using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OpenLevelOne()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void OpenLevelTwo()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void OpenLevelThree()
    {
        SceneManager.LoadScene("Level 3");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
