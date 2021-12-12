using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public float invincibleDuration = 0.5f;
    public bool invincible = false;
    public SpriteRenderer sRend;

    public float nextInvincibleTime = 0f;

    void Start()
    {
        tag = this.gameObject.tag;
        if (tag == "Hero")
        {
            sRend = transform.Find("HeroModel").GetComponent<SpriteRenderer>();
        }
        else
        {
            sRend = GetComponent<SpriteRenderer>();
        }
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (invincible && this.gameObject.tag != "Hero")
        {
            return;
        }
        currentHealth -= damage;

        //确认无敌状态
        if (Time.time > nextInvincibleTime)
        {
            invincible = true;
            nextInvincibleTime = Time.time + invincibleDuration;
            StartCoroutine(GetInvincibility());
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Die()
    {
        if (this.gameObject.tag == "Hero")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }
        Destroy(this.gameObject);
    }

    //进入无敌状态与取消无敌状态的协程
    IEnumerator GetInvincibility()
    {
        while (Time.time < nextInvincibleTime)
        {
            if (tag == "Enemy" || tag == "SmallEnemy")
            {
                sRend.color = Color.red;
            }
            else if (tag == "Hero")
            {
                if (((int)(Time.time / 0.08)) % 2 == 0)
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
        if (tag == "Enemy" || tag == "SmallEnemy")
        {
            sRend.color = Color.white;
        }
        else if (tag == "Hero")
        {
            sRend.enabled = true;
        }
        invincible = false;
    }
}
