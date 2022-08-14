using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : MonoBehaviour
{
    protected Death death_;

    protected float stateDuration;
    protected float stateTime;

    public static DeathIdleState idling;
    protected static DeathWalkingState walking;
    protected static DeathShootingState shooting;
    protected static DeathTeleportState teleporting;
    protected static DeathAttackState attacking;
    protected static DeathWeakState weaking;
    protected static DeathRepelState repelling;

    public virtual DeathState HandleCommand(MoveCommand moveCommand, ActionCommand actionCommand) {return null;}

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

    protected virtual void Awake()
    {
        idling = GetComponent<DeathIdleState>();
        walking = GetComponent<DeathWalkingState>();
        shooting = GetComponent<DeathShootingState>();
        teleporting = GetComponent<DeathTeleportState>();
        attacking = GetComponent<DeathAttackState>();
        weaking = GetComponent<DeathWeakState>();
        repelling = GetComponent<DeathRepelState>();
    }

    public void InitState(ref DeathState state_)
    {
        state_ = idling;
    }
}