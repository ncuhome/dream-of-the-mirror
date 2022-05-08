using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : MonoBehaviour
{
    protected Death death_;

    protected float stateDuration;
    protected float stateTime;

    protected static DeathIdleState idling;
    protected static DeathWalkingState walking;
    protected static DeathShootingState shooting;
    protected static DeathTeleportState teleporting;
    protected static DeathAttackState attacking;
    protected static DeathWeakState weaking;

    public virtual DeathState HandleCommand(TranslationCommand translationCommand, Command actionCommand) {return null;}

    public virtual bool CanEnter(Death death) {return true;}

    public virtual void Enter(Death death) {death_ = death;}

    public virtual void StateFixedUpdate() {}

    public virtual void StateUpdate() {}

    public virtual void Exit() {}

    protected void GetEndTime(string stateName)
    {
        stateDuration = death_.Anim_.GetClipTime(stateName);
        stateTime = Time.time + stateDuration;
    } 

    protected virtual void Start()
    {
        idling = gameObject.GetComponent<DeathIdleState>();
        walking = gameObject.GetComponent<DeathWalkingState>();
        shooting = gameObject.GetComponent<DeathShootingState>();
        teleporting = gameObject.GetComponent<DeathTeleportState>();
        attacking = gameObject.GetComponent<DeathAttackState>();
        weaking = gameObject.GetComponent<DeathWeakState>();
    }

    public void InitState(ref DeathState state_)
    {
        state_ = idling;
    }
}