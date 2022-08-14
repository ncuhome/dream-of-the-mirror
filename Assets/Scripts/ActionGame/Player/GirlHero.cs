using UnityEngine;

public class GirlHero : MonoBehaviour
{
    [Header("贴图默认朝向")]
    public Facing facing;
    
    public ActionGameDialogueControl actionGameDialogueControl;
    public ActionGameStoreControl actionGameStoreControl;

    private HeroineState heroineState_;
    private HeroineState state_;
    private InputHandler inputHandler_;
    private AreaManager areaManager_;
    private GirlHeroPhysicsComponent physics_;
    private GirlHeroParticleComponent particle_;
    private GirlHeroAnimComponent anim_;
    private GirlHeroHealth health_;
    private bool canMove = true;

    public GirlHeroPhysicsComponent Physics_
    {
        get
        {
            return physics_;
        }
    }

    public GirlHeroParticleComponent Particle_
    {
        get
        {
            return particle_;
        }
    }

    public GirlHeroAnimComponent Anim_
    {
        get
        {
            return anim_;
        }
    }

    public bool CanMove
    {
        set
        {
            canMove = value;
        }
    } 

    void Start()
    {
        health_ = GetComponent<GirlHeroHealth>();
        inputHandler_ = InputHandlerManager.instance.inputHandler;
        areaManager_ = AreaManager.instance;
        physics_ = GetComponent<GirlHeroPhysicsComponent>();
        particle_ = GetComponent<GirlHeroParticleComponent>();
        anim_ = GetComponent<GirlHeroAnimComponent>();
        heroineState_ = GetComponent<HeroineState>();
        heroineState_.InitState(ref state_);
        state_.Enter(this); 
    }

    void FixedUpdate()
    {
        if (PauseControl.gameIsPaused || !canMove)
        {
            physics_.ResetSpeed();
            return;
        }
        physics_.PhysicsFixedUpdate();
        state_.StateFixedUpdate();
    }

    void Update()
    {  
        if (PauseControl.gameIsPaused) return;
        if (!canMove)
        {
            TranslationState(HeroineState.idling);
            return;
        }
        HandleInput();
        state_.StateUpdate();
        areaManager_.CalAreaIndex(transform.position);
    }

    public void HandleInput()
    {
        MoveCommand translationCommand = inputHandler_.HandleJoyStickInput();
        ActionCommand buttonCommand = inputHandler_.HandleButtonInput();
        HeroineState state = state_.HandleCommand(this, translationCommand, buttonCommand);
        if (state != null && state.CanEnter(this))
        {
            state_.Exit();
            state_ = state;
            state_.Enter(this);
        }
    }

    public void TranslationState(HeroineState state_)
    {
        if (state_.CanEnter(this))
        {
            this.state_.Exit();
            this.state_ = state_;
            state_.Enter(this);
        }
    }    

    public void PlayAudio(AudioSource t)
    {
        if (t != null && !t.isPlaying)
        {
            t.Play();
        }
    }

    public void StopAudio(AudioSource t)
    {
        if (t != null && t.isPlaying)
        {
            t.Stop();
        }
    }

    public void ResetAnim()
    {
        anim_.animator_.ResetTrigger("Idle");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Store")    
        {
            if (other.gameObject.GetComponent<SavePoint>().storeName == "store7")
            {
                if (!actionGameDialogueControl.CanEndStoreDialogueShow)
                {
                    return;
                }
            }
            other.gameObject.GetComponent<Mark>().ConnectPlayer(true);
            actionGameDialogueControl.CurrentSavePoint = other.gameObject.GetComponent<SavePoint>();
            actionGameDialogueControl.CurrentReadyDialogue = other.gameObject.GetComponent<SavePoint>().storeName;
            actionGameStoreControl.BeginStore(other.gameObject.GetComponent<SavePoint>().storeName);
            canMove = false;
        }
        if (other.gameObject.tag == "EnemyTrigger")    
        {
            actionGameDialogueControl.CurrentEnemyTrigger = other.gameObject.GetComponent<EnemyTrigger>();
            actionGameDialogueControl.CurrentReadyDialogue = other.gameObject.GetComponent<EnemyTrigger>().triggerName;
            canMove = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Store")    
        {
            other.gameObject.GetComponent<Mark>().ConnectPlayer(false);
            actionGameDialogueControl.CurrentSavePoint = null;
            actionGameDialogueControl.CurrentReadyDialogue = "";
            actionGameDialogueControl.HasShowDialogue = false;
        }
        if (other.gameObject.tag == "EnemyTrigger")    
        {
            actionGameDialogueControl.CurrentEnemyTrigger = null;
            actionGameDialogueControl.CurrentReadyDialogue = "";
            actionGameDialogueControl.HasShowDialogue = false;
            Destroy(other.gameObject);
        }
    }
}