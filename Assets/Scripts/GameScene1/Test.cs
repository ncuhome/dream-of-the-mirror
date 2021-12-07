using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public RectTransform Background;
    public Transform bs;
    void Start()
    {
        print(Background.position);
        print(bs.position);
    }

}
