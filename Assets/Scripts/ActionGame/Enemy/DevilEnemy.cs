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
    idle,
    scratch1 , scratch2 , scratch3 , scratch4,
    dash , dashAttack,
    impact,
    storm
}

public enum AttackState
{
    idle,
    attackPre,
    attack
}

public class DevilEnemy : Enemy
{
    public float meleeAttackDistance , rangedAttackDistance;
    public float moveSpeed;
    public Health _health,girlHeroHealth;
    public EnemySlider enemySlider;
    public float attackPreDuration,attackBackDuration;
    public int attackDamage;
    public DevilState devilState;
    public Vector2 targetPoint;
    public Vector2 damageDir;
    public AttackState attackState;
    public float scratchSpeed,dashSpeed,idleSpeed;
    public float horizontalDistance,hitDistance;
    private float attackTime = 0f;
    
    public DevilAttack[] devilAttacks;


    protected override void Start()
    {
        base.Start();
        
        _health = GetComponent<Health>();
        enemySlider = GetComponent<EnemySlider>();
        girlHeroHealth = PlayerManager.instance.girlHero.GetComponent<Health>();
        attackState = AttackState.idle;

    }


    void FixedUpdate()
    {
        if ((!enemyAttackConsciousness.attackConsciousness) && (devilState == DevilState.idle))
        {
            return;
        }
        if (attackState != AttackState.attack)
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, targetPoint, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
    }

    protected override void Update()
    {
        base.Update();

        if ((!enemyAttackConsciousness.attackConsciousness) && (devilState == DevilState.idle))
        {
            return;
        }
        enemySlider.FixSlider();
        
        DevilStateUpdate();
        horizontalDistance = girlHero.transform.position.x - transform.position.x;
        switch (devilState)
        {
            case DevilState.idle:
                moveSpeed = idleSpeed;
                targetPoint = new Vector2(girlHero.transform.position.x,girlHero.transform.position.y);
                if ((Mathf.Abs( horizontalDistance ) < meleeAttackDistance))
                {
                    devilState = DevilState.scratch1;
                }
                break;
            case DevilState.scratch1:
                switch (attackState)
                {
                    case AttackState.idle:
                        Flip(horizontalDistance > 0);
                        AttackPre();
                        break;
                    case AttackState.attackPre:
                        if(Time.time > attackTime)
                        {
                            anim.SetTrigger("Scratch");
                            MeleeAttack();
                        }
                        break;
                    case AttackState.attack:
                        if (Time.time > attackTime)
                        {
                            Flip(horizontalDistance > 0);
                            moveSpeed = scratchSpeed;
                            targetPoint = new Vector2(transform.position.x + meleeAttackDistance/2f*((int)facing),transform.position.y);
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
                        if (transform.position.x == targetPoint.x)
                        {
                            AttackPre();
                        }
                        break;
                    case AttackState.attackPre:
                        if (Time.time > attackTime)
                        {
                            anim.SetTrigger("Scratch");
                            MeleeAttack();
                        }
                        break;
                    case AttackState.attack:
                        if (Time.time > attackTime)
                        {
                            targetPoint = new Vector2(transform.position.x + meleeAttackDistance/2f*((int)facing),transform.position.y);
                            devilState = DevilState.scratch3;
                            attackState = AttackState.idle;
                        }
                        break;
                }
                break;
            case DevilState.scratch3:
                switch (attackState)
                {
                    case AttackState.idle:
                        if (transform.position.x == targetPoint.x)
                        {
                            AttackPre();
                        }
                        break;
                    case AttackState.attackPre:
                        if (Time.time > attackTime)
                        {
                            anim.SetTrigger("Scratch");
                            MeleeAttack();
                        }
                        break;
                    case AttackState.attack:
                        if (Time.time > attackTime)
                        {
                            attackState = AttackState.idle;
                            if (Mathf.Abs(horizontalDistance) < rangedAttackDistance)
                            {
                                devilState = DevilState.scratch4;
                            } else
                            {
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
                        if (Mathf.Abs(horizontalDistance) > meleeAttackDistance)
                        {
                            moveSpeed = scratchSpeed;
                            targetPoint = new Vector2(transform.position.x + meleeAttackDistance/2f*((int)facing),transform.position.y);
                        }
                        if (transform.position.x == targetPoint.x)
                        {
                            Flip(horizontalDistance > 0);
                            AttackPre();
                        }
                        break;
                    case AttackState.attackPre:
                        if (Time.time > attackTime)
                        {
                            anim.SetTrigger("Scratch");
                            MeleeAttack();
                        }
                        break;
                    case AttackState.attack:
                        if (Time.time > attackTime)
                        {
                            devilState = DevilState.idle;
                            attackState = AttackState.idle;
                        }
                        break;
                }
                break;
            case DevilState.dash:
                switch (attackState)
                { 
                    case AttackState.idle:
                        targetPoint = new Vector2(girlHero.transform.position.x,girlHero.transform.position.y);
                        AttackPre();
                        break;
                    case AttackState.attackPre:
                        if (Time.time > attackTime)
                        {                       
                            anim.SetTrigger("Dash");
                            attackTime = Time.time + attackBackDuration;
                            attackState = AttackState.attack;
                        }
                        break;
                    case AttackState.attack: 
                        if (transform.position.x == targetPoint.x)
                        {
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
                        AttackPre();                        
                        break;
                    case AttackState.attackPre:
                        if (Time.time > attackTime)
                        {   
                            Flip(horizontalDistance > 0);
                            anim.SetTrigger("DashAttack");
                            MeleeAttack();
                        }
                        break;
                    case AttackState.attack:
                        if (Time.time > attackTime)
                        {
                            attackState = AttackState.idle;
                            devilState = DevilState.idle;
                        }
                        break;
                }
                break;
        }

        
        
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Health>() == null)
        {
            return;
        }
        if ((other.tag == "Player") && (devilState == DevilState.dash))
        {
            Vector2 damageDir;
            damageDir = (other.transform.position - transform.position).normalized;
            other.GetComponent<Health>().TakeDamage(attackDamage, damageDir);
        }
    }



    void DevilStateUpdate()
    {
        attackDamage = devilAttacks[(int) devilState].attackDamage;
        attackPreDuration = devilAttacks[(int) devilState].attackPreDuration;
        attackBackDuration = devilAttacks[(int) devilState].attackBackDuration;
    }

    void AttackPre()
    {
        attackState = AttackState.attackPre;
        attackTime = Time.time + attackPreDuration;
    }

    void MeleeAttack()
    {
        attackTime = Time.time + attackBackDuration;
        attackState = AttackState.attack;
        if (Mathf.Abs(horizontalDistance) < meleeAttackDistance)
        {
            Vector2 damageDir;
            damageDir = (girlHero.transform.position - transform.position).normalized;
            girlHeroHealth.TakeDamage(attackDamage , damageDir);
        }
    }





}
