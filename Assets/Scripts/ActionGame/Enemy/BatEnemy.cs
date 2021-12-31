using System.Collections;
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

    private float timeNextDecision = 0;

    protected override void Start()
    {
        base.Start();
        points = new Vector2[3];
    }

    protected override void Update()
    {
        base.Update();

        if (!enemyAttackConsciousness.attackConsciousness)
        {
            return;
        }

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
            Flip((points[2].x - transform.position.x) > 0);

            //进行这一次攻击，同时为下一次攻击做准备
            float thinkTime = Random.Range(timeThinkMin, timeThinkMax);
            timeNextDecision = GetNextTime(thinkTime);
            StartCoroutine(Move(thinkTime));
        }
    }

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

    protected override void OnTriggerEnter2D(Collider2D other) 
    {
        base.OnTriggerEnter2D(other);
    }
}
