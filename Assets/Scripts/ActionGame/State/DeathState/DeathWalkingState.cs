using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWalkingState : DeathState
{
    public float walkSpeed;

    private int walkDir;

    public override void Enter(Death death_)
    {
        base.Enter(death_);
        death_.Anim_.Animator_.SetTrigger("Walk");
    }

    public override DeathState HandleCommand(TranslationCommand translationCommand, Command actionCommand)
    {
        walkDir = (int)translationCommand.Horizontal;
        if (actionCommand is SwordCommand)
        {
            return attacking;
        }
        if (actionCommand is ShootCommand)
        {
            return shooting;
        }
        return null;
    }

    protected override void Start()
    {
        
    }

    public override void StateFixedUpdate()
    {
        death_.Physics_.HorizontalMove(walkDir, walkSpeed);
    }
}