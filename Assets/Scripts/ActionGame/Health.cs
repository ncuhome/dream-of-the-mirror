using System.Collections;
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
        tag = gameObject.tag;
        if (tag == "Player")
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
        if (invincible)
        {
            return;
        }

        currentHealth -= damage;

        //进入无敌状态
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
        if (this.gameObject.tag == "Player")
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
            if (tag == "Enemy")
            {
                sRend.color = Color.red;
            }
            if (tag == "Player")
            {
                //每0.16s闪烁一次
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

        if (tag == "Enemy")
        {
            sRend.color = Color.white;
        }
        if (tag == "Player")
        {
            sRend.enabled = true;
        }

        invincible = false;
    }
}
