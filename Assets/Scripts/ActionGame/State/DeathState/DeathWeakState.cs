using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWeakState : DeathState
{
    public override void Enter(Death death)
    {
        base.Enter(death);
        GetEndTime("Death_Weak");
        death.Anim_.Animator_.SetTrigger("Weak");
    }

    protected override void Start()
    {
        
    }

    public override void StateFixedUpdate()
    {
        death_.Physics_.HorizontalMove(0, 0);
    }

    public override void StateUpdate()
    {
        if (Time.time > stateTime)
        {
            death_.TranslationState(walking);
        }
    }
}
