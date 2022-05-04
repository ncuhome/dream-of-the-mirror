using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GirlHeroPhysicsComponent : MonoBehaviour
{
    //最大平台跳跃缓冲时间
    // public float mayJump = 0.1f;
    //摩擦系数
    public float groundDefaultFriction = 0.4f;
    public PhysicsMaterial2D groundPhysicsMaterial;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    // private GirlHeroHealth health;
    private bool grounded = false;

    public CapsuleCollider2D CapsuleCollider
    {
        get
        {
            return capsuleCollider;
        }
    }

    public bool IsGrounded
    {
        get
        {
            return grounded;
        }
    }

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
        // health = GetComponent<GirlHeroHealth>();   
    }

    public void PhysicsUpdate()
    {
        grounded = CheckGrounded();
        if (grounded)
        {
            UseDefaultFriction();
            // HeroineState.jumping.RemainJumpCount = HeroineState.jumping.maxJumpCount;
            HeroineState.rolling.RemainRollCount = HeroineState.rolling.maxRollCount;
        }
        else
        {
            UseNoFriction();
        }
    }

    public void UseDefaultFriction()
    {
        groundPhysicsMaterial.friction = groundDefaultFriction;
    }

    public void UseNoFriction()
    {
        groundPhysicsMaterial.friction = 0;
    }

    private bool CheckGrounded()
    {
        Vector2 startPos = transform.position;
        startPos += Vector2.down * (capsuleCollider.bounds.extents.y + 0.05f);
        RaycastHit2D hitData = Physics2D.Raycast(startPos, Vector3.back * (-1), 200, 1<<8);
        if (hitData.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddJumpForce(float jumpForce)
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void HorizontalMove(int horizontal, float moveSpeed)
    {
        if (horizontal == 0)
        {
            // Debug.Log(rb.velocity.y);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            // rb.AddForce(horizontal * 10 * Vector2.left, ForceMode2D.Impulse);
            Flip(horizontal > 0);            
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }
    }

    public void SwordAttackMove(int horizontal, float moveSpeed)
    {
        if (horizontal == 0)
        {
            // Debug.Log(rb.velocity.y);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            // Debug.Log(horizontal);        
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }
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
                //避免同类自残
                if (col.gameObject.tag == "Player")
                {
                    continue;
                }
                if (col.GetComponent<Health>() != null)
                {
                    Vector2 damageDir;
                    damageDir = ((Vector2)col.transform.position - (Vector2)transform.position).normalized;
                    col.GetComponent<Health>().TakeDamage(new Damage(attackDamage, damageDir, col.transform.position));
                }
            }
        }
    }

    // public void RepelMove(Vector2 repelDir, float repelSpeed)
    // {
    //     Vector2 newPos = Vector2.MoveTowards(rb.position, repelDir, repelSpeed * Time.fixedDeltaTime);
    //     rb.MovePosition(newPos);
    // }
    
    public void RepelMove(Vector2 repelDir, float repelForce)
    {
        rb.AddForce(repelDir * repelForce, ForceMode2D.Impulse);
    }

    public void RollMove(float rollSpeed)
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, rb.position + (Vector2)transform.right, rollSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    /// <summary>
    /// 会根据贴图方向通过传入的水平移动方向参数进行翻转
    /// </summary>
    /// <param name="right">正在向x轴正方向移动</param>
    public bool Flip(bool right)
    {        
        float next = right ? 0 : 180;
        if (transform.rotation.eulerAngles.y != next)
        {
            transform.rotation = Quaternion.Euler(0, next, 0);
            return true;
        }
        return false;
    }
}
