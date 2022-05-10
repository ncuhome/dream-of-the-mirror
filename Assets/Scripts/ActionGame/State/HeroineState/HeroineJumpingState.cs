using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroineJumpingState : HeroineState
{
    public float jumpMoveSpeed;
    public float shortJumpForce;
    public float longJumpForce;
    public int maxJumpCount = 2;
    public int remainJumpCount = 0;
    public AudioSource jumpAudio;
    
    private int jumpDir;
    private bool hasJump = false;

    //土狼时间计数器
    // public int graceJumpBuffer = 10;
    // public int graceTimer;

    //跳跃缓冲计时器
    // public int jumpBuffer = 10;
    // public int jumpBufferTimer;

    public int RemainJumpCount
    {
        set
        {
            remainJumpCount = value;
        }
    }

    protected override void Awake()
    {
        remainJumpCount = maxJumpCount;    
    }

    public override bool CanEnter(GirlHero girlHero)
    {
        return CanJump(girlHero);
    }

    public override void Enter(GirlHero girlHero)
    {
        base.Enter(girlHero);
        GetEndTime("GirlHero_Jump");
        girlHero.Anim_.Animator_.SetTrigger("Jump");
        girlHero.Particle_.CreateDust();
        girlHero.PlayAudio(jumpAudio);
        hasJump = false;
        // girlHero.Physics_.ResetSpeed();
    }

    public override HeroineState HandleCommand(GirlHero girlHero, TranslationCommand translationCommand, Command buttonCommand)
    {
        if (translationCommand is RepelCommand)
        {
            return HeroineState.repelling;
        }
        jumpDir = (int)translationCommand.Horizontal;

        if (buttonCommand is JumpCommand)
        {
            return HeroineState.jumping;
        }
        if (buttonCommand is SwordCommand)
        {
            return HeroineState.swording;
        }
        if (buttonCommand is ShootCommand)
        {
            return HeroineState.shooting;
        }
        if (buttonCommand is RollCommand)
        {
            return HeroineState.rolling;
        }
        return null;
    }

    public override void StateFixedUpdate()
    {
        girlHero_.Physics_.HorizontalMove(jumpDir, jumpMoveSpeed);
    }

    public override void StateUpdate()
    {
        TryApplyingForce();
        if (Time.time > stateTime)
        {
            girlHero_.TranslationState(floating);
        }
    }

    private void TryApplyingForce()
    {
        if (!hasJump)
        {
            float percentage = 0;
            if (InputHandlerManager.instance.inputHandler.jumpBtn.GetPressTimePercentage(ref percentage))
            {
                girlHero_.Physics_.ResetSpeed();
                if (percentage < 0.3f)
                {
                    girlHero_.Physics_.AddJumpForce(shortJumpForce);
                }
                else
                {
                    girlHero_.Physics_.AddJumpForce(Mathf.Sqrt(shortJumpForce*shortJumpForce + (longJumpForce*longJumpForce - shortJumpForce*shortJumpForce) * percentage));
                }
                hasJump = true;
            }
            //两种形态
            // if (InputHandlerManager.instance.inputHandler.jumpBtn.IsShortPress())
            // {
            //     // Debug.Log("cnsm");
            //     girlHero_.Physics_.Rb.velocity = Vector2.zero;
            //     girlHero_.Physics_.AddJumpForce(shortJumpForce);
            //     hasJump = true;
            // }
            // if (InputHandlerManager.instance.inputHandler.jumpBtn.IsLongPress())
            // {
            //     girlHero_.Physics_.Rb.velocity = Vector2.zero;
            //     girlHero_.Physics_.AddJumpForce(longJumpForce);
            //     hasJump = true;
            // }
        }
    }

    private bool CanJump(GirlHero girlHero)
    {
        if (girlHero.Physics_.IsGrounded)
        {
            remainJumpCount = maxJumpCount;
        }
        if (remainJumpCount <= 0)
        {
            return false;
        }
        remainJumpCount--;
        return true;
    }
}