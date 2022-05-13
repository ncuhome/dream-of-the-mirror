using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBulletPhysicsComponent : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            Destroy(this.gameObject);
        }

        if (other.tag != "Player")
        {
            return;
        }
        if (other.GetComponent<Health>() != null)
        {
            Vector2 damageDir;
            damageDir = (other.transform.position - transform.position).normalized;
            other.GetComponent<Health>().TakeDamage(new Damage(damage, damageDir, other.transform.position,Damage.DamageType.RemoteAttack));
            Destroy(this.gameObject);
        }
    } 
}
