using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class PauseControl : MonoBehaviour
{
    public static bool gameIsPaused;
    public GameObject volumeObj;
    public float step;

    public float onPausedFocusDistance;

    public GameObject pauseMenu;

    DepthOfField depthOfField;

    float defaultFocusDistance;

    IEnumerator preEnumerator;

    void Start()
    {
        if (volumeObj != null)
        {
            var volume = volumeObj.GetComponent<Volume>();

            volume.profile.TryGet<DepthOfField>(out depthOfField);

            if (depthOfField != null)
            {
                defaultFocusDistance = (float)depthOfField.focusDistance;
            }
        }

        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

    void PauseGame()
    {
        Blur(gameIsPaused);
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    void Blur(bool on)
    {
        if (depthOfField == null) return;
        if (preEnumerator != null)
        {
            StopCoroutine(preEnumerator);
        }
        IEnumerator enumerator;
        if (on)
        {
            enumerator = AnimateTo(onPausedFocusDistance);
        }
        else
        {
            enumerator = AnimateTo(defaultFocusDistance);
        }
        StartCoroutine(enumerator);
        preEnumerator = enumerator;
    }


    IEnumerator AnimateTo(float target)
    {
        float cur = depthOfField.focusDistance.GetValue<float>();
        while (cur != target)
        {
            cur = Mathf.Lerp(cur, target, step);
            depthOfField.focusDistance.Override(cur);
            yield return new WaitForSecondsRealtime(step);
        }
    }
}
