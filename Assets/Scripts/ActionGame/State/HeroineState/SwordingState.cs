using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroineStateNameSpace
{
    public class SwordingState : HeroineState
    {
        public float attackMoveSpeed;
        public int attackDamage = 4;
        public float attackRadiu = 1f;
        public float attackCd;
        public int attackCount;
        public Vector2 attackOffset;
        public LayerMask attackMask;
        public AudioSource attackAudio;

        private float lastAttackTime;
        private int attackDir;
        private float attackTime;
        private bool readyToRoll = false;

        public override bool CanEnter(GirlHero girlHero)
        {
            if (Time.time < lastAttackTime + attackCd)
            {
                return false;
            }
            return true;
        }

        public override void Enter(GirlHero girlHero)
        {
            base.Enter(girlHero);
            GetEndTime("GirlHero_Sword");
            attackTime = Time.time + stateDuration / attackCount;
            girlHero.Anim_.Animator_.SetTrigger("SwordAttack");
            girlHero.PlayAudio(attackAudio);
        }

        public override void Exit()
        {
            lastAttackTime = Time.time;
        }

        public override HeroineState HandleCommand(GirlHero girlHero, TranslationCommand translationCommand, Command buttonCommand)
        {
            if (translationCommand is RepelCommand)
            {
                Exit();
                return HeroineState.repelling;
            }
            attackDir = (int)translationCommand.Horizontal;
            // Debug.Log(translationCommand.Horizontal);

            if (buttonCommand is RollCommand)
            {
                readyToRoll = true;
            }
            return null;
        }

        protected override void Start()
        {
            
        }

        public override void StateFixedUpdate()
        {
            girlHero_.Physics_.SwordAttackMove(attackDir, attackMoveSpeed);
        }

        public override void StateUpdate()
        {
            if (Time.time > attackTime)
            {
                Attack();
                attackTime += stateTime / attackCount;
            }
            if (Time.time > stateTime)
            {
                if (readyToRoll)
                {
                    Exit();
                    girlHero_.TranslationState(rolling);
                }

                if (girlHero_.Physics_.IsGrounded)
                {
                    Exit();
                    girlHero_.TranslationState(idling);
                }
                else
                {
                    Exit();
                    girlHero_.TranslationState(floating);
                }
            }
        }

        public void Attack()
        {
            girlHero_.Physics_.SwordAttack(attackOffset, attackRadiu, attackMask, attackDamage);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere((Vector2)transform.position + attackOffset, attackRadiu); 
        }
    }
}