using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroineRollingState : HeroineState
{
    public float rollSpeed;
    public int maxRollCount = 1;
    public AudioSource rollAudio;
    public float rollCd;

    private int remainRollCount = 0;
    private float lastRollTime;

    public int RemainRollCount
    {
        set
        {
            remainRollCount = value;
        }
    }

    public override bool CanEnter(GirlHero girlHero)
    {
        return CanRoll(girlHero);
    }

    public override void Enter(GirlHero girlHero)
    {
        base.Enter(girlHero);
        GetEndTime("GirlHero_Roll");
        girlHero.Anim_.Animator_.SetTrigger("Roll");
        girlHero.Particle_.CreateDust();
        girlHero.PlayAudio(rollAudio);
        girlHero.Physics_.ResetSpeed();
    }

    public override void Exit()
    {
        lastRollTime = Time.time;
        girlHero_.Physics_.ResetSpeed();
    }

    public override HeroineState HandleCommand(GirlHero girlHero, TranslationCommand translationCommand, Command buttonCommand)
    {
        if (translationCommand is RepelCommand)
        {
            return HeroineState.repelling;
        }
        return null;
    }

    protected override void Start()
    {
        remainRollCount = maxRollCount;
    }

    public override void StateFixedUpdate()
    {
        girlHero_.Physics_.RollMove(rollSpeed);
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

    private bool CanRoll(GirlHero girlHero)
    {
        if (Time.time < lastRollTime + rollCd)
        {
            return false;
        }
        if (remainRollCount <= 0)
        {
            return false;
        }
        remainRollCount--;
        return true;
    }

    public void InitCount()
    {
        remainRollCount = maxRollCount;
    }
}