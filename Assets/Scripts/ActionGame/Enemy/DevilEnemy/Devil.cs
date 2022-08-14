using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil : MonoBehaviour
{
    public string devilName;
    public int areaIndex;
    public Facing facing;
    public ActionGameDialogueControl actionGameDialogueControl;
    private bool isEndDialogue = false;
    private DevilState devilState_;
    private DevilState state_;
    private DevilPhysicsComponent physics_;
    private DevilAnimComponent anim_;
    private DevilHealth health_;
    private DevilController devilController;
    private EnemyAttackConsciousness enemyAttackConsciousness;
    private MoveCommand moveCommand;

    public DevilPhysicsComponent Physics_
    {
        get
        {
            return physics_;
        }
    }

    public DevilAnimComponent Anim_
    {
        get
        {
            return anim_;
        }
    }

    public DevilState State_
    {
        get
        {
            return state_;
        }
    }

    public bool IsEndDialogue
    {
        set{
            isEndDialogue = value;
        }
    }

    void Start()
    {
        health_ = GetComponent<DevilHealth>();
        health_.areaIndex = areaIndex;
        devilController = GetComponent<DevilController>();
        physics_ = GetComponent<DevilPhysicsComponent>();
        anim_ = GetComponent<DevilAnimComponent>();
        enemyAttackConsciousness = GetComponent<EnemyAttackConsciousness>();
        enemyAttackConsciousness.enemyName = name;
        devilState_ = GetComponent<DevilState>();
        devilState_.InitState(ref state_);
        moveCommand = new MoveCommand(0, 0);
        state_.Enter(this, moveCommand);
    }

    void FixedUpdate()
    {
        if (PauseControl.gameIsPaused) return;
        if (!enemyAttackConsciousness.CheckAttackConsciousness()) return;
        if (!isEndDialogue)
        {
            physics_.ResetSpeed();
            return;
        }
        state_.StateFixedUpdate();
    }

    void Update()
    {
        if (PauseControl.gameIsPaused) return;
        if (!enemyAttackConsciousness.CheckAttackConsciousness()) return;
        if (!isEndDialogue)
        {
            TranslationState(DevilState.idling);
            return;
        }
        HandleInput();
        state_.StateUpdate();
    }

    public void HandleInput()
    {
        moveCommand = devilController.HandleTranslationInput();
        ActionCommand actionCommand = devilController.HandleActionInput();
        if (actionCommand == ActionCommand.Weak)
        {
            Debug.Log(state_);
        }
        DevilState state = state_.HandleCommand(moveCommand, actionCommand);
        if (state != null && state.CanEnter(this))
        {
            state_.Exit();
            state_ = state;
            state_.Enter(this, moveCommand);
        }
    }

    public void TranslationState(DevilState state_)
    {
        if (state_.CanEnter(this))
        {   
            this.state_.Exit();
            this.state_ = state_;
            state_.Enter(this, moveCommand);
        }
    }

    public void DestroyDevil()
    {
        isEndDialogue = false;
        actionGameDialogueControl.CurrentReadyDialogue = "devilEnd";
        // Destroy(this.gameObject);
        // Debug.Log("DevilEnemy die!");
    } 
}