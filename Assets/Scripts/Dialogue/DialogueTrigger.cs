using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] public TextAsset inkJSON;

    [Header("NPC")]
    [SerializeField] public string npcName;
    [SerializeField] public Sprite npcAvatar;
    
    [Header("Next Dialogue")]
    [SerializeField] public GameObject nextDialogue;
    [SerializeField] public float timeNextDialogue;

    private bool playerInRange;
    //是否进入trigger范围就触发触发器
    public bool triggerDialogueWhenEnter;
    //是否按下互动键才触发
    public bool triggerDialogueWhenInteract;
    //判定是否触发对话
    private bool trigger;
    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        trigger = false;
    }

    private void Update()
    {
        //如果进入范围就触发的模式
        if (playerInRange && triggerDialogueWhenEnter)
        {
            trigger = true;
            triggerDialogueWhenEnter = false;
        }

        //如果按对话键就触发的模式
        if (playerInRange && triggerDialogueWhenInteract && (Input.GetButtonDown("Dialogue")||Input.GetMouseButtonDown(0)))
        {
            trigger = true;
        }

        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if (trigger)
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON, npcName, npcAvatar, nextDialogue, timeNextDialogue);
                trigger =  false;
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
