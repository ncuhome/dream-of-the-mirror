using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroineRepelState : HeroineState
{
    public float repelForce;
    public Vector2 repelDir;
    public AudioSource repelAudio;

    private bool hasRepel = false;

    public override void Enter(GirlHero girlHero)
    {
        base.Enter(girlHero);
        GetEndTime("GirlHero_Repel");
        InitRepelDir();
        girlHero.Anim_.Animator_.SetTrigger("Repel");
        hasRepel = false;
    }

    protected override void Start()
    {
        
    }

    public override void StateFixedUpdate()
    {
        if (!hasRepel)
        {
            girlHero_.Physics_.RepelMove(repelDir, repelForce);
            hasRepel = true;
        }
    }

    public override void StateUpdate()
    {
        if (Time.time > stateTime)
        {
            if (girlHero_.Physics_.IsGrounded)
            {
                girlHero_.TranslationState(idling);
            }
            else
            {
                girlHero_.TranslationState(floating);
            }
        }
    }

    private void InitRepelDir()
    {
        if (InputHandlerManager.instance.inputHandler.Repel.Horizontal > 0)
        {
            repelDir.x = Mathf.Abs(repelDir.x);
        }
        else
        {
            repelDir.x = Mathf.Abs(repelDir.x) * (-1);
        }
        InputHandlerManager.instance.inputHandler.DestroyRepel();
    }
}