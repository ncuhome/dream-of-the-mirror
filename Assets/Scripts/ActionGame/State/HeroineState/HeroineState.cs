using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroineStateNameSpace;

public class HeroineState : MonoBehaviour
{
    protected GirlHero girlHero_;

    protected float stateDuration;
    protected float stateTime;

    public static IdlingState idling;
    public static RunningState running;
    public static JumpingState jumping;
    public static RollingState rolling;
    public static FloatingState floating;
    public static SwordingState swording;
    public static ShootingState shooting;
    public static LandingState landing;
    public static RepelState repelling;

    public virtual HeroineState HandleCommand(GirlHero girlHero, TranslationCommand translationCommand, Command buttonCommand) {return null;}

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

    protected virtual void Start()
    {
        idling = gameObject.GetComponent<IdlingState>();
        running = gameObject.GetComponent<RunningState>();
        jumping = gameObject.GetComponent<JumpingState>();
        rolling = gameObject.GetComponent<RollingState>();
        floating = gameObject.GetComponent<FloatingState>();
        swording = gameObject.GetComponent<SwordingState>();
        shooting = gameObject.GetComponent<ShootingState>();
        landing = gameObject.GetComponent<LandingState>();
        repelling = gameObject.GetComponent<RepelState>();
    }
}
