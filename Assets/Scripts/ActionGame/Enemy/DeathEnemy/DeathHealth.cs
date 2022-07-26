using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHealth : Health
{
    public int maxHealth;
    public float invincibleDuration;
    public float hurtPauseDuration = 0.1f;
    public float hurtShakeDuration;
    public float hurtShakeStrength;

    [HideInInspector] public int areaIndex;
    private float weakDuration;
    private int currentHealth;
    private bool invincible = false;
    private Death death;
    private DeathAnimComponent anim;
    private DeathController deathController;
    private Animator hurtAnim;
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
        death = GetComponent<Death>();
        anim = GetComponent<DeathAnimComponent>();
        hurtAnim = anim.hurtAnim;
        deathController = GetComponent<DeathController>();  
        weakDuration = anim.GetClipTime("Death_Weak");
        actionGameCameraController = Camera.main.GetComponent<ActionGameCameraController>();
    }

    public override void TakeDamage(Damage damage)
    {
        if (!invincible)
        {
            invincible = true;
            hurtAnim.SetTrigger("Hurt");
            if (damage.damageType == Damage.DamageType.MeleeAttack)
            {
                TimeControllerManager.instance.timeController.PauseTime(hurtPauseDuration);
                // TimeControllerManager.instance.timeController.StopTime(0, 0.3f, 0);
                actionGameCameraController.CameraShake(hurtShakeDuration, hurtShakeStrength);
                // AttackShake.Instance.HitPause(hitPauseDuration);
                // AttackShake.Instance.CameraShake(hitShakeDuration, hitShakeStrength);
            }        
            if (death.State_ is DeathTeleportState)
            {
                deathController.SetWeak();
                StartCoroutine(IntoWeakness());
            }
            StartCoroutine(IntoInvincibility());
            currentHealth = currentHealth - damage.damageValue;
            // else
            // {
            //     if (death.State_ is DeathWalkingState)
            //     {
            //         deathController.SetRepel(new MoveCommand(damage.damageDir.x, damage.damageDir.y, MoveCommand.MoveType.repel));
            //         StartCoroutine(IntoInvincibility());
            //         currentHealth = currentHealth - damage.damageValue;
            //     } 
            //     else
            //     {
            //         StartCoroutine(IntoInvincibility());
            //         currentHealth = currentHealth - damage.damageValue;
            //     }

            // }
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        AreaManager.instance.DestroyDoor(areaIndex);
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
        yield return new WaitForSecondsRealtime(invincibleDuration);
        // float nextInvincibleTime = Time.time + invincibleDuration;
        // float startTime = Time.time;
        // while (Time.time < nextInvincibleTime)
        // {
        //     //每0.26个无敌周期闪烁一次
        //     if (((int)((Time.time - startTime) / (0.15f * invincibleDuration))) % 2 == 0)
        //     {
        //         anim.SetColor(Color.red);
        //     }
        //     else
        //     {
        //         anim.SetColor(Color.white);
        //     }
        //     yield return null;
        // }
        // anim.SetColor(Color.white);
        invincible = false;
    }
}
