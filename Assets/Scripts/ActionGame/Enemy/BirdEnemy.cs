using System.Collections;
using UnityEngine;

public class BirdEnemy : Enemy
{
    [Header("Bird: ")]
    //力的大小
    public float force = 800f;
    //主角的刚体组件
    public Rigidbody2D grb;
    //鸟最少攻击时间
    public float timeThinkMin = 2f;
    //鸟最大攻击时间
    public float timeThinkMax = 4f;
    //鸟未攻击到时的站立时间
    public float standTime = 1.5f;
    //攻击前摇的时间
    public float attackPreTime = 0.5f;
    //判断鸟是否攻击，是否攻击到主角,是否处于攻击前摇
    public bool attack,attackSuccess,stand,attackPre;
    //鸟需要血量来判断思考时间的多少
    public Health birdHealth,girlHealth;
    //在目标范围内的时间
    public float timeInRange;
    //目标距离
    public float targetDistance;
    //目标距离范围
    public float targetDistanceMin,targetDistanceMax;
    //保持距离目标位置
    public Vector2 targetpoint;
    //接近还是远离
    public bool approach;
    //向前与向后的速度与攻击速度
    public float forwardSpeed,backSpeed,attackSpeed;
    //与角色的水平距离
    public float xDistance,absXDistance;
    //三点正弦插值
    public Vector2[] points;
    //减少一点水平飞的距离，避免飞出攻击范围
    public float flyLossX;
    //微调下落点的位置
    public float flyLossY = 0.2f;
    //每次的最高飞行高度（如果不够要回到最高高度）
    public float maxFlyY;

    //附在敌人上的敌人血量划动条脚本
    public EnemySlider enemySlider;
    //记录之前的血量，用于判定受伤起飞
    public int lastHealth;

    private float timeNextDecision = 0;

    protected override void Start()
    {
        base.Start();
        points = new Vector2[3];
        maxFlyY = transform.position.y;

        
        grb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        girlHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        birdHealth = GetComponent<Health>();
        lastHealth=birdHealth.maxHealth;
        enemySlider = GetComponent<EnemySlider>();
    }

    protected override void Update()
    {
        base.Update();
        if (!enemyAttackConsciousness.attackConsciousness)
        {
            return;
        }
        enemySlider.FixSlider();

        //如果受伤就取消站立姿态，起飞，并弹开玩家
        if (birdHealth.currentHealth!=lastHealth)
        {
            StartCoroutine(Fly());
            lastHealth = birdHealth.currentHealth;
            StartCoroutine(Bounce());
        }

        //记录水平距离与其绝对值
        xDistance = girlHero.transform.position.x - transform.position.x;
        if (xDistance > 0)
        {
            absXDistance = xDistance;
        }
        else
        {
            absXDistance = -xDistance;
        }
        
        //如果不攻击、站立就跟随移动
        if (!attack && !stand && !attackPre)
        {
            if (absXDistance != targetDistance)
            {
                //判定是否需要翻转并执行
                Flip(xDistance> 0);
                
                targetpoint.y = maxFlyY;
                //判定是接近还是远离
                approach = (absXDistance - targetDistance) > 0;

                if (xDistance > 0)
                {
                    targetpoint.x = girlHero.transform.position.x - targetDistance;
                }
                else
                {
                    targetpoint.x = girlHero.transform.position.x + targetDistance;
                }
                //跟随飞行
                if (approach)
                {
                    Vector2 newPos = Vector2.MoveTowards(rb.position, targetpoint, forwardSpeed * Time.deltaTime);
                    rb.MovePosition(newPos);
                }
                else
                {
                    Vector2 newPos = Vector2.MoveTowards(rb.position, targetpoint, backSpeed * Time.deltaTime);
                    rb.MovePosition(newPos);
                }   
            }
        }
        //在区域内就计算时间
        if ((absXDistance < targetDistanceMax)&&(absXDistance > targetDistanceMin))
        {
            timeInRange += Time.deltaTime;
        }
        //攻击间隔由剩余血量决定
        float thinkTime = timeThinkMin + (timeThinkMax-timeThinkMin)*birdHealth.currentHealth/birdHealth.maxHealth;
        //区域内时间多余攻击间隔，开始攻击
        if (timeInRange > thinkTime)
        {
            if (!attack)
            {
                targetpoint = girlHero.transform.position;
                attackSuccess = false;
            }
            attack = true;
            if (!attackPre)
            {
                attackPre = true;
                timeNextDecision = Time.time + attackPreTime;
            }
            //过了攻击前摇开始攻击
            if (Time.time >= timeNextDecision)
            {
                Vector2 tPos = Vector2.MoveTowards(rb.position, targetpoint, attackSpeed * Time.deltaTime);
                rb.MovePosition(tPos);
                //到达目的地后判断是否打到人，进行起飞或者停留的执行
                if (rb.position == targetpoint)
                {
                    if (attackSuccess)
                    {
                        StartCoroutine(Fly());
                    }
                    else
                    {
                        if (!stand)
                        {
                            timeNextDecision=Time.time + standTime;
                            stand = true;
                        }
                        if (Time.time >= timeNextDecision)
                        {
                            StartCoroutine(Fly());
                            StartCoroutine(Bounce());
                        }
                    }
                }
            }
        }
    }

    //起飞
    IEnumerator Fly()
    {
        attack = false;
        timeInRange = 0;
        stand = false;
        attackPre = false;
        yield return null;
    }

    //弹开玩家
    IEnumerator Bounce()
    {
        if (xDistance <= 0)
        {
            grb.AddForce(Vector2.left * force);
        }
        else
        {
            grb.AddForce(Vector2.right * force);
        }
        yield return null;
    }

    
    protected override void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.GetComponent<Health>() == null)
        {
            return;
        }
        if (other.tag == "Player")
        {
            //如果碰到玩家，则攻击成功
            if ((girlHealth.invincible == false))
            {
                attackSuccess = true;
            }
            other.GetComponent<Health>().TakeDamage(closeDamage);
        }  
    }
}