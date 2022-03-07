using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderControllerManager : MonoBehaviour
{
    #region Singleton

    public static SliderControllerManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion;

    public GameObject sliderController;
}
