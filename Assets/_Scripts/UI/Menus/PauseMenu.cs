using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject Menu;

    private void Start()
    {
        Menu.SetActive(false);
    }
    public bool IsEnabled()
    {
        return Menu.activeInHierarchy;
    }

    public void PauseGame()
    {
        Menu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Menu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
