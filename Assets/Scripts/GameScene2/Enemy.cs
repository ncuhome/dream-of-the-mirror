using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy: ")]
    public string ID;
    public int closeDamage;

    public EnemyAttackConsciousness enemyAttackConsciousness;
    public GirlHero girlHero;
    public Rigidbody2D rb;
    protected Animator anim;

    protected Vector2 dir = Vector2.zero;

    //暂时提供一下。。。
    protected bool curAnimIs(string animName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }

    protected virtual void Start()
    {
        ID = gameObject.name;
        girlHero = GameObject.Find("GirlHero").GetComponent<GirlHero>();

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (GetComponent<EnemyAttackConsciousness>() != null)
        {
            enemyAttackConsciousness = GetComponent<EnemyAttackConsciousness>();
        }
    }

    protected virtual void Update()
    {
        dir = girlHero.transform.position - transform.position;
        dir.Normalize();

        if (enemyAttackConsciousness == null)
        {
            return;
        }

        if (enemyAttackConsciousness.heroDistance > enemyAttackConsciousness.attackConsciousnessRange)
        {
            enemyAttackConsciousness.attackConsciousness = false;
            return;
        }
        else
        {
            enemyAttackConsciousness.FixSlider();
            enemyAttackConsciousness.attackConsciousness = true;
        }
    }

    public void Flip(bool right)
    {
        float next = right ? 0 : 180;
        if (transform.rotation.eulerAngles.y != next)
        {
            transform.rotation = Quaternion.Euler(0, next, 0);
        }
    }
}
