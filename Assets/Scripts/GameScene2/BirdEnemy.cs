using System.Collections;
using UnityEngine;

public class BirdEnemy : Enemy
{
    [Header("Bird: ")]
    //鸟最少攻击时间
    public float timeThinkMin = 2f;
    //鸟最大攻击时间
    public float timeThinkMax = 4f;
    //三点正弦插值
    public Vector2[] points;
    //减少一点水平飞的距离，避免飞出攻击范围
    public float flyLossX;
    //微调下落点的位置
    public float flyLossY = 0.2f;
    //每次的最高飞行高度（如果不够要回到最高高度）
    public float maxFlyY;

    private float timeNextDecision = 0;

    protected override void Start()
    {
        base.Start();
        points = new Vector2[3];
        maxFlyY = transform.position.y;
    }

    protected override void Update()
    {
        base.Update();
        if (heroDistance > attackRange)
        {
            return;
        }

        if (Time.time > timeNextDecision)
        {
            Vector2 tPos = transform.position;
            Camera cam = Camera.main;
            float height = 2f * cam.orthographicSize;
            float width = height * cam.aspect;

            points[0] = tPos;

            tPos.x = cam.transform.position.x;
            tPos.y = cam.transform.position.y - height/2 - flyLossY;
            points[1] = tPos;

            //减少一点水平飞的距离，避免飞出攻击范围
            float vecDif = cam.transform.position.x - transform.position.x;
            if (vecDif == 0)
            {
                tPos.x = cam.transform.position.x;
            }
            if (vecDif < 0)
            {
                tPos.x = cam.transform.position.x + (cam.transform.position.x - transform.position.x) + flyLossX;
            }
            if (vecDif > 0)
            {
                tPos.x = cam.transform.position.x + (cam.transform.position.x - transform.position.x) - flyLossX;
            }

            tPos.x = cam.transform.position.x + (cam.transform.position.x - transform.position.x);
            tPos.y = maxFlyY;
            points[2] = tPos;

            //判断贴图方向
            if ((points[2].x - transform.position.x) < 0)
            {
                Flip(true);
            }
            else
            {
                Flip(false);
            }

            //进行这一次攻击，同时为下一次攻击做准备
            float thinkTime = Random.Range(timeThinkMin, timeThinkMax);
            timeNextDecision = Time.time + thinkTime;
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

    public void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Hero")
        {
            other.GetComponent<PlayerHealth>().TakeDamage(closeDamage);
        }
    }
}
