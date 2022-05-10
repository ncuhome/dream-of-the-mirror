using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTeleportState : DeathState
{
    public float teleportSpeed = 20f;

    private int teleportDir;

    public override void Enter(Death death_)
    {
        base.Enter(death_);
        GetEndTime("Death_Teleport");
        death_.Anim_.Animator_.SetTrigger("Teleport");
    }

    public override DeathState HandleCommand(TranslationCommand translationCommand, Command actionCommand)
    {
        if (actionCommand is WeakCommand)
        {
            return DeathState.weaking;
        } 
        teleportDir = (int)translationCommand.Horizontal;

        if (actionCommand is SwordCommand)
        {
            return DeathState.attacking;
        }

        return null;
    }

    protected override void Awake()
    {
        
    }

    public override void StateFixedUpdate()
    {
        death_.Physics_.HorizontalMove(teleportDir, teleportSpeed);
    }
}
