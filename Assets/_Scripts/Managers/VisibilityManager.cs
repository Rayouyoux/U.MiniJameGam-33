using System.Collections;
using UnityEngine;
using static PlayerVision;

public class VisibilityManager : MonoBehaviour
{
    public PlayerVision PlayerVisionHandler;
    public float transitionDuration = 1.0f;

    private GameObject[] _redObjects;
    private GameObject[] _blueObjects;
    private Glasses previousGlassesState;

    void Start()
    {
        _redObjects = GameObject.FindGameObjectsWithTag("Red");
        _blueObjects = GameObject.FindGameObjectsWithTag("Blue");
        previousGlassesState = PlayerVisionHandler.CurrentGlasses;
        UpdateVisibility();
    }

    void Update()
    {
        if (PlayerVisionHandler.CurrentGlasses != previousGlassesState)
        {
            UpdateVisibility();
            previousGlassesState = PlayerVisionHandler.CurrentGlasses;
        }
    }

    void UpdateVisibility()
    {
        if (PlayerVisionHandler.CurrentGlasses == Glasses.None)
        {
            ToggleVisibility(_redObjects, false);
            ToggleVisibility(_blueObjects, false);
        }
        else if (PlayerVisionHandler.CurrentGlasses == Glasses.Red)
        {
            ToggleVisibility(_redObjects, true);
            ToggleVisibility(_blueObjects, false);
        }
        else if (PlayerVisionHandler.CurrentGlasses == Glasses.Blue)
        {
            ToggleVisibility(_redObjects, false);
            ToggleVisibility(_blueObjects, true);
        }
    }

    void ToggleVisibility(GameObject[] objects, bool isActive)
    {
        foreach (GameObject obj in objects)
        {
            StartCoroutine(LerpOpacity(obj, isActive));
        }
    }

    IEnumerator LerpOpacity(GameObject obj, bool isActive)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        Color startColor = spriteRenderer.color;
        float startOpacity = startColor.a;
        float endOpacity = isActive ? 1f : 0f;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float newOpacity = Mathf.Lerp(startOpacity, endOpacity, elapsedTime / transitionDuration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newOpacity);
            yield return null;
        }

        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, endOpacity);
    }
}
