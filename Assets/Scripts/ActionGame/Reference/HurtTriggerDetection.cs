using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtTriggerDetection : MonoBehaviour
{
    public DevilPhysicsComponent physic;

    public void GetHurt(Damage damage)
    {
        physic.EnemyHurt(damage);
    }
}
