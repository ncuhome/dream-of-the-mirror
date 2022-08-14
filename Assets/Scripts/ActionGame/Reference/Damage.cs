using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Damage {
    public enum DamageType 
    {
        MeleeAttack,
        RemoteAttack
    } 
    public int damageValue { get; private set; }
    public Vector2 damageDir { get; private set; }
    public Vector2 damagePos { get; private set; }
    public DamageType damageType {get; private set;}
        

    public Damage(int v, Vector2 dir, Vector2 pos,DamageType type) {
        damageValue = v;
        damageDir = dir;
        damagePos = pos;
        damageType = type;
    }
}