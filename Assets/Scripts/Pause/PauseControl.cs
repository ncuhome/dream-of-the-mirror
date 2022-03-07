using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PauseControl : MonoBehaviour
{
    public static PauseControl instance;
    public GameObject volumeObj;
    ChromaticAberration chromaticAberration;
    DepthOfField depthOfField;

    public static bool gameIsPaused = false;
    public float step = 0.1f;
    public GameObject pauseMenu;
    public float onPausedFocusDistance;

    private float defaultFocusDistance;

    IEnumerator preEnumerator;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (instance == null)
        {
            instance = this;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (preEnumerator != null)
        {
            StopCoroutine(preEnumerator);
        }
        gameIsPaused = false;
        Time.timeScale = 1;
        //下面这句会切换场景后会报错，但是找不到错误原因
        pauseMenu.SetActive(false);
        
        if (volumeObj == null)
        {
            volumeObj = GlobalVolumeManager.instance.volumeObj;
        }
        if (volumeObj != null)
        {
            var volume = volumeObj.GetComponent<Volume>();
            if (volume.profile.TryGet<DepthOfField>(out depthOfField))
            {
            }
            else
            {
                volume.profile.Add<DepthOfField>(true);
                volume.profile.TryGet<DepthOfField>(out depthOfField);
            }
            defaultFocusDistance = (float)depthOfField.focusDistance;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && SceneManager.GetActiveScene().buildIndex != 0)
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

    public void PauseGame()
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
            enumerator = AnimateToFocusDistance(onPausedFocusDistance);
        }
        else
        {
            enumerator = AnimateToFocusDistance(defaultFocusDistance);
        }
        StartCoroutine(enumerator);
        preEnumerator = enumerator;
    }


    IEnumerator AnimateToFocusDistance(float target)
    {
        float init = depthOfField.focusDistance.GetValue<float>();
        float cur = depthOfField.focusDistance.GetValue<float>();
        while (Mathf.Abs(cur - target) > 0.1)
        {
            cur = Mathf.Lerp(cur, target, step);
            depthOfField.focusDistance.Override(init + (cur - init));
            yield return new WaitForSecondsRealtime(step);
        }
    }
}
