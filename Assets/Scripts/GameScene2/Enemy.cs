using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy: ")]
    public string ID;
    public int closeDamage;

    public EnemyAttackConsciousness enemyAttackConsciousness;
    protected GirlHero girlHero;
    protected Animator anim;
    protected Rigidbody2D rb;

    protected Vector2 dir = Vector2.zero;

    protected virtual void Start()
    {
        ID = gameObject.name;
        girlHero = GameObject.Find("GirlHero").GetComponent<GirlHero>();

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyAttackConsciousness = GetComponent<EnemyAttackConsciousness>();
    }

    protected virtual void Update()
    {
        dir = girlHero.transform.position - transform.position;
        dir.Normalize();

        if (!enemyAttackConsciousness.attackConsciousness)
        {
            return;
        }
    }

    protected void Flip(bool right)
    {
        float next = right ? 0 : 180;
        if (transform.rotation.eulerAngles.y != next)
        {
            transform.rotation = Quaternion.Euler(0, next, 0);
        }
    }
}
