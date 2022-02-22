using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public float repelDistance;
    public float repelDuration;
    public bool isRepelled;

    public int maxHealth;
    public int currentHealth;
    public float invincibleDuration = 0.5f;
    public bool invincible = false;
    public SpriteRenderer sRend=null;
    public Rigidbody2D rb;

    private float nextInvincibleTime = 0f;
    private float repelTime;

    void Start()
    {
        if(sRend==null)
        {
            sRend = GetComponent<SpriteRenderer>();
        }
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }
    //修改为使用Damage结构体，
    public void TakeDamage(Damage damage)
    {
        if (invincible)
        {
            return;
        }
        currentHealth -= damage.damageValue;
        //进入无敌状态
        if (Time.time > nextInvincibleTime)
        {
            invincible = true;
            nextInvincibleTime = Time.time + invincibleDuration;
            StartCoroutine(GetInvincibility());
            isRepelled = true;
            repelTime = Time.time + repelDuration;
            Vector2 tempDir = damage.damageDir;
            if (tag == "Enemy")
            {
                tempDir.y = 0;
            }
            StartCoroutine(Repel(tempDir));
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }
    /// <summary>
    /// 适应之前的代码
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damageDir"></param>
    public void TakeDamage(int damage,Vector2 damageDir) {
        TakeDamage(new Damage(damage, damageDir));
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

    IEnumerator Repel(Vector2 damageDir)
    {
        Vector2 endPos = (Vector2)transform.position + repelDistance * damageDir;
        Vector2 startPos = transform.position;
        while ((repelTime - Time.time) / repelDuration >= 0)
        {
            rb.MovePosition(Vector2.Lerp(startPos, endPos, 1 - (repelTime - Time.time) / repelDuration));
            yield return null;
        }
        rb.MovePosition(endPos);
        isRepelled = false;
    }
}

public struct Damage {
    public int damageValue { get; private set; }
    public Vector2 damageDir { get; private set; }
    public Damage(int v,Vector2 dir) {
        damageValue = v;
        damageDir = dir;
    }
}