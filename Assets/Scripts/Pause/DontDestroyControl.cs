using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyControl : MonoBehaviour
{
    static bool isDontDestroy = false;
    void Awake()
    {
        if (!isDontDestroy)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        isDontDestroy = true;
    }
}
