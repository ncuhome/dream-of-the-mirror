using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoAttack : MonoBehaviour
{
    public int attackDamage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Health>() != null)
        {
            if (this.tag == "HeroBo")
            {
                if (other.gameObject.tag != "Hero" && other.gameObject.tag != "HeroBo")
                {
                    other.GetComponent<Health>().TakeDamage(attackDamage);
                    Destroy(this.gameObject);
                }
            }
            else if (this.tag == "EnemyBo")
            {
                if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "EnemyBo")
                {
                    other.GetComponent<Health>().TakeDamage(attackDamage);
                    Destroy(this.gameObject);
                }
            }
        }
        else
        {
            Destroy(this.gameObject);
        }  
    }
}
