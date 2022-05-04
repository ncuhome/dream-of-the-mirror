using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroineStateNameSpace
{
    public class RollingState : HeroineState
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
            // girlHero.Health_.SetInvincible(true);
            girlHero.Anim_.Animator_.SetTrigger("Roll");
            // girlHero.Physics_.UseNoFriction();
            girlHero.Particle_.CreateDust();
            girlHero.PlayAudio(rollAudio);
        }

        public override void Exit()
        {
            lastRollTime = Time.time;
            girlHero_.Physics_.Rb.velocity = Vector2.zero;
            // girlHero_.Health_.SetInvincible(false);
            girlHero_.Physics_.UseDefaultFriction();
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
            remainRollCount = maxRollCount;
        }

        public override void StateFixedUpdate()
        {
            girlHero_.Physics_.RollMove(rollSpeed);
            // PhysicsFixedUpdate();
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

        private bool CanRoll(GirlHero girlHero)
        {
            if (Time.time < lastRollTime + rollCd)
            {
                return false;
            }

            // if (girlHero.Physics_.IsGrounded)
            // {
            //     remainRollCount = maxRollCount;
            // }
            if (remainRollCount <= 0)
            {
                return false;
            }
            remainRollCount--;
            return true;
        }
    }
}