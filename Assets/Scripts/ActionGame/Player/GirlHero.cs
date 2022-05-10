using UnityEngine;

public class GirlHero : MonoBehaviour
{
    [Header("贴图默认朝向")]
    public Facing facing;

    private HeroineState heroineState_;
    private HeroineState state_;
    private InputHandler inputHandler_;
    private GirlHeroPhysicsComponent physics_;
    private GirlHeroParticleComponent particle_;
    private GirlHeroAnimComponent anim_;
    private GirlHeroHealth health_;

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

    void Start()
    {
        health_ = GetComponent<GirlHeroHealth>();
        inputHandler_ = InputHandlerManager.instance.inputHandler;
        physics_ = GetComponent<GirlHeroPhysicsComponent>();
        particle_ = GetComponent<GirlHeroParticleComponent>();
        anim_ = GetComponent<GirlHeroAnimComponent>();
        heroineState_ = GetComponent<HeroineState>();
        heroineState_.InitState(ref state_);
        state_.Enter(this); 
    }

    void FixedUpdate()
    {
        if (PauseControl.gameIsPaused) return;
        physics_.PhysicsFixedUpdate();
        state_.StateFixedUpdate();
    }

    void Update()
    {
        if (PauseControl.gameIsPaused) return;
        HandleInput();
        state_.StateUpdate();
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
}