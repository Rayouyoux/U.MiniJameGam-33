using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float _initialScrollSpeed;
    public static float ScrollSpeed;
    [NonSerialized] public static bool IsScrolling;
    public List<Scroll> ScrollObjects;

    public PlayerHealth Player;

    [NonSerialized] public PauseMenu PausedMenu;
    [NonSerialized] public LooseMenu LostMenu;
    [NonSerialized] public WinMenu WonMenu;
    public static bool PlayerWin;

    public static bool HasStarted;
    public static bool IsPaused;
    public static bool LockedControls;

    public Camera MainCamera;
    public GameObject BackGround1;
    public GameObject BackGround2;
    private float _backgroundSize;

    void Start()
    {
        PausedMenu = GetComponent<PauseMenu>();
        LostMenu = GetComponent<LooseMenu>();
        WonMenu = GetComponent<WinMenu>();

        PlayerWin = false;
        IsScrolling = false;
        ScrollSpeed = _initialScrollSpeed;
        _backgroundSize = BackGround1.GetComponent<SpriteRenderer>().bounds.size.x;
        HasStarted = false;
        LockedControls = false;
    }

    void Update()
    {

        if (PausedMenu.IsEnabled() || LostMenu.IsEnabled() || WonMenu.IsEnabled())
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

        if (PlayerWin)
        {
            if (WonMenu.IsEnabled()) return;

            WonMenu.OpenMenu();
        }

        HandleBackgroundPosition();

        if (IsScrolling)
        {
            foreach (Scroll obj in ScrollObjects)
            {
                obj.Move();
            }
        }

    }

    private void HandleBackgroundPosition()
    {
        if (MainCamera.transform.position.x >= BackGround1.transform.position.x && BackGround1.transform.position.x > BackGround2.transform.position.x)
        {
            BackGround2.transform.position += new Vector3(_backgroundSize * 2, 0, 0);
        }
        else if (MainCamera.transform.position.x >= BackGround2.transform.position.x && BackGround2.transform.position.x > BackGround1.transform.position.x)
        {
            BackGround1.transform.position += new Vector3(_backgroundSize * 2, 0, 0);
        }
    }

    public void StartLevel()
    {
        IsScrolling = true;

        AudioManager.Instance.PlayMusic("Main");
    }

    public void ToggleScroll()
    {
        IsScrolling = !IsScrolling;
    }
}
