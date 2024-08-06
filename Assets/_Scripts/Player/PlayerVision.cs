using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVision : MonoBehaviour
{
    public GameObject RedGlasses;
    public GameObject BlueGlasses;
    public float WearingTime;
    public float SwitchingTime;

    public enum Glasses
    {
        None,
        Red,
        Blue
    }
    private Glasses _currentGlasses;
    private Glasses _previousGlasses;

    private float _wearingTimer;
    private float _switchingTimer;
    private bool _stopGlasses;

    public Glasses CurrentGlasses { get => _currentGlasses; set => _currentGlasses = value; }
    public bool StopGlasses { get => _stopGlasses; set => _stopGlasses = value; }

    void Start()
    {
        _currentGlasses = Glasses.Blue;
        _wearingTimer = WearingTime;
        StopGlasses = false;
    }

    void Update()
    {
        ApplyCurrentGlasses();

        if (StopGlasses)
        {
            if (_currentGlasses != Glasses.None)
                _currentGlasses = Glasses.None;
            return;
        }

        SetCurrentGlasses();
    }

    private void ApplyCurrentGlasses()
    {
        if (_currentGlasses == Glasses.Red)
        {
            if (!RedGlasses.activeInHierarchy)
                RedGlasses.SetActive(true);
            if (BlueGlasses.activeInHierarchy)
                BlueGlasses.SetActive(false);
        }
        else if (_currentGlasses == Glasses.Blue)
        {
            if (RedGlasses.activeInHierarchy)
                RedGlasses.SetActive(false);
            if (!BlueGlasses.activeInHierarchy)
                BlueGlasses.SetActive(true);
        }
        else
        {
            if (RedGlasses.activeInHierarchy)
                RedGlasses.SetActive(false);
            if (BlueGlasses.activeInHierarchy)
                BlueGlasses.SetActive(false);

        }
    }

    private void SetCurrentGlasses()
    {
        if (_currentGlasses == Glasses.Blue || _currentGlasses == Glasses.Red)
        {
            _wearingTimer -= Time.deltaTime;
            if (_wearingTimer <= 0)
            {
                _wearingTimer = 0;
                _switchingTimer = SwitchingTime;

                _previousGlasses = _currentGlasses;
                _currentGlasses = Glasses.None;
            }
        }
        else
        {
            _switchingTimer -= Time.deltaTime;
            if (_switchingTimer <= 0)
            {
                _switchingTimer = 0;
                _wearingTimer = WearingTime;

                if (_previousGlasses == Glasses.Blue)
                    _currentGlasses = Glasses.Red;
                else
                    _currentGlasses = Glasses.Blue;
                _previousGlasses = Glasses.None;
            }
        }
    }
}
