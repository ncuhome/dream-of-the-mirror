using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    //现在是解谜的第几关（用于读取）
    public int level = 1;
    //当人物生成后，人物脚本会赋值给SceneController与HeroMoveController
    public Hero leftHero;
    public Hero rightHero;
    public GameObject volumeObj;
    public TextAsset _inkAsset;
    public GameObject leftHeroPanel;
    public GameObject rightHeroPanel;
    public TMP_Text leftHeroText;
    public TMP_Text rightHeroText;
    public Animator lightAnim;
    public float step = 0.1f;
    public float textSpeed;
    public float fadeDuration = 0.5f;

    ChromaticAberration chromaticAberration;
    DepthOfField depthOfField;

    private LoadCanvasControl load;
    private bool isToNextScene = false;
    private bool hasDialogue = false;
    private bool isInDialogue = false;
    private Story _inkStroy;
    private string currentName;
    private string currentWord;
    private Color currentColor;
    private bool textFinished = true;

    public bool IsInDialogue
    {
        get{
            return isInDialogue;
        }
    }

    void Awake()
    {
        if (this != null)
            instance = this;
    }

    void Start()
    {
        _inkStroy = new Story(_inkAsset.text);
        load = DontDestroyCanvasManager.instance.dontDestroyCanvas.GetComponent<LoadCanvasControl>();
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

    //若两个Hero同时到达终点，加载下一个场景
    void Update()
    {
        //因为人物是子对象，加载较慢
        if (leftHero == null || rightHero == null)
        {
            return;
        }

        if (!hasDialogue)
        {
            
            BeginDialogue();
            return;
        }

        if (isInDialogue)
        {
            if (textFinished)
            {
                GetNextData();
            }
            return;
        }

        if (leftHero.ended && rightHero.ended)
        {
            if (!isToNextScene)
            {
                // if (chromaticAberration != null)
                // {
                //     chromaticAberration.intensity.Override(0f);
                // }
                // if (depthOfField != null)
                // {
                //     depthOfField.focusDistance.Override(10f);
                // }
                StartCoroutine(ToActionScene());
                isToNextScene = true;
            }
        }
    }

    void BeginDialogue()
    {
        hasDialogue = true;
        isInDialogue = true;
        if (!GetData(ref currentName, ref currentWord))
        {
            Debug.Log("这是一个空结");
        }
        else
        {
            StartCoroutine(ShowDialogue());
        }
    }

    void GetNextData()
    {
        if (GetData(ref currentName, ref currentWord))
        {
            textFinished = false;
            StartCoroutine(ShowDialogue());
        }
        else
        {
            FinishDialogue();
        }
    }

    void FinishDialogue()
    {
        isInDialogue = false;
        leftHeroPanel.SetActive(false);
        rightHeroPanel.SetActive(false);
    }

    bool GetData(ref string currentName, ref string currentWord)
    {
        if (!_inkStroy.canContinue)
        {
            return false;
        }
        string s = _inkStroy.Continue();
        string [] value = s.Split(':');
        currentName = value[0];
        currentWord = value[1];
        if (value.Length == 2)
        {
            currentColor = Color.white;
        }
        else
        {
            currentColor = Color.red;
        }
        return true;
    }

    IEnumerator ShowDialogue()
    {
        textFinished = false;
        int num = 0;
        switch (currentName)
        {
            case "player1":
                leftHeroPanel.SetActive(true);
                rightHeroPanel.SetActive(false);
                leftHeroText.color = currentColor;
                leftHeroText.text = "";
                num = currentWord.Length;
                for(int i = 0; i < num; ++i)
                {
                    leftHeroText.text += currentWord[i];
                    yield return new WaitForSeconds(textSpeed);
                }
                break;
            case "player2":
                leftHeroPanel.SetActive(false);
                rightHeroPanel.SetActive(true);
                rightHeroText.color = currentColor;
                rightHeroText.text = "";
                num = currentWord.Length;
                for(int i = 0; i < num; ++i)
                {
                    rightHeroText.text += currentWord[i];
                    yield return new WaitForSeconds(textSpeed);
                }
                break;
        }
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        textFinished = true;
    }

    IEnumerator ToActionScene()
    {
        lightAnim.SetTrigger("Crease");
        while (chromaticAberration.intensity.value < 0.8)
        {
            chromaticAberration.intensity.value += step;
            yield return new WaitForSeconds(step);
        }

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
        load.FadeIn(fadeDuration);
        StaticData.storeSceneIndex = 4;
        PlayerPrefs.SetInt("StoreSceneIndex", 4);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        // AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        // operation.allowSceneActivation = false;

        // while (!operation.isDone)
        // {
        //     step = operation.progress;
        //     chromaticAberration.intensity.value = step;
        //     if (depthOfField != null && depthOfField.focusDistance.value > 0)
        //     {
        //         depthOfField.focusDistance.value = 10 - step * 10f;
        //     }

        //     if (operation.progress >= 0.9f)
        //     {
        //         if (Input.anyKeyDown)
        //         {
        //             operation.allowSceneActivation = true;
        //         }
        //     }
        //     yield return null;
        // }
    }

    private void OnLoadScene(AsyncOperation obj)
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
    }
}
