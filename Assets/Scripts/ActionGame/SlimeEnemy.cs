using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : Enemy
{
    //这个没有enemyAttackConsciousness
    [Header("Rabbit: ")]
    public float attackConsciousnessRange;
    public float attackRange = 3f;
    public float speedUpRange = 5f;
    public float speedRate = 3f;
    public float speed;

    private float heroDistance;

    protected override void Update()
    {
        Vector2 dir;
        dir = girlHero.transform.position - transform.position;
        heroDistance = dir.magnitude;

        if (heroDistance < attackConsciousnessRange)
        {
            //判断贴图方向
            if ((girlHero.transform.position.x - transform.position.x) < 0)
            {
                Flip(false);
            }
            else
            {
                Flip(true);
            }

            float tSpeed = speed;
            if (heroDistance < speedUpRange)
            {
                tSpeed *= speedRate;
            }
            Vector2 target = new Vector2(girlHero.transform.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, tSpeed * Time.deltaTime);
            rb.MovePosition(newPos);
            if (heroDistance < attackRange)
            {
                anim.SetTrigger("Attack");
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other) 
    {
        if (curAnimIs("Slime_Attack"))
        {
            if (other.tag == "Hero")
            {
                other.GetComponent<Health>().TakeDamage(closeDamage);
            }
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Health>() != null && curAnimIs("Slime_Attack"))
        {
            if (Time.time > other.gameObject.GetComponent<Health>().nextInvincibleTime)
            {
                if (other.tag == "Hero")
                {
                    other.GetComponent<Health>().TakeDamage(closeDamage);
                }
            }   
        } 
    }
}
