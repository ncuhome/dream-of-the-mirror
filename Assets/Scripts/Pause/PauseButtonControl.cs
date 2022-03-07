using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseButtonControl : MonoBehaviour
{
    public Button reStartBtn;
    public Button continueBtn;
    public Button exitBtn;

    public GameObject volumeObj;
    public float step = 0.1f;
    ChromaticAberration chromaticAberration;
    DepthOfField depthOfField;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        reStartBtn.onClick.AddListener(ReStart);
        continueBtn.onClick.AddListener(Continue);
        exitBtn.onClick.AddListener(Exit);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {   
        if (volumeObj == null)
        {
            volumeObj = GlobalVolumeManager.instance.volumeObj;
        }
        if (volumeObj != null)
        {
            var volume = volumeObj.GetComponent<Volume>();

            if (volume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
            {
            }
            else
            {
                volume.profile.Add<ChromaticAberration>(true);
                volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
            }

            if (volume.profile.TryGet<DepthOfField>(out depthOfField))
            {
            }
            else
            {
                volume.profile.Add<DepthOfField>(true);
                volume.profile.TryGet<DepthOfField>(out depthOfField);
            }
        }
    }

    void Continue()
    {
        PauseControl.gameIsPaused = !PauseControl.gameIsPaused;
        PauseControl.instance.PauseGame();
    }

    void ReStart()
    {
        PauseControl.instance.pauseMenu.SetActive(false);
        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.Override(0f);
        }
        if (depthOfField != null)
        {
            depthOfField.focusDistance.Override(10f);
        }
        StartCoroutine(ToNowScene());
    }

    void Exit()
    {
        PauseControl.instance.pauseMenu.SetActive(false);
        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.Override(0f);
        }
        if (depthOfField != null)
        {
            depthOfField.focusDistance.Override(10f);
        }
        StartCoroutine(ToStartScene());
    }

    IEnumerator ToStartScene()
    {
        if (chromaticAberration == null && depthOfField == null)
        {
            GoBack();
            yield return null;
        }
        while (chromaticAberration.intensity.value < 1)
        {
            chromaticAberration.intensity.value += step;
            if (depthOfField != null && depthOfField.focusDistance.value > 0)
            {
                depthOfField.focusDistance.value -= step * 10f;
            }
            yield return new WaitForSecondsRealtime(step);
        }
        GoBack();
    }

    IEnumerator ToNowScene()
    {
        while (chromaticAberration.intensity.value < 1)
        {
            chromaticAberration.intensity.value += step;
            if (depthOfField != null && depthOfField.focusDistance.value > 0)
            {
                depthOfField.focusDistance.value -= step * 10f;
            }
            yield return new WaitForSecondsRealtime(step);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void GoBack()
    {
        var curIndex = SceneManager.GetActiveScene().buildIndex;
        if (curIndex == 0)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
