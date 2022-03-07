using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionJoyStickManager : MonoBehaviour
{
    #region Singleton

    public static DirectionJoyStickManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion;

    public GameObject directionJoyStick;
}
