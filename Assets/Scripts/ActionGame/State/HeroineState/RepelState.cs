using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroineStateNameSpace
{
    public class RepelState : HeroineState
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
            // girlHero.Health_.SetInvincible(true);
            girlHero.Anim_.Animator_.SetTrigger("Repel");
            // girlHero.Particle_.CreateDust();
            // girlHero.PlayAudio(repelAudio);
            hasRepel = false;
        }

        public override void Exit()
        {
            // girlHero_.Health_.SetInvincible(false);
        }

        // public override HeroineState HandleCommand(GirlHero girlHero, TranslationCommand translationCommand, Command buttonCommand)
        // {
        //     if (translationCommand is RepelCommand)
        //     {
        //         repelDir.x = repelDir.x * translationCommand.Horizontal * (-1);
        //         Debug.Log(repelDir.x + "xxx");
        //     }
        //     return null;
        // }

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
}