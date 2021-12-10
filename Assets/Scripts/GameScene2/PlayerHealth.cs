using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public string playerTag;
    public int maxHealth;
    public int currentHealth;
    public float invincibleDuration = 0.5f;
    public bool invincible = false;
    public SpriteRenderer sRend;

    private float nextInvincibleTime = 0f;

    void Start()
    {
        playerTag = this.gameObject.tag;
        sRend = gameObject.GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;    
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //确认无敌状态
        if (!invincible && Time.time > nextInvincibleTime)
        {
            invincible = true;
            nextInvincibleTime = Time.time + invincibleDuration;
            StartCoroutine(GetInvincibility());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    //进入无敌状态与取消无敌状态的协程
    IEnumerator GetInvincibility()
    {
        while (Time.time < nextInvincibleTime)
        {
            if (playerTag == "Enemy")
            {
                sRend.color = Color.red;
            }
            else if (playerTag == "Hero")
            {
                if(((int)(Time.time/0.08))%2 == 0)
                {
                    sRend.enabled = false;
                }
                else
                {
                    sRend.enabled = true;
                }
            }
            yield return null;
        }
        invincible = false;
    }
}
