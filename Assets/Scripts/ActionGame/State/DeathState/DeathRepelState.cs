using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRepelState : DeathState
{
    public Vector2 repelDir;
    public float repelForce;
    public AudioSource repelAudio;
    private DeathController deathController;
    private bool hasRepel = false;
    public override void Enter(Death death_)
    {
        base.Enter(death_);
        death_.Anim_.Animator_.SetTrigger("Repel");
        GetEndTime("Death_Repel");
        InitRepelDir();
        hasRepel = false;
        death_.Physics_.ResetSpeed();
    }

    public void Start()
    {
        deathController = GetComponent<DeathController>();
    }

    public override void StateUpdate()
    {
        if (Time.time > stateTime)
        {
            death_.TranslationState(walking);
        }
    }

    protected override void Awake()
    {
    }

    public override void StateFixedUpdate()
    {
        if (!hasRepel)
        {
            death_.Physics_.RepelMove(repelDir, repelForce);
            hasRepel = true;
        }
    }
    private void InitRepelDir()
    {
        if (deathController.Repel.horizontal > 0)
        {
            repelDir.x = Mathf.Abs(repelDir.x);
        }
        else
        {
            repelDir.x = Mathf.Abs(repelDir.x) * (-1);
        }
        deathController.DestroyRepel();
    }
}
