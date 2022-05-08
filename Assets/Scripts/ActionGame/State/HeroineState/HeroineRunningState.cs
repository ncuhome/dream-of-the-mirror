using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroineRunningState : HeroineState
{
    public float moveSpeed;
    public AudioSource runAudio;

    private int runDir;

    public override void Enter(GirlHero girlHero)
    {
        base.Enter(girlHero);
        girlHero.Particle_.CreateDust();
        girlHero.Anim_.Animator_.SetTrigger("Run");
        girlHero.PlayAudio(runAudio);
    }

    public override void Exit()
    {
        girlHero_.StopAudio(runAudio);
    }

    public override HeroineState HandleCommand(GirlHero girlHero, TranslationCommand translationCommand, Command buttonCommand)
    {
        if (translationCommand is RepelCommand)
        {
            Exit();
            return HeroineState.repelling;
        }

        runDir = (int)translationCommand.Horizontal;
        if (runDir == 0)
        {
            Exit();
            return HeroineState.idling;
        }

        if (buttonCommand is JumpCommand)
        {
            Exit();
            return HeroineState.jumping;
        }
        if (buttonCommand is SwordCommand)
        {
            Exit();
            return HeroineState.swording;
        }
        if (buttonCommand is ShootCommand)
        {
            Exit();
            return HeroineState.shooting;
        }
        if (buttonCommand is RollCommand)
        {
            Exit();
            return HeroineState.rolling;
        }
        return null;
    }

    protected override void Start()
    {
        
    }

    public override void StateFixedUpdate()
    {
        girlHero_.Physics_.HorizontalMove(runDir, moveSpeed);
    }
}