using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    #region Singleton

    public static DialogueManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion;

    public DialogueControl dialogueControl;
}
