using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image avatar;
    [SerializeField] private TextMeshProUGUI nameText;
    
    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;

    [Header("NPC")]
    [SerializeField] private string npcName;
    [SerializeField] private Sprite npcAvatar;

    [Header("Player")]
    [SerializeField] private string playerName;
    [SerializeField] private Sprite playerAvatar; 
    
    //下一个触发器的信息
    private bool haveNextDialogue;
    private GameObject nextTrigger;
    private float timeNextDialogue;
    private TextAsset nextInkJSON;
    private string nextNpcName;
    private Sprite nextNpcAvatar;
    private GameObject thirdTrigger;
    private float timeThirdDialogue;
    private bool waitForNextDialogue;

    
    private  TextMeshProUGUI[] choicesText;
    private  string text;//当前行文本
    private Story currentStory;
    
    public bool dialogueIsPlaying { get; private set; }//是否处于对话框模式
    private bool makeChoice , textIsFinished;//是否做出选择（主角发言），当前行文本是否播放完毕
    private static DialogueManager instance;

    public float textSpeed; //文本播放速度

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        haveNextDialogue = false;
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if ((!dialogueIsPlaying)||(waitForNextDialogue))
        {
            return;
        }

        

        if (Input.GetButtonDown("Dialogue")||(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)||(Input.GetMouseButtonDown(0)))//按下互动键
        {
            if (currentStory.currentChoices.Count != 0)
            {
                if (makeChoice)
                {
                    if (textIsFinished)
                    {
                        //主角发言时切换到主角头像与名字
                        makeChoice = false;
                        nameText.text = playerName;
                        avatar.sprite = playerAvatar;
                        //如果文本播放完毕则进如下行
                        ContinueStory();
                    }
                    else
                    {
                        //否则停止播放文本，直接全部展出，并且滞后0.2s切换textIsFinished开关，保证下次按下互动键才切换到下行
                        StopCoroutine("DisPlayDialogue");
                        dialogueText.text = text;
                        StartCoroutine(FinishText());
                    }
                }
            }
            else
            {
                if (textIsFinished)
                {
                    nameText.text = npcName;
                    avatar.sprite = npcAvatar;
                    //如果文本播放完毕则进如下行
                    ContinueStory();
                }
                else
                {
                    //否则停止播放文本，直接全部展出，并且滞后0.2s切换textIsFinished开关，保证下次按下互动键才切换到下行
                    StopCoroutine("DisPlayDialogue");
                    dialogueText.text = text;
                    StartCoroutine(FinishText());
                }
            }
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON, string n_name, Sprite n_avatar, GameObject nextDialogue, float time)
    {
        currentStory = new Story(inkJSON.text);
        npcName = n_name;
        npcAvatar = n_avatar;

        nameText.text = npcName;
        avatar.sprite = npcAvatar;

        //如果有下段，则获取下段信息
        if (nextDialogue != null)
        {
            haveNextDialogue = true;
            nextTrigger = nextDialogue;
            timeNextDialogue = time;
            nextInkJSON = nextTrigger.GetComponent<DialogueTrigger>().inkJSON;
            nextNpcAvatar = nextTrigger.GetComponent<DialogueTrigger>().npcAvatar;
            nextNpcName = nextTrigger.GetComponent<DialogueTrigger>().npcName;
            
            thirdTrigger = nextTrigger.GetComponent<DialogueTrigger>().nextDialogue;
            timeThirdDialogue = nextTrigger.GetComponent<DialogueTrigger>().timeNextDialogue;
        }
        else
        {
            haveNextDialogue = false;
        }

        textIsFinished = true;
        makeChoice = false; 
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        dialogueText.text = "";

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        if (haveNextDialogue)//如有下段则在给定时间后播放下段
        {
            dialoguePanel.SetActive(false);
            dialogueText.text = "";
            StartCoroutine(NextDialogue());

            yield return null;
        }
        else//否则延迟0.2s退出对话模式
        {
            dialoguePanel.SetActive(false);
            dialogueText.text = "";
            yield return new WaitForSeconds(0.2f);

            dialogueIsPlaying = false;
            if (NextScene.GetInstance() != null)
            {
                if (NextScene.GetInstance().enterWhenExitDialogue)
                {
                    NextScene.GetInstance().exitDialogueMode = true;
                }
            }
        }
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)//如有下行则进入下行
        {
            dialogueText.text = "";
            text = currentStory.Continue();
            StartCoroutine("DisPlayDialogue");
            DisPlayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    public void DisPlayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " 
            + currentChoices.Count);
        }

        //显示有的选项
        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        } 
        //隐藏没有的选项
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    //将当前选项置于第一个选项
    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }
    
    public void MakeChoice(int choiceIndex)//由选项按钮触发
    {
        if (textIsFinished)//保证文本播放完毕后才可选选项
        {
            StartCoroutine(turnChoice());
            currentStory.ChooseChoiceIndex(choiceIndex);
        }
    }

    public IEnumerator turnChoice()
    {
        //yield return new WaitForSeconds(0.1f); 
        yield return null;
        makeChoice = true;
        nameText.text = playerName;
        avatar.sprite = playerAvatar;
    }

    public  IEnumerator DisPlayDialogue()//一个字一个字显示文本
    {
        textIsFinished = false;
        for (int i = 0; i < text.Length; i++)
        {
            dialogueText.text += text[i];

            yield return new WaitForSeconds(textSpeed);
        }
        textIsFinished = true;
    }

    public IEnumerator NextDialogue()//在给定时间后进入下段对话
    {
        waitForNextDialogue = true;
        yield return new WaitForSeconds(timeNextDialogue);
        waitForNextDialogue = false;
        EnterDialogueMode(nextInkJSON, nextNpcName, nextNpcAvatar, thirdTrigger, timeThirdDialogue);
    }

    public IEnumerator FinishText()
    {
        yield return new WaitForSeconds(0.2f);
        textIsFinished = true;
    }
}
