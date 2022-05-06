using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlHeroHealth : Health
{
    public int maxHealth;
    public float invincibleDuration;

    private int currentHealth;
    private bool invincible = false;
    private GirlHeroParticleComponent particle;
    private GirlHeroAnimComponent anim;

    void Start()
    {
        currentHealth = maxHealth;    
        particle = GetComponent<GirlHeroParticleComponent>();
        anim = GetComponent<GirlHeroAnimComponent>();
    }

    public override void TakeDamage(Damage damage)
    {
        if (!invincible)
        {
            particle.CreateSpark(damage.damagePos);
            TimeControllerManager.instance.timeController.StopTime(0.05f, 10, 0.1f);
            InputHandlerManager.instance.inputHandler.SetRepel(new RepelCommand(damage.damageDir.x, damage.damageDir.y));
            StartCoroutine(IntoInvincibility());
            currentHealth = currentHealth - damage.damageValue;
            invincible = true;
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        Debug.Log("GirlHero die!");
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
            //每0.26个无敌周期闪烁一次
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
