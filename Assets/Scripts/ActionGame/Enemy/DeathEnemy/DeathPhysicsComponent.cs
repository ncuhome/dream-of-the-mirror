using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPhysicsComponent : MonoBehaviour
{
    private EnemyAttackConsciousness enemyAttackConsciousness;
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;

    public Rigidbody2D Rb
    {
        get
        {
            return rb;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        capsuleCollider = GetComponent<CapsuleCollider2D>();  
        enemyAttackConsciousness = GetComponent<EnemyAttackConsciousness>();
    }

    public void HorizontalMove(int horizontal, float moveSpeed)
    {
        if (horizontal == 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            Flip(horizontal > 0);            
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }
    }

    public void RepelMove(Vector2 repelDir, float repelSpeed)
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, repelDir, repelSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    public void SwordAttack(Vector2 attackOffset, float attackRadiu, int attackMask, int attackDamage)
    {
        Vector2 pos = transform.position;
        pos += (Vector2)transform.right * attackOffset.x;
        pos += (Vector2)transform.up * attackOffset.y;

        Collider2D [] colInfo = Physics2D.OverlapCircleAll(pos, attackRadiu, attackMask);
        if (colInfo.Length != 0)
        {
            foreach (Collider2D col in colInfo)
            {
                if (col.gameObject.tag == "Player")
                {
                    Vector2 damageDir;
                    damageDir = ((Vector2)col.transform.position - (Vector2)transform.position).normalized;
                    col.GetComponent<Health>().TakeDamage(new Damage(attackDamage, damageDir, col.transform.position));
                }
            }
        }
    }

    /// <summary>
    /// 会根据贴图方向通过传入的水平移动方向参数进行翻转
    /// </summary>
    /// <param name="right">正在向x轴正方向移动</param>
    public bool Flip(bool right)
    {        
        float next = right ? 180 : 0;
        if (transform.rotation.eulerAngles.y != next)
        {
            transform.rotation = Quaternion.Euler(0, next, 0);
            return true;
        }
        return false;
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.tag == "Player")
    //     {
    //         Vector2 damageDir;
    //         damageDir = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
    //         other.GetComponent<Health>().TakeDamage(new Damage(closeDamage, damageDir, other.transform.position));
    //     }    
    // }
}
