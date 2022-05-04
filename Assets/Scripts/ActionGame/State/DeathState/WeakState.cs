using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeathStateNameSpace
{
    public class WeakState : DeathState
    {
        public override void Enter(Death death)
        {
            base.Enter(death);
            GetEndTime("Death_Weak");
            // death.Health_.SetInvincible(true);
            death.Anim_.Animator_.SetTrigger("Weak");
        }

        public override void Exit()
        {
            // death_.Health_.SetInvincible(false);
        }

        protected override void Start()
        {
            
        }

        public override void StateFixedUpdate()
        {
            death_.Physics_.HorizontalMove(0, 0);
        }

        public override void StateUpdate()
        {
            if (Time.time > stateTime)
            {
                Exit();
                death_.TranslationState(walking);
            }
        }
    }
}
