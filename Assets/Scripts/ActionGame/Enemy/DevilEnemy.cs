using System.Collections;
using UnityEngine;


/// </summary>
[System.Serializable] 
public class DevilAttack
{
    public DevilState devilState;
    public int attackDamage;
    public float attackPreDuration;
    public float attackBackDuration;
}
public enum DevilState
{
    scratch1 , scratch2 , scratch3 , scratch4,
    dash , dashAttack,
    impact,
    storm,
    idle,
    dodge
}

public enum AttackState
{
    idle,
    attackPre,
    attack
}

public class DevilEnemy : Enemy
{
    public float meleeAttackDistance , rangedAttackDistance , verticalAttackDistance;
    //记录当前移动速度
    private float moveSpeed;
    //获取血量判定攻击逻辑
    public Health _health,girlHeroHealth;
    //进入第一和第二阶段的血量百分比
    public float firstStageHealthPer , secondStageHealthPer;
    //向前移动的概率
    public float moveForwardRate , dodgeRate;
    public EnemySlider enemySlider;
    //记录当前攻击前摇，攻击后摇，两次近战的最小攻击间隔,移动方向变换的间隔
    public float attackPreDuration , attackBackDuration , meleeAttackDuration , moveDuration , dodgeDuration;
    //记录当前的攻击伤害
    private int attackDamage;
    public DevilState devilState;
    private Vector2 targetPoint;
    private Vector2 damageDir;
    public AttackState attackState;
    public float scratchSpeed , dashSpeed , idleSpeed , dodgeSpeed;
    private float horizontalDistance,verticalDistance;
    private float timeNextDecision,timeNextMove = 0f;
    //记录当前朝向
    private float devilFacing;
    //记录在地面的高度
    public float groundY;
    //记录下砸攻击的段数
    private int impactTimes;
    //判定两个阶段的下砸攻击是否触发过
    private bool impactAttack1,impactAttack2;
    //判断是否触发过闪避判断
    private bool dodge;
    
    //放置每种状态的攻击前摇，攻击后摇，攻击伤害
    public DevilAttack[] devilAttacks;
    public Bullet bullet;
    private float dodgeRandom;
    private DevilState[] verticalAttack = new DevilState[]{ 
        DevilState.dash , DevilState.storm
    };
    private int moveDirection;


    protected override void Start()
    {
        base.Start();
        
        _health = GetComponent<Health>();
        enemySlider = GetComponent<EnemySlider>();
        girlHeroHealth = PlayerManager.instance.girlHero.GetComponent<Health>();
        attackState = AttackState.idle;
        groundY = transform.position.y;
        impactAttack1 = false;
        impactAttack2 = false;
        dodge = false;   
    }


    void FixedUpdate()
    {
        if ((!enemyAttackConsciousness.attackConsciousness) && (devilState == DevilState.idle))
        {
            return;
        }
        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPoint, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        enemySlider.FixSlider();

        if ((girlHero.anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Sword")) && (attackState != AttackState.attack) && (!dodge))
        {
            dodge = true;
            dodgeRandom = Random.value;
            if (dodgeRandom <= dodgeRate)
            {
                anim.SetTrigger("Dodge");
                attackState = AttackState.idle;
                devilState = DevilState.dodge;
                moveSpeed = dodgeSpeed;
                targetPoint = new Vector2 (transform.position.x + moveSpeed * devilFacing * dodgeDuration , groundY);
                timeNextDecision = Time.time + dodgeDuration;
            }
        }
        if (!girlHero.anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Sword"))
        {
            dodge = false;
        }

        //当达到50%与30%血量时触发下砸攻击
        if ((_health.maxHealth * firstStageHealthPer >= _health.currentHealth) && (!impactAttack1))
        {
            attackState = AttackState.idle;
            impactAttack1 = true;
            devilState = DevilState.impact;
        }
        if ((_health.maxHealth * secondStageHealthPer >= _health.currentHealth) && (!impactAttack2))
        {
            impactAttack2 = true;
            devilState = DevilState.impact;
        }
        
        verticalDistance = girlHero.transform.position.y - transform.position.y;
        horizontalDistance = girlHero.transform.position.x - transform.position.x;

        DevilStateUpdate();

        switch (devilState)
        {
            case DevilState.idle:
                Move();
                attackState = AttackState.idle;
                //如果在近战范围就进入近战攻击
                if ((Mathf.Abs( horizontalDistance ) < meleeAttackDistance) && (Time.time > timeNextDecision))
                {
                    devilState = DevilState.scratch1;
                }
                //如果在远程范围之外就随机触发一种远程攻击
                if ((Mathf.Abs( horizontalDistance ) > rangedAttackDistance))
                {
                    devilState = verticalAttack[Random.Range(0,verticalAttack.Length)];
                }
                break;

            case DevilState.dodge:  
                if (Time.time > timeNextDecision)
                {
                    devilState = DevilState.idle;
                }
                break;

            case DevilState.scratch1:
                switch (attackState)
                {
                    case AttackState.idle:
                        targetPoint = new Vector2(transform.position.x,groundY);
                        Flip(horizontalDistance > 0);
                        AttackPre();
                        break;
                    case AttackState.attackPre:
                        if(Time.time > timeNextDecision)
                        {
                            //触发抓击动画
                            anim.SetTrigger("Scratch");
                            MeleeAttack();
                        }
                        break;
                    case AttackState.attack:
                        if (Time.time > timeNextDecision)
                        {
                            //转向玩家并前突，进入第二段攻击
                            Flip(horizontalDistance > 0);
                            moveSpeed = scratchSpeed;
                            targetPoint = new Vector2(transform.position.x - meleeAttackDistance / 2 * devilFacing ,groundY);
                            timeNextDecision = Time.time + (targetPoint.x - transform.position.x) / moveSpeed;  
                            devilState = DevilState.scratch2;
                            attackState = AttackState.idle;
                        }
                        break;
                }
                break;
            case DevilState.scratch2:
                switch (attackState)
                {
                    case AttackState.idle:
                        if (Time.time > timeNextDecision)
                        {
                            AttackPre();
                        }
                        break;
                    case AttackState.attackPre:
                        if (Time.time > timeNextDecision)
                        {
                            anim.SetTrigger("Scratch");
                            MeleeAttack();
                        }
                        break;
                    case AttackState.attack:
                        if (Time.time > timeNextDecision)
                        {
                            //前突并进入第三段攻击
                            targetPoint = new Vector2(transform.position.x - meleeAttackDistance / 2 * devilFacing ,groundY);
                            devilState = DevilState.scratch3;
                            attackState = AttackState.idle;
                            timeNextDecision = Time.time + (targetPoint.x - transform.position.x) / moveSpeed;  
                        }
                        break;
                }
                break;
            case DevilState.scratch3:
                switch (attackState)
                {
                    case AttackState.idle:
                        if (Time.time > timeNextDecision)
                        {
                            AttackPre();
                        }
                        break;
                    case AttackState.attackPre:
                        if (Time.time > timeNextDecision)
                        {
                            anim.SetTrigger("Scratch");
                            MeleeAttack();
                        }
                        break;
                    case AttackState.attack:
                        if (Time.time > timeNextDecision)
                        {
                            Flip(horizontalDistance > 0);
                            attackState = AttackState.idle;
                            if (Mathf.Abs(horizontalDistance) < rangedAttackDistance)
                            {
                                //如果在近战攻击范围内，就直接进入第四段，否则前进一步再进入第四段
                                if (Mathf.Abs(horizontalDistance) > meleeAttackDistance)
                                {
                                    moveSpeed = scratchSpeed;
                                    DevilStateUpdate();
                                    targetPoint = new Vector2(transform.position.x - meleeAttackDistance / 2 * devilFacing ,groundY);
                                    timeNextDecision = Time.time + (targetPoint.x - transform.position.x) / moveSpeed;  
                                }
                                devilState = DevilState.scratch4;
                            } else
                            {
                                //如果在远程范围外就进入冲刺攻击
                                devilState = DevilState.dash;
                            }
                        }
                        break;
                }
                break;
            case DevilState.scratch4:
                switch (attackState)
                {
                    case AttackState.idle:
                        if (Time.time > timeNextDecision)
                        {
                            AttackPre();
                        }
                        break;
                    case AttackState.attackPre:
                        if (Time.time > timeNextDecision)
                        {
                            anim.SetTrigger("Scratch");
                            MeleeAttack();
                        }
                        break;
                    case AttackState.attack:
                        if (Time.time > timeNextDecision)
                        {
                            //打完四段攻击回归普通状态
                            devilState = DevilState.idle;
                            attackState = AttackState.idle;
                            timeNextDecision = Time.time + meleeAttackDuration;
                        }
                        break;
                }
                break;

            case DevilState.dash:
                switch (attackState)
                { 
                    case AttackState.idle:
                        moveSpeed = dashSpeed;
                        AttackPre();
                        break;
                    case AttackState.attackPre:
                        if (Time.time > timeNextDecision)
                        {
                            anim.SetTrigger("Dash");
                            attackState = AttackState.attack;
                        }
                        break;
                    case AttackState.attack: 
                        targetPoint = new Vector2(girlHero.transform.position.x,groundY);
                        if (Mathf.Abs(horizontalDistance) <= meleeAttackDistance)
                        {
                            //冲刺完毕进入近身攻击
                            targetPoint = new Vector2(transform.position.x , groundY);
                            attackState = AttackState.idle;
                            devilState = DevilState.dashAttack;
                        }
                        break;
                }
                break;
            case DevilState.dashAttack:
                switch (attackState)
                {
                    case AttackState.idle:
                        Flip(horizontalDistance > 0);
                        AttackPre();                        
                        break;
                    case AttackState.attackPre:
                        if (Time.time > timeNextDecision)
                        {   
                            anim.SetTrigger("DashAttack");
                            MeleeAttack();
                        }
                        break;
                    case AttackState.attack:
                        if (Time.time > timeNextDecision)
                        {
                            attackState = AttackState.idle;
                            devilState = DevilState.idle;
                        }
                        break;
                }
                break;

            case DevilState.impact:
                switch (attackState)
                {
                    case AttackState.idle:
                        //站立不动
                        targetPoint = new Vector2(transform.position.x , groundY);
                        impactTimes = 0;
                        AttackPre();
                        break;
                    case AttackState.attackPre:
                        if (Time.time > timeNextDecision)
                        {
                            anim.SetTrigger("Impact");
                            attackState = AttackState.attack;
                        }
                        break;
                    case AttackState.attack:
                        switch (impactTimes)
                        {
                            //三段攻击分别攻击三个范围
                            case 0:
                                if ((Mathf.Abs(horizontalDistance) < meleeAttackDistance) && (Mathf.Abs(verticalDistance) < verticalAttackDistance))
                                {
                                    Vector2 damageDir;
                                    damageDir = (girlHero.transform.position - transform.position).normalized;
                                    girlHeroHealth.TakeDamage(attackDamage , damageDir);
                                    timeNextDecision = Time.time + attackBackDuration;
                                }
                                if (Time.time > timeNextDecision)
                                {
                                    impactTimes += 1;
                                    anim.SetTrigger("Impact");
                                }
                                break;
                            case 1:
                                if ((Mathf.Abs(horizontalDistance) >= meleeAttackDistance) && (Mathf.Abs(horizontalDistance) < rangedAttackDistance) && (Mathf.Abs(verticalDistance) < verticalAttackDistance))
                                {
                                    Vector2 damageDir;
                                    damageDir = (girlHero.transform.position - transform.position).normalized;
                                    girlHeroHealth.TakeDamage(attackDamage , damageDir);
                                    timeNextDecision = Time.time + attackBackDuration;
                                }
                                if (Time.time > timeNextDecision)
                                {
                                    impactTimes += 1;
                                    anim.SetTrigger("Impact");
                                }
                                break;    
                            case 2:
                                if ((Mathf.Abs(horizontalDistance) >= rangedAttackDistance) && (Mathf.Abs(verticalDistance) < verticalAttackDistance))
                                {
                                    Vector2 damageDir;
                                    damageDir = (girlHero.transform.position - transform.position).normalized;
                                    girlHeroHealth.TakeDamage(attackDamage , damageDir);
                                    timeNextDecision = Time.time + attackBackDuration;
                                }
                                if (Time.time > timeNextDecision)
                                {
                                    impactTimes += 1;
                                    anim.SetTrigger("Impact");
                                }
                                break;
                            case 3:
                                //攻击结束
                                attackState = AttackState.idle;
                                devilState = DevilState.idle;
                                break;
                        }
                        break;
                }
                break;
            case DevilState.storm:
                switch (attackState)
                {
                    case AttackState.idle:
                        Flip(horizontalDistance > 0);                       
                        AttackPre();
                        break;
                    case AttackState.attackPre:
                        if (Time.time > timeNextDecision)
                        {
                            targetPoint = new Vector2 (transform.position.x , groundY);
                            timeNextDecision = targetPoint.y - transform.position.y;
                            attackState = AttackState.attack;
                        }
                        break;
                    case AttackState.attack:
                        if (Time.time > timeNextDecision)
                        {
                            //开始攻击动画并且在身前生成风暴
                            anim.SetTrigger("Storm");
                            Quaternion bulletRotation = transform.rotation;
                            bulletRotation.y = (transform.rotation.y == 0) ? 180 : 0;
                            Instantiate(bullet, transform.position + transform.right * (-1), bulletRotation);
                            attackState = AttackState.idle;
                            devilState = DevilState.idle;
                        }
                        break;
                }
                break;                
        }
    }

    protected override void Update()
    {
        base.Update();  
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Health>() == null)
        {
            return;
        }
        //如果在冲刺中则开启碰撞伤害，否则关闭
        if ((other.tag == "Player") && (devilState == DevilState.dash))
        {
            Vector2 damageDir;
            damageDir = (other.transform.position - transform.position).normalized;
            other.GetComponent<Health>().TakeDamage(attackDamage, damageDir);
        }
    }



    //刷新状态
    void DevilStateUpdate()
    {
        attackDamage = devilAttacks[(int) devilState].attackDamage;
        attackPreDuration = devilAttacks[(int) devilState].attackPreDuration;
        attackBackDuration = devilAttacks[(int) devilState].attackBackDuration;
        devilFacing = transform.rotation.y * 2 + 1;
    }

    //进入攻击前摇阶段
    void AttackPre()
    {
        attackState = AttackState.attackPre;
        timeNextDecision = Time.time + attackPreDuration;
    }

    //进行攻击判定并且进入攻击后摇阶段
    void MeleeAttack()
    {
        if (((horizontalDistance / devilFacing) < 0) && (Mathf.Abs(horizontalDistance)< meleeAttackDistance) && (Mathf.Abs(verticalDistance) < verticalAttackDistance))
        {
            Vector2 damageDir;
            damageDir = (girlHero.transform.position - transform.position).normalized;
            girlHeroHealth.TakeDamage(attackDamage , damageDir);
        }
        timeNextDecision = Time.time + attackBackDuration;
        attackState = AttackState.attack;
    }

    void Move()
    {
        moveSpeed = idleSpeed;
        moveDirection = (Random.value <= moveForwardRate) ? 1 : -1;
        if (Time.time > timeNextMove)
        {
            targetPoint = new Vector2 (transform.position.x - moveSpeed * moveDuration * moveDirection * devilFacing , groundY);
            timeNextMove = Time.time + moveDuration;
        }
    }





}
