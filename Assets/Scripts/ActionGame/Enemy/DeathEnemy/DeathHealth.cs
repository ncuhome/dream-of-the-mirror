using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHealth : Health
{
    public int maxHealth;
    public float invincibleDuration;

    private float weakDuration;
    private int currentHealth;
    private bool invincible = false;
    private Death death;
    private DeathAnimComponent anim;
    private DeathController deathController;

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        death = GetComponent<Death>();
        anim = GetComponent<DeathAnimComponent>();
        deathController = GetComponent<DeathController>();  
        weakDuration = anim.GetClipTime("Death_Weak");
    }

    public override void TakeDamage(Damage damage)
    {
        if (!invincible)
        {
            invincible = true;
            if (death.State_ is DeathTeleportState)
            {
                deathController.SetWeak();
                StartCoroutine(IntoWeakness());
            }
            else
            {
                StartCoroutine(IntoInvincibility());
                currentHealth = currentHealth - damage.damageValue;
            }
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        //TODO: 将死亡事件传入到事件队列中
        Destroy(this.gameObject);
        Debug.Log("DeathEnemy die!");
    }

    public IEnumerator IntoWeakness()
    {
        float nextWeakTime = Time.time + weakDuration;
        float startTime = Time.time;
        while (Time.time < nextWeakTime)
        {
            //每0.26个无敌周期闪烁一次
            if (((int)((Time.time - startTime) / (0.15f * weakDuration))) % 2 == 0)
            {
                anim.SetColor(Color.black);
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
