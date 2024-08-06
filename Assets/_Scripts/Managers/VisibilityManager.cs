using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerVision;

public class VisibilityManager : MonoBehaviour
{
    public PlayerVision PlayerVisionHandler;
    public float transitionDuration = 1.0f;

    private List<GameObject> _redObjects;
    private List<GameObject> _blueObjects;
    private Glasses previousGlassesState;
    private Dictionary<GameObject, Coroutine> activeCoroutines;

    void Start()
    {
        _redObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Red"));
        _blueObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Blue"));
        previousGlassesState = PlayerVisionHandler.CurrentGlasses;
        activeCoroutines = new Dictionary<GameObject, Coroutine>();
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
        CleanUpList(_redObjects);
        CleanUpList(_blueObjects);

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

    void ToggleVisibility(List<GameObject> objects, bool isActive)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                // Stop any existing coroutine for this object
                if (activeCoroutines.ContainsKey(obj))
                {
                    StopCoroutine(activeCoroutines[obj]);
                    activeCoroutines.Remove(obj);
                }
                // Start a new coroutine
                Coroutine coroutine = StartCoroutine(LerpOpacity(obj, isActive));
                activeCoroutines[obj] = coroutine;
            }
        }
    }

    IEnumerator LerpOpacity(GameObject obj, bool isActive)
    {
        if (obj == null || !obj.activeInHierarchy)
        {
            yield break;
        }

        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            yield break;
        }

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

            // Check if the object has been destroyed or deactivated during the coroutine
            if (obj == null || !obj.activeInHierarchy)
            {
                yield break;
            }
        }

        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, endOpacity);
    }

    void CleanUpList(List<GameObject> objects)
    {
        objects.RemoveAll(obj => obj == null);
    }
}
