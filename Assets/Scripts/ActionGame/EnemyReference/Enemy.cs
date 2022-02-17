using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy: ")]
    public int closeDamage;
    [Header("贴图默认朝向")]
    public Facing facing;

    //附在敌人上的攻击范围脚本
    public EnemyAttackConsciousness enemyAttackConsciousness;
    //玩家脚本
    public GirlHero girlHero;
    public Health health;

    public Rigidbody2D rb;
    protected Animator anim;

    protected bool curAnimIs(string animName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }

    protected virtual void Start()
    {
        girlHero = GameObject.FindGameObjectWithTag("Player").GetComponent<GirlHero>();

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        enemyAttackConsciousness = GetComponent<EnemyAttackConsciousness>();
    }

    protected virtual void Update()
    {
        //时刻获取碰撞状态
        rb.WakeUp();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Health>() == null)
        {
            return;
        }
        if (other.tag == "Player")
        {
            Vector2 damageDir;
            damageDir = (other.transform.position - transform.position).normalized;
            other.GetComponent<Health>().TakeDamage(closeDamage, damageDir);
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Health>() == null)
        {
             return;
        }
        if (other.tag != "Player")
        {
            return;
        }
        if (!other.gameObject.GetComponent<Health>().invincible)
        {
            Vector2 damageDir;
            damageDir = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
            damageDir.x = (damageDir.x > 0) ? 1 : -1;
            other.GetComponent<Health>().TakeDamage(closeDamage, damageDir);
        }
    }

    /// <summary>
    /// 会根据贴图方向通过传入的水平移动方向参数进行翻转
    /// </summary>
    /// <param name="right">正在向x轴正方向移动</param>
    public void Flip(bool right)
    {
        if (facing == Facing.Left)
        {
            right = !right;
        }
        
        float next = right ? 0 : 180;
        if (transform.rotation.eulerAngles.y != next)
        {
            transform.rotation = Quaternion.Euler(0, next, 0);
        }
    }

    protected float GetNextTime(float offset)
    {
        return Time.time + offset;
    }
}