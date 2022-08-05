using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubtitleControl : MonoBehaviour
{
    public float fadeDuration = 0.5f;

    private Animator subtitle;
    private bool isBegin;
    private bool isEnd;
    private LoadCanvasControl load;

    void Start()
    {
        isBegin = false;    
        isEnd = false;
        subtitle = GetComponent<Animator>();
        load = DontDestroyCanvasManager.instance.dontDestroyCanvas.GetComponent<LoadCanvasControl>();
    }

    void Update()
    {
        if (Input.anyKeyDown && !isEnd)
        {
            if (!isBegin)
            {
                isBegin = true;
                subtitle.SetTrigger("Fade");
            }
            else if (subtitle.GetCurrentAnimatorStateInfo(0).IsName("Flash"))
            {
                isEnd = true;
                StartCoroutine(LoadNextScene());
            }
        }    
    }

    IEnumerator LoadNextScene()
    {
        load.FadeOut(fadeDuration);
        yield return new WaitForSeconds(fadeDuration);

        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
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
        load.FadeIn(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
