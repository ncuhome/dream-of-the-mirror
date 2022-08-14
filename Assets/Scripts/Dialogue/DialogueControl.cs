using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueControl : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Image avatar;
    public Sprite bedAvatar;
    public Sprite statueAvatar;
    public Sprite mirrorAvatar;
    public Sprite playerAvatar;
    public Animator mirrorLightAnim;
    public GameObject mirrorLight; 
    public TextAsset inkAsset;
    public float textSpeed;
    public float fadeDuration = 0.5f;
    

    private bool isInDialogue = false;
    private string currentReadyDialogue = "";
    private Story inkStroy;
    private LoadCanvasControl load;

    private string currentName;
    private string currentWord;
    private Sprite currentSprite;
    private bool textFinished = true;
    private float currentTextSpeed;
    private bool hasShowDialogue = false;

    private bool bedHasDialogue;
    private bool statueHasDialogue;
    private bool canMirrorDialogueShow = false;

    public bool CanMirrorDialogueShow
    {
        get{
            return canMirrorDialogueShow;
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

    private void Start()
    {
        inkStroy = new Story(inkAsset.text);
        dialoguePanel.SetActive(false);
        currentTextSpeed = textSpeed;
        mirrorLight.SetActive(false);
        load = DontDestroyCanvasManager.instance.dontDestroyCanvas.GetComponent<LoadCanvasControl>();
    }

    private void Update()
    {
        if (hasShowDialogue)
        {
            return;
        }
        if (bedHasDialogue && statueHasDialogue)
        {
            ShowMirrorDialogue();
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

    void ShowMirrorDialogue()
    {
        canMirrorDialogueShow = true;
        mirrorLight.SetActive(true);
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
        if (currentReadyDialogue == "mirror")
        {
            dialoguePanel.SetActive(false);
            hasShowDialogue = true;
            EndScene();
            return;
        }
        isInDialogue = false;
        dialoguePanel.SetActive(false);
        hasShowDialogue = true;
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
            case "bed":
                currentSprite = bedAvatar;
                bedHasDialogue = true;
                break;
            case "statue":
                currentSprite = statueAvatar;
                statueHasDialogue = true;
                break;
            case "mirror":
                currentSprite = mirrorAvatar;
                break;
            case "player":
                currentSprite = playerAvatar;
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
        mirrorLightAnim.SetTrigger("Crease");
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
        StaticData.storeSceneIndex = 3;
        PlayerPrefs.SetInt("StoreSceneIndex", 3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}