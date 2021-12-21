using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Boss: ")]
    //Boss最少攻击思考时间
    public float timeThinkMin = 4f;
    //Boss最大攻击思考时间
    public float timeThinkMax = 6f;
    public float attackRange = 1.5f;
    public float attackCd = 0.5f;
    public int attackDamage = 1;
    public Health playerHealth;

    public float timeNextDecision = 0;

    private float nextAttackTime;
    //远程两种方式
    private string[] remoteAttack = new string[]{
        "Magic", "Teleport"
    };

    protected override void Start()
    {
        base.Start();
        playerHealth = GetComponent<Health>();
    }

    protected override void Update()
    {
        base.Update();
        if (enemyAttackConsciousness.heroDistance > enemyAttackConsciousness.attackConsciousnessRange)
        {
            return;
        }
        if (playerHealth.currentHealth <= playerHealth.maxHealth/2)
        {
            timeThinkMin /= 2;
            timeThinkMax /= 2;
        }
        if (!curAnimIs("Boss_Teleport"))
        {
            //判断贴图方向
            if ((girlHero.transform.position.x - transform.position.x) > 0)
            {
                Flip(false);
            }
            else
            {
                Flip(true);
            }
        }

        //特殊动作判定
        if (Time.time > timeNextDecision)
        {
            anim.SetTrigger(remoteAttack[Random.Range(0, remoteAttack.Length)]);

            //进行这一次攻击，同时为下一次攻击做准备
            float thinkTime = Random.Range(timeThinkMin, timeThinkMax);
            timeNextDecision = Time.time + thinkTime;
        }

        //攻击判定
        if (!curAnimIs("Boss_Magic") && Time.time >= nextAttackTime)
        {
            if (enemyAttackConsciousness.heroDistance < attackRange)
            {
                anim.SetTrigger("Attack");
                nextAttackTime += attackCd;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.GetComponent<Health>() == null || !curAnimIs("Boss_Attack"))
        {
            return;
        }
        if (other.tag == "Hero")
        {
            other.GetComponent<Health>().TakeDamage(attackDamage);
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Health>() != null && curAnimIs("Boss_Attack"))
        {
            if (Time.time > other.gameObject.GetComponent<Health>().nextInvincibleTime)
            {
                if (other.tag == "Hero")
                {
                    other.GetComponent<Health>().TakeDamage(attackDamage);
                }
            }   
        } 
    }
}
