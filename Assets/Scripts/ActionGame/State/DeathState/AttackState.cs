using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeathStateNameSpace
{
    public class AttackState : DeathState
    {
        public int attackDamage = 1;
        public float attackRadiu = 1f;
        public float attackCd;
        public Vector2 attackOffset;
        public LayerMask attackMask;
        public AudioSource attackAudio;

        private float lastAttackTime;
        // private int attackDir;

        public override bool CanEnter(Death death)
        {
            if (Time.time < lastAttackTime + attackCd)
            {
                return false;
            }
            return true;
        }

        public override void Enter(Death death_)
        {
            base.Enter(death_);
            GetEndTime("Death_Attack");
            death_.Anim_.Animator_.SetTrigger("Attack");
            // death_.PlayAudio(attackAudio);
        }

        public override void Exit()
        {
            lastAttackTime = Time.time;
        }

        public override DeathState HandleCommand(TranslationCommand translationCommand, Command actionCommand)
        {
            // if (translationCommand is RepelCommand)
            // {
            //     Exit();
            //     return DeathState.repelling;
            // }
            // attackDir = (int)translationCommand.Horizontal;
            
            return null;
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
                death_.TranslationState(idling);
            }
        }

        public void Attack()
        {
            // Debug.Log(attackMask);
            death_.Physics_.SwordAttack(attackOffset, attackRadiu, attackMask, attackDamage);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere((Vector2)transform.position + attackOffset, attackRadiu); 
        }
    }
}