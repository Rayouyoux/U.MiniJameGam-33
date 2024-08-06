using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    public GameObject Menu;
    public Button NextLevelButton;

    private void Start()
    {
        Menu.SetActive(false);

        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (NextLevelButton != null)
        {
            TextMeshProUGUI textComponent = NextLevelButton.GetComponentInChildren<TextMeshProUGUI>();

            if (textComponent != null)
            {
                if (!(nextSceneIndex >= 0 && nextSceneIndex < sceneCount))
                {
                    textComponent.text = "CREDITS";
                    NextLevelButton.onClick.RemoveAllListeners();
                    NextLevelButton.onClick.AddListener(Credits);
                }
            }
            else
            {
                Debug.LogError("TextMeshPro component not found in NextLevelButton's children.");
            }
        }
        else
        {
            Debug.LogError("NextLevelButton is null.");
        }
    }

    public bool IsEnabled()
    {
        return Menu.activeInHierarchy;
    }

    public void OpenMenu()
    {
        AudioManager.Instance.PlaySFX("Win");
        Time.timeScale = 0f;
        Menu.SetActive(true);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        Menu.SetActive(false);

        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex >= 0 && nextSceneIndex < sceneCount)
            SceneManager.LoadScene(nextSceneIndex);
        else
            SceneManager.LoadScene(0);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        Menu.SetActive(false);
        SceneManager.LoadScene("Main Menu");
    }

    public void Credits()
    {
        PlayerPrefs.SetInt("StartFromCredits", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Main Menu");
    }
}
