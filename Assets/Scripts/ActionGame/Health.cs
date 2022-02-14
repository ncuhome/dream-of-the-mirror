using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    public float repelDistance;
    public float repelDuration;

    public int maxHealth;
    public int currentHealth;
    public float invincibleDuration = 0.5f;
    public bool invincible = false;
    public SpriteRenderer sRend;

    public float nextInvincibleTime = 0f;

    private float repelTime;

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

    public void TakeDamage(int damage, Vector2 damageDir)
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
            if (tag == "Player")
            {
                StartCoroutine(PlayerRepel(damageDir));
            }
            
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
        float startTime = Time.time;
        while (Time.time < nextInvincibleTime)
        {
            if (tag == "Enemy")
            {
                sRend.color = Color.red;
            }
            if (tag == "Player")
            {
                //每0.26个无敌周期闪烁一次
                if (((int)((Time.time - startTime) / (0.26f * invincibleDuration))) % 2 == 0)
                {
                    sRend.color = Color.red;
                }
                else
                {
                    sRend.color = Color.white;
                }
            }
            yield return null;
        }
        sRend.color = Color.white;
        invincible = false;
    }

    IEnumerator PlayerRepel(Vector2 damageDir)
    {
        Vector2 endPos = (Vector2)transform.position + repelDistance * damageDir;
        Vector2 startPos = transform.position;
        while ((repelTime - Time.time) / repelDuration >= 0)
        {
            transform.position = Vector2.Lerp(startPos, endPos, 1-(repelTime - Time.time) / repelDuration);
            yield return null;
        }
        print(endPos);
        transform.position = endPos;
    }
}
