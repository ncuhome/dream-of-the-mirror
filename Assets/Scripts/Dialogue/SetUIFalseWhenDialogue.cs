using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUIFalseWhenDialogue : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]  private GameObject UI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.GetInstance()!=null)
        {
            if(DialogueManager.GetInstance().dialogueIsPlaying)
            {
                UI.SetActive(false);
            }else
            {
                UI.SetActive(true);
            }
        }
    }
}
