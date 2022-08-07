using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyCanvasManager : MonoBehaviour
{
    #region Singleton

    public static DontDestroyCanvasManager instance;

    private void Awake()
    {
        instance = this;
        if (dontDestroyCanvas == null)
        {
            dontDestroyCanvas = GameObject.FindGameObjectWithTag("DontDestroyCanvas");
        }
    }

    #endregion;

    public GameObject dontDestroyCanvas;
}
