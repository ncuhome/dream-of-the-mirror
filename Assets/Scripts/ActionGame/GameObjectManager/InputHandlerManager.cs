using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandlerManager : MonoBehaviour
{
    #region Singleton

    public static InputHandlerManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion;

    public InputHandler inputHandler;
}
