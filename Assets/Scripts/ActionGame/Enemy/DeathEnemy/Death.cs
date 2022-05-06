using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    [Header("贴图默认朝向")]
    public Facing facing;
    private DeathState deathState_;
    private DeathState state_;
    private DeathPhysicsComponent physics_;
    private DeathAnimComponent anim_;
    private DeathHealth health_;
    private DeathController deathController;
    private EnemyAttackConsciousness enemyAttackConsciousness;

    public DeathPhysicsComponent Physics_
    {
        get
        {
            return physics_;
        }
    }

    public DeathAnimComponent Anim_
    {
        get
        {
            return anim_;
        }
    }

    public DeathHealth Health_
    {
        get
        {
            return health_;
        }
    }

    public DeathState State_
    {
        get
        {
            return state_;
        }
    }

    void Start()
    {
        health_ = GetComponent<DeathHealth>();
        physics_ = GetComponent<DeathPhysicsComponent>();
        anim_ = GetComponent<DeathAnimComponent>();
        deathController = GetComponent<DeathController>();
        enemyAttackConsciousness = GetComponent<EnemyAttackConsciousness>();
        deathState_ = GetComponent<DeathState>();
        deathState_.InitState(ref state_);
        state_.Enter(this);
    }

    void FixedUpdate()
    {
        if (PauseControl.gameIsPaused) return;
        if (!enemyAttackConsciousness.CheckAttackConsciousness()) return;
        state_.StateFixedUpdate();
    }

    void Update()
    {
        if (PauseControl.gameIsPaused) return;
        if (!enemyAttackConsciousness.CheckAttackConsciousness()) return;
        HandleInput();
        state_.StateUpdate();
    }

    public void HandleInput()
    {
        TranslationCommand translationCommand = deathController.HandleTranslationInput();
        Command actionCommand = deathController.HandleActionInput();
        DeathState state = state_.HandleCommand(translationCommand, actionCommand);
        if (state != null && state.CanEnter(this))
        {
            state_ = state;
            state_.Enter(this);
        }
    }

    public void TranslationState(DeathState state_)
    {
        this.state_ = state_;
        state_.Enter(this);
    }
}