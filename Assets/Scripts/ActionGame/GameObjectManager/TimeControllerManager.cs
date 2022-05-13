using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControllerManager : MonoBehaviour
{
    #region Singleton

    public static TimeControllerManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion;

    public TimeController timeController;
}
