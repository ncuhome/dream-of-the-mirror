using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroineStateNameSpace
{
    public class FloatingState : HeroineState
    {
        public float moveSpeed;
        private int horizontal = 0;
        private int vertical = 0;

        public override void Enter(GirlHero girlHero)
        {
            base.Enter(girlHero);
            // girlHero.Physics_.UseNoFriction();
            // girlHero.CanSpawnLandDust(true);
            girlHero.Anim_.Animator_.SetTrigger("Float");
            // girlHero.Anim_.Animator_.SetBool("Grounded", false);
        }

        public override HeroineState HandleCommand(GirlHero girlHero, TranslationCommand translationCommand, Command buttonCommand)
        {
            if (translationCommand is RepelCommand)
            {
                return HeroineState.repelling;
            }

            InitState(translationCommand);
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

        protected override void Start()
        {
            
        }

        public override void StateFixedUpdate()
        {
            girlHero_.Physics_.HorizontalMove(horizontal, moveSpeed);
        }

        public override void StateUpdate()
        {
            if (girlHero_.Physics_.IsGrounded)
            {
                girlHero_.TranslationState(HeroineState.landing);
                return;
            }
        }

        private void InitState(TranslationCommand translationCommand)
        {
            horizontal = (int)translationCommand.Horizontal;
            vertical = (int)translationCommand.Vertical;
        }
    }
}
