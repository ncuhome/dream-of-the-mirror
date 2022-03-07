using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVolumeManager : MonoBehaviour
{
    #region Singleton

    public static GlobalVolumeManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion;

    public GameObject volumeObj;
}
