using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitEnemy : Enemy
{
    //这个没有enemyAttackConsciousness
    [Header("Rabbit: ")]
    public float attackConsciousnessRange;
    public float speed;
    public float speedRate = 2;

    private float heroDistance;

    protected override void Update()
    {
        Vector2 dir;
        dir = girlHero.transform.position - transform.position;
        heroDistance = dir.magnitude;

        if (heroDistance < attackConsciousnessRange)
        {
            //判断贴图方向（我裂开了哇。。。这个不知道为啥不能用Flip）
            if ((girlHero.transform.position.x - transform.position.x) > 0)
            {
                Vector2 tScale = transform.localScale;
                tScale.x = -1;
                transform.localScale = tScale;
                anim.SetBool("RollLeft", false);
                anim.SetBool("RollRight", true);
            }
            else
            {
                Vector2 tScale = transform.localScale;
                tScale.x = 1;
                transform.localScale = tScale;
                anim.SetBool("RollLeft", true);
                anim.SetBool("RollRight", false);
            }

            float tSpeed = speed;
            if (curAnimIs("Rabbit_RollLeft") || curAnimIs("Rabbit_RollRight"))
            {
                tSpeed *= speedRate;
            }
            Vector2 target = new Vector2(girlHero.transform.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, tSpeed * Time.deltaTime);
            rb.MovePosition(newPos);
        }
    }

    public void OnTriggerEnter2D(Collider2D other) 
    {
        if (curAnimIs("Rabbit_RollLeft") || curAnimIs("Rabbit_RollRight"))
        {
            if (other.tag == "Hero")
            {
                other.GetComponent<Health>().TakeDamage(closeDamage);
            }
        }
    }
}
