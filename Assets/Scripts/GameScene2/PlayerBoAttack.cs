using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoAttack : MonoBehaviour
{
    public int attackDamage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerHealth>() != null)
        {
            //避免同类自残
            if (other.gameObject.tag != gameObject.tag)
            {
                other.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
                Destroy(this.gameObject); 
            }
        }
        else
        {
            Destroy(this.gameObject);  
        }  
    }
}
