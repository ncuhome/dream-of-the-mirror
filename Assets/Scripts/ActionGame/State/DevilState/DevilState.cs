using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilState : MonoBehaviour
{
    protected Devil devil_;

    protected float stateDuration;
    protected float stateTime;

    protected static DevilIdleState idling;
    protected static DevilRunState running;
    protected static DevilShootState shooting;
    protected static DevilSprintState sprinting;
    protected static DevilClaw1State clawing1;
    protected static DevilClaw2State clawing2;
    protected static DevilClaw3State clawing3;
    protected static DevilClaw4State clawing4;
    protected static DevilDownwardAttackState downwardAttack;
    protected static DevilUppercutState uppercut;

    public virtual DevilState HandleCommand(MoveCommand moveCommand, ActionCommand actionCommand) {return null;}

    public virtual bool CanEnter(Devil devil) {return true;}

    public virtual void Enter(Devil devil, MoveCommand moveCommand) {devil_ = devil;}

    public virtual void StateFixedUpdate() {}

    public virtual void StateUpdate() {}

    public virtual void Exit() {}

    protected void GetEndTime(string stateName)
    {
        stateDuration = devil_.Anim_.GetClipTime(stateName);
        stateTime = Time.time + stateDuration;
    } 

    protected virtual void Awake()
    {
        idling = GetComponent<DevilIdleState>();
        running = GetComponent<DevilRunState>();
        shooting = GetComponent<DevilShootState>();
        sprinting = GetComponent<DevilSprintState>();
        clawing1 = GetComponent<DevilClaw1State>();
        clawing2 = GetComponent<DevilClaw2State>();
        clawing3 = GetComponent<DevilClaw3State>();
        clawing4 = GetComponent<DevilClaw4State>();
        downwardAttack = GetComponent<DevilDownwardAttackState>();
        uppercut = GetComponent<DevilUppercutState>();
    }

    public void InitState(ref DevilState state_)
    {
        state_ = idling;
    }
}
