using System.Security.Cryptography.X509Certificates;
using System.Collections;
using UnityEngine;


/// </summary>
[System.Serializable] 
public class DevilAttack
{
    public DevilState devilState;
    public int attackDamage;
    //攻击前摇时间
    public float attackPreDuration;
    //攻击时间
    public float attackDuration;
    //攻击后摇时间
    public float attackBackDuration;
}
public enum DevilState
{
    idle,
    walk,
    scratch1, scratch2, scratch3, scratch4,
    dash, dashAttack,
    //下拍暴怒攻击
    impact,
    //远程风暴攻击
    storm,
    //闪避玩家攻击
    dodge
}

// public enum AttackState
// {
//     idle,
//     attackPre,
//     attack
// }

public enum AttackState
{
    attackPre,
    attack,
    attackEnd
}

public class DevilEnemy : Enemy
{
    //Boss阶段百分比：
    public float firstStageHealthPer, secondStageHealthPer;
    public float scratchSpeed, dashSpeed, walkSpeed, jumpSpeed, dodgeSpeed;
    //近战水平攻击距离, 近战竖直攻击距离, 远程水平攻击距离
    public float meleeAttackDistance, verticalAttackDistance, rangedAttackDistance;

    [Header("位置参数")]
    public float groundY;
    public float roomLeftPointX;
    public float roomRightPointX;
    
    [Header("预制件：")]
    public Bullet stormbullet;
    public GameObject impactParticlePrefab;

    [Header("当前怪物状态")]
    public DevilState devilState;
    [Header("当前怪物状态时刻")]
    public AttackState attackState;

    [Header("每种攻击状态的攻击前摇，攻击后摇，攻击伤害，攻击时常")]
    public DevilAttack[] devilAttacks;

    [Header("移动向前概率")]
    public float moveForwardRate;
    [Header("移动最短时间")]
    public float moveDuration;
    [Header("闪避概率")]
    public float dodgeRate;
    [Header("闪避时间")]
    public float dodgeDuration;
    [Header("旋转时间")]
    public float flipDuration = 0.2f;
    [Header("僵直时间")]
    public float rigidityDuration = 0.2f;

    [Header("是否处于旋转状态")]
    public bool isFliped = false;

    [Header("是否在上升")]
    public bool isRised = false;

    private int moveDirection;

    //判断是否触发过闪避判断（闪避是快速的，理论上不会出现重复闪避）
    // private bool dodge = false;
    private float dodgeRandom;

    private float moveSpeed;
    private int attackDamage;
    private float horizontalDistance, verticalDistance;
    private float timeNextDecision, timeNextMove = 0f;
    
    //记录下砸攻击的段数
    private int impactTimes = 0;
    //判定两个阶段的下砸攻击是否触发过
    private bool impactAttack1 = false;
    private bool impactAttack2 = false;
    private ParticleSystem impactParticleSystem;
    private Vector2 impactLeftPoint, impactRightPoint;
    private GameObject prefab1, prefab2;
    
    private Health girlHeroHealth;
    private bool isEventBegin = true;
    private Vector2 targetPoint;
    private int devilFacing;
    private float attackPreDuration, attackDuration, attackBackDuration;
    private DevilState[] verticalAttack = new DevilState[]{ 
        DevilState.dash , DevilState.storm
    };

    protected override void Start()
    {
        base.Start();
        
        Flip(horizontalDistance > 0);
        groundY = transform.position.y;
        targetPoint = transform.position;
        // attackState = AttackState.idle;
        attackState = AttackState.attackPre;
        girlHeroHealth = PlayerManager.instance.girlHero.GetComponent<Health>();
        impactParticleSystem = impactParticlePrefab.GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        if (enemyAttackConsciousness.heroDistance < 0.05f)
        {
            return;
        }
        //脱离仇恨
        if ((enemyAttackConsciousness.heroDistance > enemyAttackConsciousness.attackConsciousnessRange) && (attackState == AttackState.attackPre))
        {
            devilState = DevilState.idle;
            attackState = AttackState.attackPre;
            isEventBegin = true;
            return;
        }

        DevilStateUpdate();

        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPoint, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        //当达到50%与25%血量时触发下砸攻击
        if ((health.maxHealth * firstStageHealthPer >= health.currentHealth) && (!impactAttack1))
        {
            anim.SetBool("Walk", false);
            impactAttack1 = true;
            devilState = DevilState.impact;
            attackState = AttackState.attackPre;
            isEventBegin = true;
        }
        if ((health.maxHealth * secondStageHealthPer >= health.currentHealth) && (!impactAttack2) && impactAttack1)
        {
            anim.SetBool("Walk", false);
            impactAttack2 = true;
            devilState = DevilState.impact;
            attackState = AttackState.attackPre;
            isEventBegin = true;
        }

        // Debug.Log(devilState + " and " + attackState);
        switch (devilState)
        {
            //idle无前摇后摇
            case DevilState.idle:
                //默认状态结束
                if (Time.time > timeNextMove)
                {
                    anim.SetBool("Walk", true);
                    devilState = DevilState.walk;
                    attackState = AttackState.attackPre;
                    isEventBegin = true;
                }
                break;
                
            //walk无前摇后摇
            case DevilState.walk:
                anim.speed = 1;
                moveSpeed = walkSpeed;
                moveDirection = (Random.value <= moveForwardRate) ? 1 : -1;
                if (!anim.GetBool("Walk"))
                {
                    Flip(horizontalDistance > 0);
                    targetPoint = new Vector2 (transform.position.x - moveSpeed * moveDuration * moveDirection * devilFacing , groundY);
                    timeNextMove = Time.time + moveDuration;
                }
                anim.SetBool("Walk", true);
                if (Time.time > timeNextMove)
                {
                    if ((Mathf.Abs( horizontalDistance ) < meleeAttackDistance))
                    {
                        anim.SetBool("Walk", false);
                        devilState = DevilState.scratch1;
                        attackState = AttackState.attackPre;
                    }
                    //如果在远程范围之外就随机触发一种远程攻击
                    else if ((Mathf.Abs( horizontalDistance ) > rangedAttackDistance))
                    {
                        anim.SetBool("Walk", false);
                        devilState = verticalAttack[Random.Range(0,verticalAttack.Length)];
                        attackState = AttackState.attackPre;
                    }
                    else
                    {
                        Flip(horizontalDistance > 0);
                        targetPoint = new Vector2 (transform.position.x - moveSpeed * moveDuration * moveDirection * devilFacing, groundY);
                        timeNextMove = Time.time + moveDuration;
                    }
                }
                
                break;

            //闪避暂时无前摇后摇
            case DevilState.dodge: 
                //闪避开始事件
                if (isEventBegin)
                {
                    timeNextDecision = Time.time + dodgeDuration;
                    isEventBegin = false;
                }
                //闪避结束事件
                if (Time.time > timeNextDecision)
                {
                    health.invincible = false;
                    devilState = DevilState.walk;
                    attackState = AttackState.attackPre;
                    isEventBegin = true;
                }
                break;

            case DevilState.scratch1:
                switch (attackState)
                {
                    case AttackState.attackPre:
                        //前摇开始事件
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackPreDuration;
                            targetPoint = new Vector2(transform.position.x, groundY);
                            // Flip(horizontalDistance > 0, flipDuration);
                            Flip(horizontalDistance > 0);
                            //触发爪击动画
                            anim.SetTrigger("Scratch");

                            isEventBegin = false;
                        }
                        //前摇结束事件
                        if (Time.time > timeNextDecision)
                        {
                            attackState = AttackState.attack;
                            isEventBegin = true;
                        }
                        break;
                    case AttackState.attack:
                        //攻击准备事件
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackDuration;

                            isEventBegin = false;
                            break;
                        }
                        //攻击期间事件
                        if (Time.time <= timeNextDecision)
                        {
                            //持续攻击
                            MeleeAttack();
                        }
                        //攻击结束事件
                        if (Time.time > timeNextDecision)
                        {
                            attackState = AttackState.attackEnd;

                            isEventBegin = true;
                        }
                        break;
                    case AttackState.attackEnd:
                        //攻击后摇准备事件
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackBackDuration;

                            isEventBegin = false;
                            break;
                        }
                        //攻击后摇结束事件
                        if (Time.time > timeNextDecision)
                        {
                            //转向玩家并前突，进入第二段攻击
                            devilState = DevilState.scratch2;
                            attackState = AttackState.attackPre;
                            isEventBegin = true;
                        }
                        break;
                }
                break;

            case DevilState.scratch2:
                switch (attackState)
                {
                    case AttackState.attackPre:
                        //前摇开始事件
                        if (isEventBegin)
                        {
                            moveSpeed = scratchSpeed;
                            timeNextDecision = Time.time + attackPreDuration;
                            //前突
                            targetPoint = new Vector2(transform.position.x - meleeAttackDistance / 2 * devilFacing, groundY);
                            // Flip(horizontalDistance > 0, flipDuration / 2);
                            Flip(horizontalDistance > 0);
                            //触发爪击动画
                            anim.SetTrigger("Scratch");
                            anim.speed = 2;

                            isEventBegin = false;
                            break;
                        }
                        //前摇结束事件
                        if (Time.time > timeNextDecision)
                        {
                            attackState = AttackState.attack;
                            isEventBegin = true;
                        }
                        break;
                    case AttackState.attack:
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackDuration;

                            isEventBegin = false;
                            break;
                        }
                        //攻击期间事件
                        if (Time.time <= timeNextDecision)
                        {
                            MeleeAttack();
                        }
                        //攻击结束事件
                        if (Time.time > timeNextDecision)
                        {
                            attackState = AttackState.attackEnd;

                            isEventBegin = true;
                        }
                        break;
                    case AttackState.attackEnd:
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackBackDuration;

                            isEventBegin = false;
                            break;
                        }
                        //攻击后摇结束事件
                        if (Time.time > timeNextDecision)
                        {
                            anim.speed = 1;
                            devilState = DevilState.scratch3;
                            attackState = AttackState.attackPre;
                            isEventBegin = true;
                        }
                        break;
                }
                break;

            case DevilState.scratch3:
                switch (attackState)
                {
                    case AttackState.attackPre:
                        //前摇开始事件
                        if (isEventBegin)
                        {
                            moveSpeed = scratchSpeed;
                            timeNextDecision = Time.time + attackPreDuration;
                            //前突
                            targetPoint = new Vector2(transform.position.x - meleeAttackDistance / 2 * devilFacing, groundY);
                            //触发爪击动画
                            anim.SetTrigger("Scratch");
                            anim.speed = 2;

                            isEventBegin = false;
                            break;
                        }
                        //前摇结束事件
                        if (Time.time > timeNextDecision)
                        {
                            attackState = AttackState.attack;
                            isEventBegin = true;
                        }
                        break;
                    case AttackState.attack:
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackDuration;

                            isEventBegin = false;
                            break;
                        }
                        //攻击期间事件
                        if (Time.time <= timeNextDecision)
                        {
                            MeleeAttack();
                        }
                        //攻击结束事件
                        if (Time.time > timeNextDecision)
                        {
                            attackState = AttackState.attackEnd;

                            isEventBegin = true;
                        }
                        break;
                    case AttackState.attackEnd:
                        //攻击后摇开始事件
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackBackDuration;

                            isEventBegin = false;
                            break;
                        }
                        //攻击后摇结束事件
                        if (Time.time > timeNextDecision)
                        {
                            anim.speed = 1;
                            devilState = DevilState.scratch4;
                            attackState = AttackState.attackPre;

                            isEventBegin = true;
                        }
                        break;
                }
                break;

            case DevilState.scratch4:
                switch (attackState)
                {
                    case AttackState.attackPre:
                        //前摇开始事件
                        if (isEventBegin)
                        {
                            moveSpeed = scratchSpeed;
                            if (Mathf.Abs(horizontalDistance) < meleeAttackDistance)
                            {
                                timeNextDecision = Time.time + attackPreDuration;
                                // Flip(horizontalDistance > 0, flipDuration);
                                Flip(horizontalDistance > 0);
                                //触发爪击动画
                                anim.SetTrigger("Scratch");
                                anim.speed = 0.5f;

                                isEventBegin = false;
                                break;
                            }
                            if (Mathf.Abs(horizontalDistance) < rangedAttackDistance)
                            {
                                Debug.Log("bbb");
                                timeNextDecision = Time.time + attackPreDuration;
                                //前突
                                targetPoint = new Vector2(transform.position.x - meleeAttackDistance * devilFacing, groundY);
                                // Flip(horizontalDistance > 0, flipDuration);
                                Flip(horizontalDistance > 0);
                                //触发爪击动画
                                anim.SetTrigger("Scratch");
                                anim.speed = 0.5f;

                                isEventBegin = false;
                                break;
                            }
                            if (Mathf.Abs(horizontalDistance) >= rangedAttackDistance)
                            {
                                //如果在远程范围外就进入冲刺攻击
                                devilState = DevilState.dash;
                                attackState = AttackState.attackPre;
                                isEventBegin = true;
                                break;
                            }
                        }
                        //前摇结束事件
                        if (Time.time > timeNextDecision)
                        {
                            attackState = AttackState.attack;
                            isEventBegin = true;
                        }
                        break;
                    case AttackState.attack:
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackDuration;

                            isEventBegin = false;
                            break;
                        }
                        //攻击期间事件
                        if (Time.time <= timeNextDecision)
                        {
                            MeleeAttack();
                        }
                        //攻击结束事件
                        if (Time.time > timeNextDecision)
                        {
                            attackState = AttackState.attackEnd;

                            isEventBegin = true;
                        }
                        break;
                    case AttackState.attackEnd:
                        //攻击后摇开始事件
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackBackDuration;

                            isEventBegin = false;
                            break;
                        }
                        //攻击后摇结束事件
                        if (Time.time > timeNextDecision)
                        {
                            anim.speed = 1;
                            devilState = DevilState.walk;
                            attackState = AttackState.attackPre;
                            isEventBegin = true;
                        }
                        break;
                }
                break;

            case DevilState.dash:
                switch (attackState)
                { 
                    case AttackState.attackPre:
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackPreDuration;
                            moveSpeed = dashSpeed;
                            anim.SetTrigger("Dash");
                            // Flip(horizontalDistance > 0, flipDuration);
                            Flip(horizontalDistance > 0);

                            isEventBegin = false;
                            break;
                        }
                        if (Time.time > timeNextDecision)
                        {
                            attackState = AttackState.attack;
                            isEventBegin = true;
                        }
                        break;
                    case AttackState.attack:
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackDuration;

                            isEventBegin = false;
                            break;
                        }
                        if (Time.time <= timeNextDecision)
                        {
                            targetPoint = new Vector2(girlHero.transform.position.x, groundY);
                            if (Mathf.Abs(horizontalDistance) <= meleeAttackDistance)
                            {
                                //冲刺完毕进入近身攻击
                                targetPoint = new Vector2(transform.position.x , groundY);
                                attackState = AttackState.attackPre;
                                devilState = DevilState.dashAttack;
                                isEventBegin = true;
                            }
                            break;
                        }
                        //攻击结束事件
                        if (Time.time > timeNextDecision)
                        {
                            attackState = AttackState.attackEnd;

                            isEventBegin = true;
                        }
                        break;
                    case AttackState.attackEnd: 
                        //攻击后摇开始事件
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackBackDuration;

                            isEventBegin = false;
                            break;
                        }
                        //攻击后摇结束事件
                        if (Time.time > timeNextDecision)
                        {
                            devilState = DevilState.walk;
                            attackState = AttackState.attackPre;
                            isEventBegin = true;
                        }
                        break;
                }
                break;

            case DevilState.dashAttack:
                switch (attackState)
                {
                    case AttackState.attackPre:
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackPreDuration;
                            anim.SetTrigger("DashAttack");
                            // Flip(horizontalDistance > 0, flipDuration);
                            Flip(horizontalDistance > 0);

                            isEventBegin = false;
                            break;
                        }
                        if (Time.time > timeNextDecision)
                        {
                            attackState = AttackState.attack;
                            isEventBegin = true;
                        }
                        break;
                    case AttackState.attack:
                        //攻击准备事件
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackDuration;

                            isEventBegin = false;
                            break;
                        }
                        //攻击期间事件
                        if (Time.time <= timeNextDecision)
                        {
                            //持续攻击
                            MeleeAttack();
                        }
                        //攻击结束事件
                        if (Time.time > timeNextDecision)
                        {
                            attackState = AttackState.attackEnd;

                            isEventBegin = true;
                        }
                        break;
                    case AttackState.attackEnd:
                        //攻击后摇开始事件
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackBackDuration;

                            isEventBegin = false;
                            break;
                        }
                        //攻击后摇结束事件
                        if (Time.time > timeNextDecision)
                        {
                            devilState = DevilState.walk;
                            attackState = AttackState.attackPre;
                            isEventBegin = true;
                        }
                        break;
                }
                break;

            case DevilState.impact:
                switch (attackState)
                {
                    case AttackState.attackPre:
                        if (isEventBegin)
                        {
                            anim.speed = 1;
                            moveSpeed = jumpSpeed;
                            targetPoint = new Vector2(transform.position.x, groundY + jumpSpeed * attackPreDuration / 2);
                            impactTimes = 0;
                            //下降时间
                            timeNextDecision = Time.time + attackPreDuration * 3 / 4;
                            anim.SetTrigger("Impact");
                            isRised = true;

                            isEventBegin = false;
                            break;
                        }
                        if (Time.time > timeNextDecision && isRised)
                        {
                            targetPoint = new Vector2(transform.position.x, groundY);
                            timeNextDecision = Time.time + attackPreDuration * 3 / 4;
                            isRised = false;
                            break;
                        }
                        if (Time.time > timeNextDecision && !isRised)
                        {
                            attackState = AttackState.attack;
                            isEventBegin = true;
                        }
                        break;
                    case AttackState.attack:
                        switch (impactTimes)
                        {
                            //三段攻击分别攻击三个范围
                            case 0:
                                if (isEventBegin)
                                {
                                    impactLeftPoint.x = transform.position.x - meleeAttackDistance / 2;
                                    impactLeftPoint.y = groundY + 0.5f;
                                    impactRightPoint.x = transform.position.x + meleeAttackDistance / 2;
                                    impactLeftPoint.y = groundY + 0.5f;
                                    Vector3 tScale = new Vector3(meleeAttackDistance, 1, 1);
                                    var sh = impactParticleSystem.shape;
                                    sh.scale = tScale;
                                    prefab1 = Instantiate(impactParticlePrefab, impactLeftPoint, Quaternion.identity);
                                    prefab2 = Instantiate(impactParticlePrefab, impactRightPoint, Quaternion.identity);

                                    timeNextDecision = Time.time + attackDuration;
                                    isEventBegin = false;
                                    break;
                                }
                                if (Time.time <= timeNextDecision)
                                {
                                    if ((Mathf.Abs(horizontalDistance) < meleeAttackDistance) 
                                    && (Mathf.Abs(verticalDistance) < verticalAttackDistance))
                                    {
                                        Vector2 damageDir;
                                        damageDir = (girlHero.transform.position - transform.position).normalized;
                                        girlHeroHealth.TakeDamage(attackDamage , damageDir);
                                    }
                                    break;
                                }
                                if (Time.time > timeNextDecision)
                                {
                                    Destroy(prefab1);
                                    Destroy(prefab2);
                                    impactTimes += 1;

                                    isEventBegin = true;
                                }
                                break;
                            case 1:
                                if (isEventBegin)
                                {
                                    impactLeftPoint.x = transform.position.x - meleeAttackDistance - (rangedAttackDistance - meleeAttackDistance) / 2;
                                    impactLeftPoint.y = groundY + 0.5f;
                                    impactRightPoint.x = transform.position.x + meleeAttackDistance + (rangedAttackDistance - meleeAttackDistance) / 2;
                                    impactRightPoint.y = groundY + 0.5f;
                                    Vector3 tScale = new Vector3(rangedAttackDistance - meleeAttackDistance, 1, 1);
                                    var sh = impactParticleSystem.shape;
                                    sh.scale = tScale;
                                    prefab1 = Instantiate(impactParticlePrefab, impactLeftPoint, Quaternion.identity);
                                    prefab2 = Instantiate(impactParticlePrefab, impactRightPoint, Quaternion.identity);

                                    timeNextDecision = Time.time + attackDuration;
                                    isEventBegin = false;
                                    break;
                                }
                                if (Time.time <= timeNextDecision)
                                {
                                    if ((Mathf.Abs(horizontalDistance) > meleeAttackDistance) 
                                    && (Mathf.Abs(horizontalDistance) < rangedAttackDistance)
                                    && (Mathf.Abs(verticalDistance) < verticalAttackDistance))
                                    {
                                        Vector2 damageDir;
                                        damageDir = (girlHero.transform.position - transform.position).normalized;
                                        girlHeroHealth.TakeDamage(attackDamage , damageDir);
                                    }
                                    break;
                                }
                                if (Time.time > timeNextDecision)
                                {
                                    Destroy(prefab1);
                                    Destroy(prefab2);
                                    impactTimes += 1;

                                    isEventBegin = true;
                                }
                                break; 
                            case 2:
                                if (isEventBegin)
                                {
                                    impactLeftPoint.x = roomLeftPointX + (roomRightPointX - roomLeftPointX) / 2;
                                    impactLeftPoint.y = groundY + 0.5f;
                                    Vector3 tScale = new Vector3((roomRightPointX - roomLeftPointX), 1, 1);
                                    var sh = impactParticleSystem.shape;
                                    sh.scale = tScale;
                                    prefab1 = Instantiate(impactParticlePrefab, impactLeftPoint, Quaternion.identity);

                                    timeNextDecision = Time.time + attackDuration;
                                    isEventBegin = false;
                                    break;
                                }
                                if (Time.time <= timeNextDecision)
                                {
                                    if ((Mathf.Abs(verticalDistance) < verticalAttackDistance))
                                    {
                                        Vector2 damageDir;
                                        damageDir = (girlHero.transform.position - transform.position).normalized;
                                        girlHeroHealth.TakeDamage(attackDamage , damageDir);
                                    }
                                    break;
                                }
                                if (Time.time > timeNextDecision)
                                {
                                    Destroy(prefab1);
                                    impactTimes += 1;

                                    isEventBegin = true;
                                }
                                break; 
                            case 3:
                                attackState = AttackState.attackEnd;

                                isEventBegin = true;
                                //攻击结束
                                devilState = DevilState.walk;
                                attackState = AttackState.attackPre;
                                isEventBegin = true;
                                break;
                        }
                        break;
                    case AttackState.attackEnd:
                        //攻击后摇开始事件
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackBackDuration;

                            isEventBegin = false;
                            break;
                        }
                        //攻击后摇结束事件
                        if (Time.time > timeNextDecision)
                        {
                            devilState = DevilState.walk;
                            attackState = AttackState.attackPre;
                            isEventBegin = true;
                        }
                        break;
                }
                break;

            case DevilState.storm:
                switch (attackState)
                {
                    case AttackState.attackPre:
                        if (isEventBegin)
                        {
                            targetPoint = new Vector2 (transform.position.x, groundY);
                            timeNextDecision = Time.time + attackPreDuration;
                            anim.SetTrigger("Storm");
                            // Flip(horizontalDistance > 0, flipDuration);
                            Flip(horizontalDistance > 0);

                            isEventBegin = false;
                            break;
                        }
                        if (Time.time > timeNextDecision)
                        {
                            attackState = AttackState.attack;
                            isEventBegin = true;
                        }
                        break;
                    case AttackState.attack:
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackDuration;

                            isEventBegin = false;
                            break;
                        }
                        //攻击结束事件
                        if (Time.time > timeNextDecision)
                        {
                            //在身前生成风暴
                            Quaternion bulletRotation = transform.rotation;
                            bulletRotation.y = (transform.rotation.y < -0.5) ? 180 : 0;
                            Instantiate(stormbullet, transform.position + transform.right, bulletRotation);

                            attackState = AttackState.attackEnd;
                            isEventBegin = true;
                        }
                        break;
                    case AttackState.attackEnd:
                        //攻击后摇开始事件
                        if (isEventBegin)
                        {
                            timeNextDecision = Time.time + attackBackDuration;

                            isEventBegin = false;
                            break;
                        }
                        //攻击后摇结束事件
                        if (Time.time > timeNextDecision)
                        {
                            devilState = DevilState.walk;
                            attackState = AttackState.attackPre;
                            isEventBegin = true;
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

    protected override void OnTriggerStay2D(Collider2D other)
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
        verticalDistance = girlHero.transform.position.y - transform.position.y;
        horizontalDistance = girlHero.transform.position.x - transform.position.x;

        // devilFacing = (int)transform.rotation.y * 2 + 1;
        // girlHeroFacing = (int)girlHero.transform.rotation.y * 2 + 1;
        if (transform.rotation.y < -0.5)
        {
            devilFacing = 1;
        }
        else
        {
            devilFacing = -1;
        }

        attackDamage = devilAttacks[(int) devilState].attackDamage;
        attackPreDuration = devilAttacks[(int) devilState].attackPreDuration;
        attackDuration = devilAttacks[(int) devilState].attackDuration;
        attackBackDuration = devilAttacks[(int) devilState].attackBackDuration;
    }

    //进行攻击判定并且进入攻击后摇阶段
    void MeleeAttack()
    {
        if (((horizontalDistance / devilFacing) < 0) 
            && (Mathf.Abs(horizontalDistance)< meleeAttackDistance) 
            && (Mathf.Abs(verticalDistance) < verticalAttackDistance))
        {
            Vector2 damageDir;
            damageDir = (girlHero.transform.position - transform.position).normalized;
            girlHeroHealth.TakeDamage(attackDamage , damageDir);
        }
    }

    public void BeginDodge(int attackDamage, Vector2 damageDir)
    {
        DevilStateUpdate();
        dodgeRandom = Random.value;
        if (attackState != AttackState.attack && devilState != DevilState.impact)
        {
            dodgeRandom = Random.value;
            if (dodgeRandom <= dodgeRate)
            {
                moveSpeed = dodgeSpeed;
                anim.SetTrigger("Dodge");
                devilState = DevilState.dodge;
                health.invincible = true;
                
                targetPoint = new Vector2(transform.position.x - moveSpeed * devilFacing * dodgeDuration , groundY);
            }
            else
            {
                health.TakeDamage(attackDamage, damageDir);
            }
        }
        else
        {
            health.TakeDamage(attackDamage, damageDir);
        }
    }

    private void Flip(bool right, float flipDuration)
    {
        if (facing == Facing.Left)
        {
            right = !right;
        }
        
        float next = right ? 0 : 180;
        if (transform.rotation.eulerAngles.y != next)
        {
            isFliped = true;
            StartCoroutine(FlipSlerp(next));
        }
    }

    IEnumerator FlipSlerp(float next)
    {
        float flipTime = Time.time + flipDuration;
        float u = (flipTime - Time.time) / flipDuration;
        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.Euler(0, next, 0);
        while (u > 0)
        {
            transform.rotation = Quaternion.Slerp(start, end, 1-u);
            u = (flipTime - Time.time) / flipDuration;
            yield return null;
        }
        isFliped = false;
    }
}