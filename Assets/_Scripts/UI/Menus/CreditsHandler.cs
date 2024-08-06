using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsHandler : MonoBehaviour
{
    public GameObject MainPage;
    public GameObject CreditsPage;

    private void Start()
    {
        bool openCredits = PlayerPrefs.GetInt("StartFromCredits") == 1;
        if (openCredits)
        {
            PlayerPrefs.SetInt("StartFromCredits", 0);
            PlayerPrefs.Save();
            MainPage.SetActive(false);
            CreditsPage.SetActive(true);
        }
    }
}
