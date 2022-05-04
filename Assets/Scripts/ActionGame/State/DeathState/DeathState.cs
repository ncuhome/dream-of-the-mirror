using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeathStateNameSpace;

public class DeathState : MonoBehaviour
{
    protected Death death_;

    protected float stateDuration;
    protected float stateTime;

    public static IdleState idling;
    public static WalkingState walking;
    public static ShootingState shooting;
    public static TeleportState teleporting;
    public static AttackState attacking;
    public static WeakState weaking;

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
        idling = gameObject.GetComponent<IdleState>();
        walking = gameObject.GetComponent<WalkingState>();
        shooting = gameObject.GetComponent<ShootingState>();
        teleporting = gameObject.GetComponent<TeleportState>();
        attacking = gameObject.GetComponent<AttackState>();
        weaking = gameObject.GetComponent<WeakState>();
    }
}