using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroineShootingState : HeroineState
{
    [Header("魔法冲击波预制件")]
    public Bullet bullet;
    public AudioSource attackAudio;
    public float shootCd;

    private float lastShootTime;

    public override bool CanEnter(GirlHero girlHero)
    {
        if (Time.time < lastShootTime + shootCd)
        {
            return false;
        }
        return true;
    }

    public override void Enter(GirlHero girlHero)
    {
        base.Enter(girlHero);
        GetEndTime("GirlHero_Shoot");
        girlHero.Anim_.Animator_.SetTrigger("Shoot");
        girlHero.PlayAudio(attackAudio);
        girlHero.Physics_.ResetSpeed();
    }

    public override void Exit()
    {
        lastShootTime = Time.time;
        Instantiate(bullet, girlHero_.transform.position + girlHero_.transform.right, girlHero_.transform.rotation);
    }

    public override HeroineState HandleCommand(GirlHero girlHero, TranslationCommand translationCommand, Command buttonCommand)
    {
        if (translationCommand is RepelCommand)
        {
            Exit();
            return HeroineState.repelling;
        }
        return null;
    }

    protected override void Start()
    {
        
    }

    public override void StateUpdate()
    {
        if (Time.time > stateTime)
        {
            Exit();
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
}