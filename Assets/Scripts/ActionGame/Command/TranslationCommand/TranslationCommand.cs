using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationCommand : Command
{
    private float horizontal = 0;
    private float vertical = 0;

    public TranslationCommand(float _x, float _y)
    {
        horizontal = _x;
        vertical = _y;
    }

    public float Horizontal
    {
        get
        {
            return horizontal;
        }
    }

    public float Vertical
    {
        get
        {
            return vertical;
        }
    }
}
