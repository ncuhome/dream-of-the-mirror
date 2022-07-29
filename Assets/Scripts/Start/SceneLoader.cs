using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoader : MonoBehaviour
{
    public Button button;
    public GameObject volumeObj;
    public int sceneIndex;
    public float volumeStep;
    public float fadeDuration = 0.25f;
    public float fontStep = 0.1f;

    ChromaticAberration chromaticAberration;
    private LoadCanvasControl load;
    private float loadPer;

    void Start()
    {
        load = DontDestroyCanvasManager.instance.dontDestroyCanvas.GetComponent<LoadCanvasControl>();
        Volume volume = volumeObj.GetComponent<Volume>();

        if (volume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
        {
            button.onClick.AddListener(() => StartCoroutine("LoadNextScene"));
        }
    }

    IEnumerator LoadNextScene()
    {
        button.onClick.RemoveAllListeners();
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
            tPer += fontStep;
            yield return new WaitForSeconds(fontStep);
        }

        Debug.Log("djl");

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        Debug.Log("fxd");
        load.LoadFont(1f);
        load.FontFlash();
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        operation.allowSceneActivation = true;
        load.FadeIn(fadeDuration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + sceneIndex);

        // while (!operation.isDone)
        // {
        //     loadPer = operation.progress;
        //     load.Load(loadPer);

        //     if (operation.progress >= 0.9f)
        //     {
        //         if (Input.anyKeyDown)
        //         {
        //             break;
        //         }
        //     }
        //     yield return null;
        // }
        // operation.allowSceneActivation = true;
        // load.FadeIn(fadeDuration);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + sceneIndex);
    }

}
