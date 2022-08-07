using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlHeroHealth : Health
{
    public int maxHealth;
    public float invincibleDuration;
    public float hurtPauseDuration = 0.1f;
    public float hurtShakeDuration = 0.1f;
    public float hurtShakeStrength = 0.05f;

    private int currentHealth;
    private bool invincible = false;
    private GirlHeroAnimComponent anim;
    private GirlHeroParticleComponent particle;
    private ActionGameCameraController actionGameCameraController;
    public ActionGameDialogueControl actionGameDialogueControl;
    private bool hasDie = false;

    void Start()
    {
        currentHealth = maxHealth;    
        particle = GetComponent<GirlHeroParticleComponent>();
        anim = GetComponent<GirlHeroAnimComponent>();
        actionGameCameraController = Camera.main.GetComponent<ActionGameCameraController>();
    }

    public override void TakeDamage(Damage damage)
    {
        if (!invincible)
        {
            // TimeControllerManager.instance.timeController.PauseTime(hurtPauseDuration);
            TimeControllerManager.instance.timeController.StopTime(0, 1/hurtPauseDuration, 0);
            actionGameCameraController.CameraShake(hurtShakeDuration, hurtShakeStrength);
            particle.CreateSpark(damage.damagePos);
            InputHandlerManager.instance.inputHandler.SetRepel(new MoveCommand(damage.damageDir.x, damage.damageDir.y, MoveCommand.MoveType.repel));
            StartCoroutine(IntoInvincibility());
            currentHealth = currentHealth - damage.damageValue;
            invincible = true;
        }
        if (currentHealth <= 0)
        {
            if (!hasDie)
            {
                Die();
            }
        }
    }

    public override void Die()
    {
        hasDie = true;
        StartCoroutine(actionGameDialogueControl.ReLoadScene());
    }

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
    }

    IEnumerator IntoInvincibility()
    {
        float nextInvincibleTime = Time.time + invincibleDuration;
        float startTime = Time.time;
        while (Time.time < nextInvincibleTime)
        {
            //每0.15个无敌周期闪烁一次
            if (((int)((Time.time - startTime) / (0.15f * invincibleDuration))) % 2 == 0)
            {
                anim.SetColor(Color.red);
            }
            else
            {
                anim.SetColor(Color.white);
            }
            yield return null;
        }
        anim.SetColor(Color.white);
        invincible = false;
    }
}
