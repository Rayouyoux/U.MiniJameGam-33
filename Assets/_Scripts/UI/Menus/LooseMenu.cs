using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LooseMenu : MonoBehaviour
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

    public void OpenMenu()
    {
        AudioManager.Instance.PlaySFX("Loose");
        Time.timeScale = 0f;
        Menu.SetActive(true);
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        Menu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        Menu.SetActive(false);
        SceneManager.LoadScene("Main Menu");
    }
}
