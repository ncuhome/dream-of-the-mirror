using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlHeroBulletPhysicsComponent : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            Destroy(this.gameObject);
        }

        if (other.tag != "Enemy")
        {
            return;
        }
        Vector2 damageDir;
        damageDir = (other.transform.position - transform.position).normalized;
        if (other.GetComponent<Health>() != null)
        {
            other.GetComponent<Health>().TakeDamage(new Damage(damage, damageDir, other.transform.position,Damage.DamageType.RemoteAttack));
            Destroy(this.gameObject);
        }
        if (other.GetComponent<HurtTriggerDetection>() != null)
        {
            other.GetComponent<HurtTriggerDetection>().GetHurt(new Damage(damage, damageDir, other.transform.position,Damage.DamageType.RemoteAttack));
            Destroy(this.gameObject);
        }
    }
}
