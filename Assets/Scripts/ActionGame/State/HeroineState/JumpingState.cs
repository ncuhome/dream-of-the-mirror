using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroineStateNameSpace
{
    public class JumpingState : HeroineState
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

        protected override void Start()
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
            // girlHero.Physics_.UseNoFriction();
            girlHero.Particle_.CreateDust();
            girlHero.PlayAudio(jumpAudio);
            hasJump = false;
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
                    girlHero_.Physics_.Rb.velocity = Vector2.zero;
                    if (percentage < 0.3f)
                    {
                        // Debug.Log(shortJumpForce);
                        girlHero_.Physics_.AddJumpForce(shortJumpForce);
                        // Debug.Log(girlHero_.Physics_.Rb.velocity + "xxx");
                    }
                    else
                    {
                        girlHero_.Physics_.AddJumpForce(Mathf.Sqrt(shortJumpForce*shortJumpForce + (longJumpForce*longJumpForce - shortJumpForce*shortJumpForce) * percentage));
                        // Debug.Log(Mathf.Sqrt(shortJumpForce*shortJumpForce + (longJumpForce*longJumpForce - shortJumpForce*shortJumpForce) * percentage));
                        // Debug.Log(girlHero_.Physics_.Rb.velocity + "xxx");
                    }
                    hasJump = true;
                }
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
            // if (InputHandlerManager.instance.inputHandler.jumpBtn.CanBeJudgeLongPress())
            // {
            //     if (InputHandlerManager.instance.inputHandler.jumpBtn.IsLongPress())
            //     {
            //         // Debug.Log(longJumpForce);
            //         girlHero_.Physics_.AddLongJumpForce(longJumpForce);
            //     }
            //     else
            //     {
            //         // Debug.Log(shortJumpForce);
            //         girlHero_.Physics_.AddShortJumpForce(shortJumpForce);
            //     }
            // }
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
            // Debug.Log("wc");
            remainJumpCount--;
            return true;
        }
    }
}