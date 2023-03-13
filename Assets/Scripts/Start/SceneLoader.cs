using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoader : MonoBehaviour
{
    public Button startButton;
    public Button continueButton;
    public Button exitButton;
    public Button startCommitButton;
    public Button startCancelButton;
    public GameObject commitPanel;
    public GameObject nonePanel;
    public GameObject volumeObj;
    public ExitControl exitControl;
    public int sceneIndex;
    public float volumeStep;
    public float fadeDuration = 0.25f;

    ChromaticAberration chromaticAberration;
    private LoadCanvasControl load;
    private float loadPer;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("StoreSceneIndex"))
        {
            // PlayerPrefs.SetInt("StoreSceneIndex", 0);
            StaticData.storeSceneIndex = PlayerPrefs.GetInt("StoreSceneIndex");
        }
        else
        {
            PlayerPrefs.SetInt("StoreSceneIndex", 0);
        }

        if (PlayerPrefs.HasKey("StorePoint"))
        {
            // PlayerPrefs.SetInt("StorePoint", 0);
            StaticData.storePoint = PlayerPrefs.GetInt("StorePoint");
        }
        else
        {
            PlayerPrefs.SetInt("StorePoint", 0);
        }
    }

    void Start()
    {
        load = DontDestroyCanvasManager.instance.dontDestroyCanvas.GetComponent<LoadCanvasControl>();
        commitPanel.SetActive(false);
        Volume volume = volumeObj.GetComponent<Volume>();

        if (volume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
        {
            startButton.onClick.AddListener(() => ShowCommitUI());
            continueButton.onClick.AddListener(() => StartCoroutine("LoadStoreScene"));
            exitButton.onClick.AddListener(() => StartCoroutine("ExitGame"));
            startCommitButton.onClick.AddListener(() => StartCoroutine("LoadNextScene"));
            startCancelButton.onClick.AddListener(() => StartCoroutine("HideCommitUI"));
        }
    }

    void ShowCommitUI()
    {
        if (StaticData.storeSceneIndex == 0)
        {
            StartCoroutine(LoadNextScene());
        }
        else
        {
            commitPanel.SetActive(true);
        }
    }

    void HideCommitUI()
    {
        commitPanel.SetActive(true);
    }

    void ExitGame()
    {
        exitControl.gameEnd = true;
    }

    IEnumerator LoadStoreScene()
    {
        if (StaticData.storeSceneIndex == 0)
        {
            nonePanel.SetActive(true);
            yield return new WaitForSeconds(1f);
            nonePanel.SetActive(false);
        }
        else
        {
            commitPanel.SetActive(false);
            startButton.onClick.RemoveAllListeners();
            continueButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
            startCommitButton.onClick.RemoveAllListeners();
            startCancelButton.onClick.RemoveAllListeners();
            while (chromaticAberration.intensity.value < 0.8)
            {
                chromaticAberration.intensity.value += volumeStep;
                yield return new WaitForSeconds(volumeStep);
            }

            load.FadeOut(fadeDuration);
            yield return new WaitForSeconds(fadeDuration);

            AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + StaticData.storeSceneIndex);
            operation.allowSceneActivation = false;

            float tPer = 0;
            while (tPer <= 0.9)
            {
                load.LoadFont(tPer);
                tPer += load.fontStep;
                yield return new WaitForSeconds(load.fontStep);
            }

            while (operation.progress < 0.9f)
            {
                yield return null;
            }

            load.LoadFont(1f);
            load.FontFlash();
            while (!Input.anyKeyDown)
            {
                yield return null;
            }

            operation.allowSceneActivation = true;
            load.FadeIn(fadeDuration);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + StaticData.storeSceneIndex);
        }
    }

    IEnumerator LoadNextScene()
    {
        commitPanel.SetActive(false);
        startButton.onClick.RemoveAllListeners();
        continueButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
        startCommitButton.onClick.RemoveAllListeners();
        startCancelButton.onClick.RemoveAllListeners();
        while (chromaticAberration.intensity.value < 0.8)
        {
            chromaticAberration.intensity.value += volumeStep;
            yield return new WaitForSeconds(volumeStep);
        }

        load.FadeOut(fadeDuration);
        yield return new WaitForSeconds(fadeDuration);

        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + sceneIndex);
        operation.allowSceneActivation = false;

        float tPer = 0;
        while (tPer <= 0.9)
        {
            load.LoadFont(tPer);
            tPer += load.fontStep;
            yield return new WaitForSeconds(load.fontStep);
        }

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        load.LoadFont(1f);
        load.FontFlash();
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        operation.allowSceneActivation = true;
        load.FadeIn(fadeDuration);
        StaticData.storeSceneIndex = 1;
        PlayerPrefs.SetInt("StoreSceneIndex", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + sceneIndex);
    }

}
