using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilHealth : Health
{
    public int maxHealth;
    public float invincibleDuration;
    public float hurtPauseDuration = 0.1f;
    public float hurtShakeDuration;
    public float hurtShakeStrength;

    [HideInInspector] public int areaIndex;
    private int currentHealth;
    private bool invincible = false;
    private Devil devil;
    public DevilAnimComponent anim;
    private DevilController devilController;
    public Animator hurtAnim;
    private ActionGameCameraController actionGameCameraController;

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
        devil = GetComponent<Devil>();
        areaIndex = devil.areaIndex;
        anim = GetComponent<DevilAnimComponent>();
        hurtAnim = anim.hurtAnim;
        devilController = GetComponent<DevilController>();  
        actionGameCameraController = Camera.main.GetComponent<ActionGameCameraController>();
    }

    public override void TakeDamage(Damage damage)
    {
        // Debug.Log(hurtAnim);
        if (!invincible)
        {
            invincible = true;
            hurtAnim.SetTrigger("Hurt");
            if (damage.damageType == Damage.DamageType.MeleeAttack)
            {
                TimeControllerManager.instance.timeController.PauseTime(hurtPauseDuration);
                // TimeControllerManager.instance.timeController.StopTime(0, 1/Time.deltaTime, hurtPauseDuration);
                actionGameCameraController.CameraShake(hurtShakeDuration, hurtShakeStrength);
            }
            StartCoroutine(IntoWeakness());
            currentHealth = currentHealth - damage.damageValue;
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        //TODO: 将死亡事件传入到事件队列中
        AreaManager.instance.DestroyDoor(areaIndex);
        devil.DestroyDevil();
    }

    IEnumerator IntoInvincibility()
    {
        yield return new WaitForSecondsRealtime(invincibleDuration);
        invincible = false;
    }

    public IEnumerator IntoWeakness()
    {
        float nextWeakTime = Time.time + invincibleDuration;
        float startTime = Time.time;
        while (Time.time < nextWeakTime)
        {
            //每0.26个无敌周期闪烁一次
            if (((int)((Time.time - startTime) / (0.15f * invincibleDuration))) % 2 == 0)
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
}
