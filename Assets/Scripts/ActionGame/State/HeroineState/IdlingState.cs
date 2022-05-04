using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroineStateNameSpace
{
    public class IdlingState : HeroineState
    {
        private int horizontal = 0;
        private int vertical = 0;

        public override void Enter(GirlHero girlHero)
        {
            base.Enter(girlHero);
            girlHero.Anim_.Animator_.SetTrigger("Idle");
        }

        public override HeroineState HandleCommand(GirlHero girlHero, TranslationCommand translationCommand, Command buttonCommand)
        {
            if (translationCommand is RepelCommand)
            {
                return HeroineState.repelling;
            }
            
            if (!girlHero.Physics_.IsGrounded)
            {
                return HeroineState.floating;
            }
            InitState(translationCommand);
            if (horizontal != 0)
            {
                return HeroineState.running;
            }
            // Debug.Log(buttonCommand);
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
            // Debug.Log(horizontal);
            girlHero_.Physics_.HorizontalMove(0, 0);
        }

        private void InitState(TranslationCommand translationCommand)
        {
            horizontal = (int)translationCommand.Horizontal;
            vertical = (int)translationCommand.Vertical;
        }
    }
}