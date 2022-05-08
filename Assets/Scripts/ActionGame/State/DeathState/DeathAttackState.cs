using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAttackState : DeathState
{
    public int attackDamage = 1;
    public float attackRadiu = 1f;
    public float attackCd;
    public float attackBeginDuration;
    public int attackCount;
    public Vector2 attackOffset;
    public LayerMask attackMask;
    public AudioSource attackAudio;

    private float lastAttackTime;
    private float attackTime;

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
        attackTime = Time.time + attackBeginDuration + stateDuration / attackCount;
        GetEndTime("Death_Attack");
        death_.Anim_.Animator_.SetTrigger("Attack");
    }

    public override void Exit()
    {
        lastAttackTime = Time.time;
    }

    public override DeathState HandleCommand(TranslationCommand translationCommand, Command actionCommand)
    {            
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
        if (Time.time > attackTime)
        {
            Attack();
            attackTime += stateTime / attackCount;
        }
        if (Time.time > stateTime)
        {
            Exit();
            death_.TranslationState(idling);
        }
    }

    public void Attack()
    {
        death_.Physics_.SwordAttack(attackOffset, attackRadiu, attackMask, attackDamage);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + attackOffset, attackRadiu); 
    }
}