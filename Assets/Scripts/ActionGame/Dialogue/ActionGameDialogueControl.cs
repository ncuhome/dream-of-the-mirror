using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ActionGameDialogueControl : MonoBehaviour
{
    public GameObject devil;
    public GameObject death;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Image avatar;
    public Sprite storeAvatar;
    public Sprite player1Avatar;
    public Sprite player2Avatar;
    public Sprite deathAvatar;
    public Sprite deerAvatar;
    public Sprite devilAvatar;
    public Sprite treeAvatar;
    public TextAsset inkAsset;
    public Animator lightAnim;
    public GameObject endStoreLight;
    public float textSpeed;
    public float fadeDuration = 0.5f;
    
    private bool isInDialogue = false;
    private string currentReadyDialogue = "";
    private Story inkStroy;
    private LoadCanvasControl load;

    private string currentName;
    private string currentWord;
    private Sprite currentSprite;
    private SavePoint currentSavePoint;
    private EnemyTrigger currentEnemyTrigger;
    private bool textFinished = true;
    private float currentTextSpeed;
    private bool hasShowDialogue = false;
    private bool canEndStoreDialogueShow = false;

    public bool CanEndStoreDialogueShow
    {
        get{
            return canEndStoreDialogueShow;
        }
        set{
            canEndStoreDialogueShow = value;
        }
    }
    
    public bool HasShowDialogue
    {
        set{
            hasShowDialogue = value;
        }
    }

    public string CurrentReadyDialogue
    {
        get{
            return currentReadyDialogue;
        }
        set{
            currentReadyDialogue = value;
        }
    }
    
    public bool IsInDialogue
    {
        get{
            return isInDialogue;
        }
    }

    public EnemyTrigger CurrentEnemyTrigger
    {
        set{
            currentEnemyTrigger = value;
        }
    }

    public SavePoint CurrentSavePoint
    {
        set{
            currentSavePoint = value;
        }
    }

    private void Start()
    {
        inkStroy = new Story(inkAsset.text);
        dialoguePanel.SetActive(false);
        currentTextSpeed = textSpeed;
        load = DontDestroyCanvasManager.instance.dontDestroyCanvas.GetComponent<LoadCanvasControl>();
    }

    private void Update()
    {
        if (hasShowDialogue)
        {
            return;
        }
        if (death == null)
        {
            ShowEndStoreDialogue();
        }
        if (isInDialogue)
        {
            if (textFinished)
            {
                GetNextData();
            }
            else
            {
                if (Input.anyKeyDown)
                {
                    currentTextSpeed = textSpeed / 10;
                }
            }
        }
        else if (Input.anyKeyDown && currentReadyDialogue != "")
        {
            StartDialogue(currentReadyDialogue);
        }
    }

    void GetNextData()
    {
        if (GetData(ref currentName, ref currentWord, ref currentSprite))
        {
            textFinished = false;
            StartCoroutine(ShowDialogue());
        }
        else
        {
            FinishDialogue();
        }
    }

    void ShowEndStoreDialogue()
    {
        canEndStoreDialogueShow = true;
        endStoreLight.SetActive(true);
    }

    void StartDialogue(string path)
    {
        isInDialogue = true;
        dialoguePanel.SetActive(true);
        inkStroy.ChoosePathString(path);
        if (!GetData(ref currentName, ref currentWord, ref currentSprite))
        {
            Debug.Log("这是一个空结");
        }
        else
        {
            StartCoroutine(ShowDialogue());
        }
    }

    void FinishDialogue()
    {
        if (currentReadyDialogue == "store7")
        {
            CurrentReadyDialogue = "";
            isInDialogue = false;
            dialoguePanel.SetActive(false);
            hasShowDialogue = false;
            EndScene();
        }
        if (currentReadyDialogue == "devilEnd")
        {
            CurrentReadyDialogue = "";
            isInDialogue = false;
            dialoguePanel.SetActive(false);
            hasShowDialogue = false;
            Destroy(devil);
            return;
        }
        if (currentReadyDialogue == "deathEnd")
        {
            CurrentReadyDialogue = "";
            isInDialogue = false;
            dialoguePanel.SetActive(false);
            hasShowDialogue = false;
            Destroy(death);
            return;
        }
        if (currentSavePoint != null)
        {
            currentSavePoint.EndDialogue();
            currentSavePoint = null;
        }
        if (currentEnemyTrigger != null)
        {
            currentEnemyTrigger.MakeEnemyMove();
        }
        isInDialogue = false;
        dialoguePanel.SetActive(false);
        hasShowDialogue = true;
        PlayerManager.instance.girlHero.GetComponent<GirlHero>().CanMove = true;
        PlayerManager.instance.girlHero.GetComponent<GirlHero>().ResetAnim();
    }

    bool GetData(ref string currentName, ref string currentWord, ref Sprite currentSprite)
    {
        if (!inkStroy.canContinue)
        {
            return false;
        }
        string s = inkStroy.Continue();
        string [] value = s.Split(':');
        currentName = value[0];
        currentWord = value[1];
        switch (currentName)
        {
            case "store":
                currentSprite = storeAvatar;
                break;
            case "death":
                currentSprite = deathAvatar;
                break;
            case "deer":
                currentSprite = deerAvatar;
                break;
            case "devil":
                currentSprite = devilAvatar;
                break;
            case "player1":
                currentSprite = player1Avatar;
                break;
            case "player2":
                currentSprite = player2Avatar;
                break;
            case "tree":
                currentSprite = treeAvatar;
                break;
        }
        return true;
    }
    
    void EndScene()
    {
        CreaseLight();
        StartCoroutine(LoadNextScene());
    }

    void CreaseLight()
    {
        lightAnim.SetTrigger("Crease");
    }

    IEnumerator ShowDialogue()
    {
        textFinished = false;
        nameText.text = currentName;
        avatar.sprite = currentSprite;
        dialogueText.text = "";
        int num = currentWord.Length;
        for(int i = 0; i < num; ++i)
        {
            dialogueText.text += currentWord[i];

            yield return new WaitForSeconds(currentTextSpeed);
        }
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        textFinished = true;
        currentTextSpeed = textSpeed;
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
        StaticData.storeSceneIndex = 5;
        PlayerPrefs.SetInt("StoreSceneIndex", 5);
        StaticData.storePoint = 0;
        PlayerPrefs.SetInt("StorePoint", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public IEnumerator ReLoadScene()
    {
        load.FadeOut(fadeDuration);
        yield return new WaitForSeconds(fadeDuration);

        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
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
        StaticData.storeSceneIndex = 5;
        PlayerPrefs.SetInt("StoreSceneIndex", 5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
