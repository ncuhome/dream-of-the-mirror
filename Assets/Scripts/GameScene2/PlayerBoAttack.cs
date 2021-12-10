using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoAttack : MonoBehaviour
{
    public int attackDamage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Hero")
        {
            if (other.gameObject.GetComponent<PlayerHealth>() != null)
            {
                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            }
            Destroy(this.gameObject);
        }    
    }
}
