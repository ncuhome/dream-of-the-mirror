using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroineState : MonoBehaviour
{
    protected GirlHero girlHero_;

    protected float stateDuration;
    protected float stateTime;

    public static HeroineIdlingState idling;
    protected static HeroineRunningState running;
    protected static HeroineJumpingState jumping;
    protected static HeroineRollingState rolling;
    protected static HeroineFloatingState floating;
    protected static HeroineSwordingState swording;
    protected static HeroineShootingState shooting;
    protected static HeroineLandingState landing;
    protected static HeroineRepelState repelling;

    public virtual HeroineState HandleCommand(GirlHero girlHero, MoveCommand moveCommand, ActionCommand actionCommand) {return null;}

    public virtual bool CanEnter(GirlHero girlHero) {return true;}

    public virtual void Enter(GirlHero girlHero) {girlHero_ = girlHero;}

    public virtual void StateFixedUpdate() {}

    public virtual void StateUpdate() {}

    public virtual void Exit() {}

    protected void GetEndTime(string stateName)
    {
        stateDuration = girlHero_.Anim_.GetClipTime(stateName);
        stateTime = Time.time + stateDuration;
    } 

    protected virtual void Awake()
    {
        idling = GetComponent<HeroineIdlingState>();
        running = GetComponent<HeroineRunningState>();
        jumping = GetComponent<HeroineJumpingState>();
        rolling = GetComponent<HeroineRollingState>();
        floating = GetComponent<HeroineFloatingState>();
        swording = GetComponent<HeroineSwordingState>();
        shooting = GetComponent<HeroineShootingState>();
        landing = GetComponent<HeroineLandingState>();
        repelling = GetComponent<HeroineRepelState>();
    }

    public void InitState(ref HeroineState state_)
    {
        state_ = idling;
    }

    public void InitRollCount()
    {
        rolling.InitCount();
    }
}
