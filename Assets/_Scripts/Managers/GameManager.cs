using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera MainCamera;

    public PlayerHealth Player;

    public PauseMenu PausedMenu;

    public LooseMenu LostMenu;

    public static bool HasStarted;

    public static bool IsPaused;

    public static bool LockedControls;

    void Start()
    {
        HasStarted = false;
        LockedControls = false;
    }

    void Update()
    {
        if (PausedMenu.IsEnabled() || LostMenu.IsEnabled())
        {
            Time.timeScale = 0f;
            IsPaused = true;
            if (AudioManager.Instance.MusicSource.isPlaying)
                AudioManager.Instance.MusicSource.Pause();
        }
        else
        {
            Time.timeScale = 1f;
            IsPaused = false;
            if (!AudioManager.Instance.MusicSource.isPlaying)
                AudioManager.Instance.MusicSource.Play();
        }

        if (!HasStarted)
        {
            if (Input.anyKey)
            {
                HasStarted = true;
                StartLevel();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
                PausedMenu.ResumeGame();
            else
                PausedMenu.PauseGame();
        }

        if (!Player.IsAlive)
        {
            if (LostMenu.IsEnabled()) return;

            LostMenu.OpenMenu();
        }

    }

    public void StartLevel()
    {
        CameraMovement cameraMovement = MainCamera.GetComponent<CameraMovement>();

        cameraMovement.MoveCamera = true;

        //AudioManager.Instance.PlayMusic("Main");
    }
}
