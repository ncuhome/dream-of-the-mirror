using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnemy : Enemy
{
    //这个没有enemyAttackConsciousness
    [Header("Bat: ")]
    //蝙蝠最少攻击时间
    public float timeThinkMin = 2f;
    //蝙蝠最大攻击时间
    public float timeThinkMax = 4f;
    //三点线性插值
    public Vector2[] points;
    //微调下落点的位置
    public float flyLossY = 0.2f;

    public float attackConsciousnessRange;
    public float speed;
    public float speedRate = 2;

    private float heroDistance;
    private float timeNextDecision = 0;

    protected override void Start()
    {
        base.Start();
        points = new Vector2[3];
    }

    protected override void Update()
    {
        Vector2 dir;
        dir = girlHero.transform.position - transform.position;
        heroDistance = dir.magnitude;

        if (heroDistance < attackConsciousnessRange)
        {
            if (Time.time > timeNextDecision)
            {
                Vector2 targetPos;
                targetPos.x = girlHero.transform.position.x;
                targetPos.y = girlHero.transform.position.y - flyLossY;
                points[0] = transform.position;
                points[1] = targetPos;
                points[2].x = points[0].x + (points[1].x - points[0].x)*2;
                points[2].y = points[0].y;

                //判断贴图方向
                if ((points[2].x - transform.position.x) < 0)
                {
                    Flip(false);
                }
                else
                {
                    Flip(true);
                }

                //进行这一次攻击，同时为下一次攻击做准备
                float thinkTime = Random.Range(timeThinkMin, timeThinkMax);
                timeNextDecision = Time.time + thinkTime;
                StartCoroutine(Move(thinkTime));
            }
        }
    }

    //为什么线性插值会出错哇，我不理解
    public IEnumerator Move(float thinkTime)
    {
        float u = (timeNextDecision - Time.time) / thinkTime;
        while (u >= 0)
        {
            u = (timeNextDecision - Time.time) / thinkTime;
            //插值
            Vector3 p01, p12;
            u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
            p01 = u * points[0] + (1 - u) * points[1];
            p12 = u * points[1] + (1 - u) * points[2];
            transform.position = u * p01 + (1 - u) * p12;

            yield return null;
        } 
    }

    public void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Hero" || other.tag == "HeroBo")
        {
            other.GetComponent<Health>().TakeDamage(closeDamage);
        }
    }
}
