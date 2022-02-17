using System.Collections;
using UnityEngine;

public enum BirdState
{
    idle,
    attackPre,
    attack,
    stand
}

public class BirdEnemy : Enemy
{
    [Header("Bird: ")]
    public float force = 800f;
    public float timeThinkMin = 2f;
    public float timeThinkMax = 4f;
    public float standDuration = 1.5f;
    public float attackPreDuration = 0.5f;
    public float followingDistanceX = 6f;
    public float maxFlyY = 0;
    public float bounceDistance = 1;

    
    //向前与向后的速度与攻击速度
    public float forwardSpeed, backSpeed, attackSpeed;
    public Rigidbody2D grb;
    public EnemySlider enemySlider;
    //鸟需要血量来判断思考时间的多少
    public Health birdHealth,girlHealth;
    public BirdState birdState;

    private float flySpeed;
    //攻击间隔由剩余血量决定
    private float thinkDuration;
    //在目标范围内的时间
    private float timeInRange;
    private bool attackSuccess;
    private float attackTime = 0;
    private float timeStand;
    private Vector2 targetPoint;
    

    protected override void Start()
    {
        base.Start();
        if (maxFlyY == 0)
        {
            maxFlyY = transform.position.y;
        }

        grb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        girlHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        birdHealth = GetComponent<Health>();
        enemySlider = GetComponent<EnemySlider>();
        targetPoint.y = maxFlyY;
    }

    void FixedUpdate()
    {
        if (!enemyAttackConsciousness.attackConsciousness)
        {
            timeInRange = 0;
            return;
        }
        if (birdState == BirdState.idle || birdState == BirdState.attack)
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, targetPoint, flySpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (!enemyAttackConsciousness.attackConsciousness)
        {
            return;
        }

        enemySlider.FixSlider();
        //判断贴图方向
        Flip((girlHero.transform.position.x - transform.position.x) > 0);

        switch (birdState)
        {
            case BirdState.idle:
                if (Mathf.Abs(girlHero.transform.position.x - transform.position.x) > followingDistanceX)
                {
                    flySpeed = forwardSpeed;
                }
                else
                {
                    flySpeed = backSpeed;
                }                
                targetPoint.x = girlHero.transform.position.x + (-1) * transform.right.x * (int)facing * followingDistanceX;
                targetPoint.y = maxFlyY;
                thinkDuration = timeThinkMin + (timeThinkMax - timeThinkMin) * (birdHealth.currentHealth / birdHealth.maxHealth);

                if (timeInRange < thinkDuration)
                {
                    timeInRange += Time.deltaTime;
                }
                else
                {
                    timeInRange = 0;
                    birdState = BirdState.attackPre;
                }
                attackTime = Time.time + attackPreDuration;
                break;
            case BirdState.attackPre:
                targetPoint = girlHero.transform.position;
                if (Time.time > attackTime)
                {
                    birdState = BirdState.attack;
                    flySpeed = attackSpeed;
                }
                break;
            case BirdState.attack:
                //到达目的地后判断是否打到人，进行起飞或者停留的执行
                if (rb.position == targetPoint)
                {
                    if (attackSuccess)
                    {
                        birdState = BirdState.idle;
                    }
                    else
                    {
                        birdState = BirdState.stand;
                        timeStand = Time.time + standDuration;
                    }
                }
                break;
            case BirdState.stand:
                if (Time.time > timeStand)
                {
                    Bounce();
                    birdState = BirdState.idle;
                }
                break;
        }
    }

    //弹开玩家
    private void Bounce()
    {
        if (Mathf.Abs(grb.position.x - rb.position.x) > bounceDistance)
        {
            return;
        }
        if ((grb.position.x - rb.position.x) <= 0)
        {
            grb.AddForce(Vector2.left * force);
        }
        else
        {
            grb.AddForce(Vector2.right * force);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
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
        }
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerStay2D(other);
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
        }
    }
}