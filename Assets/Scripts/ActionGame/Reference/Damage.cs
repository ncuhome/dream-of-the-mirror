using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Damage {
    public int damageValue { get; private set; }
    public Vector2 damageDir { get; private set; }
    public Vector2 damagePos { get; private set; }
    public Damage(int v, Vector2 dir, Vector2 pos) {
        damageValue = v;
        damageDir = dir;
        damagePos = pos;
    }
}