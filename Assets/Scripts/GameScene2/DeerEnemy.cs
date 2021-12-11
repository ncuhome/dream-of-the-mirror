using System.Collections;
using UnityEngine;

public class DeerEnemy : Enemy
{
    [Header("Deer: ")]
    //鹿最少攻击思考时间
    public float timeThinkMin = 4f;
    //鸟最大攻击思考时间
    public float timeThinkMax = 6f;

    public PlayerHealth playerHealth;

    private float timeNextDecision = 0;
    //远程两种攻击方式
    private string[] remoteAttack = new string[]{
        "Magic", "Jump"
    };

    protected override void Start()
    {
        base.Start();
        playerHealth = GetComponent<PlayerHealth>();
        anim.SetBool("Walk", true);
        anim.SetBool("Impact", false);
    }

    protected override void Update()
    {
        base.Update();
        if (!enemyAttackConsciousness.attackConsciousness)
        {
            return;
        }
        if (playerHealth.currentHealth <= playerHealth.maxHealth/2 && curAnimIs("Deer_Walk"))
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Impact", true);
        }

        //判断贴图方向
        if ((girlHero.transform.position.x - transform.position.x) > 0)
        {
            Flip(true);
        }
        else
        {
            Flip(false);
        }

        //远程攻击判定
        if (Time.time > timeNextDecision)
        {
            anim.SetTrigger(remoteAttack[Random.Range(0, remoteAttack.Length)]);

            //进行这一次攻击，同时为下一次攻击做准备
            float thinkTime = Random.Range(timeThinkMin, timeThinkMax);
            timeNextDecision = Time.time + thinkTime;
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Hero")
        {
            other.transform.GetComponent<PlayerHealth>().TakeDamage(closeDamage);
        }
    }
}
