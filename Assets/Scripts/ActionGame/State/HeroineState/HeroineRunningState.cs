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

    public override HeroineState HandleCommand(GirlHero girlHero, MoveCommand moveCommand, ActionCommand actionCommand)
    {
        if (moveCommand.type == MoveCommand.MoveType.repel)
        {
            return HeroineState.repelling;
        }

        runDir = (int)moveCommand.horizontal;
        if (runDir == 0)
        {
            return HeroineState.idling;
        }

        if (actionCommand is ActionCommand.Jump)
        {
            return HeroineState.jumping;
        }
        if (actionCommand is ActionCommand.Sword)
        {
            return HeroineState.swording;
        }
        if (actionCommand is ActionCommand.Shoot)
        {
            return HeroineState.shooting;
        }
        if (actionCommand is ActionCommand.Roll)
        {
            return HeroineState.rolling;
        }
        return null;
    }

    protected override void Awake()
    {
        
    }

    public override void StateFixedUpdate()
    {
        girlHero_.Physics_.HorizontalMove(runDir, moveSpeed);
    }
}