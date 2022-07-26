using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilPhysicsComponent : MonoBehaviour
{
    public GameObject downwardAttackParticlePrefab;

    private ParticleSystem downwardAttackParticleSystem;
    private EnemyAttackConsciousness enemyAttackConsciousness;
    private DevilHealth devilHealth;
    // private ColliderController colliderController;
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;

    private float groundY;
    private bool rising = true;
    private float riseHeight = 0;
    private int attackDamage;
    private int attackMask;
    GameObject prefab1, prefab2;

    public Rigidbody2D Rb
    {
        get
        {
            return rb;
        }
    }

    void Start()
    {
        groundY = transform.position.y;
        rb = GetComponent<Rigidbody2D>();
        devilHealth = GetComponent<DevilHealth>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        capsuleCollider = GetComponent<CapsuleCollider2D>();  
        // colliderController = GetComponent<ColliderController>();
        enemyAttackConsciousness = GetComponent<EnemyAttackConsciousness>();
        downwardAttackParticleSystem = downwardAttackParticlePrefab.GetComponent<ParticleSystem>();
    }

    // void Update()
    // {
    //     if (colliderController.HurtCollider != null)
    //     {
    //         Collider2D col = colliderController.HurtCollider;
    //         Vector2 damageDir;
    //         damageDir = ((Vector2)col.transform.position - (Vector2)transform.position).normalized;
    //         col.GetComponent<Health>().TakeDamage(new Damage(1, damageDir, col.transform.position,Damage.DamageType.MeleeAttack));
    //     }
    // }

    public void SetAttack(int damage, LayerMask mask)
    {
        attackDamage = damage;
        attackMask = mask;
    }

    public void EnemyHit(Collider2D other)
    {
        Vector2 damageDir;
        damageDir = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
        if (other.GetComponent<Health>() != null)
        {
            other.GetComponent<Health>().TakeDamage(new Damage(attackDamage, damageDir, other.transform.position, Damage.DamageType.MeleeAttack));
        }
        if (other.GetComponent<HurtTriggerDetection>() != null)
        {
            other.GetComponent<HurtTriggerDetection>().GetHurt(new Damage(attackDamage, damageDir, other.transform.position, Damage.DamageType.MeleeAttack));
        }
    }

    public void EnemyHurt(Damage damage)
    {
        devilHealth.TakeDamage(damage);
    }

    public void DownwardJump(int index, float jumpHeight, float jumpDuration)
    {
        float height = 2 * jumpHeight / jumpDuration * Time.fixedDeltaTime;
        if (rising)
        {
            if (riseHeight < jumpHeight)
            {
                rb.position = new Vector2(transform.position.x, groundY + height);
            }
            else
            {
                rising = false;
            }
        }
        else
        {
            if (riseHeight > groundY)
            {
                rb.position = new Vector2(transform.position.x, groundY - height);
            }
            else
            {
                rising = true;
            }
        }
    }

    public void DownwardAttack(int damage, int mask, Vector2 offset)
    {
        Destroy(prefab1);
        Destroy(prefab2);
        Vector2 downwardAttackLeftPoint, downwardAttackRightPoint;
        downwardAttackLeftPoint.x = transform.position.x - (offset.y + offset.x) / 2;
        downwardAttackLeftPoint.y = groundY - 0.75f;
        downwardAttackRightPoint.x = transform.position.x + (offset.y + offset.x) / 2;
        downwardAttackRightPoint.y = groundY - 0.75f;
        Vector3 tScale = new Vector3(offset.y - offset.x, 1, 1);
        var sh = downwardAttackParticleSystem.shape;
        sh.scale = tScale;
        // GameObject prefab1, prefab2;
        prefab1 = Instantiate(downwardAttackParticlePrefab, downwardAttackLeftPoint, Quaternion.identity);
        prefab1.GetComponent<ParticleHitTriggerDetection>().Physics = this;
        prefab2 = Instantiate(downwardAttackParticlePrefab, downwardAttackRightPoint, Quaternion.identity);
        prefab2.GetComponent<ParticleHitTriggerDetection>().Physics = this;
        // if (colliderController.HitCollider != null)
        // {
        //     Vector2 damageDir;
        //     damageDir = ((Vector2)colliderController.HitCollider.transform.position - (Vector2)transform.position).normalized;
        //     colliderController.HitCollider.GetComponent<Health>().TakeDamage(new Damage(damage, damageDir, colliderController.HitCollider.transform.position,Damage.DamageType.MeleeAttack));
        // }
        // else
        // {
            if (Mathf.Abs(enemyAttackConsciousness.HeroDistance().x - transform.position.x) > offset.x
            && Mathf.Abs(enemyAttackConsciousness.HeroDistance().x - transform.position.x) < offset.y
            && Mathf.Abs(enemyAttackConsciousness.HeroDistance().y - transform.position.y) < 0.5f)
            {
                Vector2 damageDir;
                damageDir = Vector2.up;
                // colliderController.HitCollider.GetComponent<Health>().TakeDamage(new Damage(damage, damageDir, colliderController.HitCollider.transform.position,Damage.DamageType.MeleeAttack));
            }
        // }
    }

    // public void HorizontalMove(float moveSpeed)
    // {       
    //     rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    // }

    public void RunMove(int horizontal, float moveSpeed)
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

    // public void ShootMove(int horizontal, float moveSpeed)
    // {
    //     rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
    // }

    public void ForwardMove(float moveSpeed)
    {
        if (transform.rotation.eulerAngles.y < 5)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2((-1) * moveSpeed, rb.velocity.y);
        }
    }

    /// <summary>
    /// 会根据贴图方向通过传入的水平移动方向参数进行翻转
    /// </summary>
    /// <param name="right">正在向x轴正方向移动</param>
    public bool Flip(bool left)
    {        
        float next = left ? 0 : 180;
        if (transform.rotation.eulerAngles.y != next)
        {
            transform.rotation = Quaternion.Euler(0, next, 0);
            return true;
        }
        return false;
    }
    
    public void ResetSpeed()
    {
        rb.velocity = Vector2.zero;
    }
}
