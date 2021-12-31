using UnityEngine;

public class BoAttack : MonoBehaviour
{
    public int attackDamage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            Destroy(this.gameObject);
        }

        if (this.tag == "Player")
        {
            if (other.tag != "Enemy")
            {
                return;
            }
            if (other.GetComponent<Health>() != null)
            {
                other.GetComponent<Health>().TakeDamage(attackDamage);
                Destroy(this.gameObject);
            }
        }

        if (this.tag == "Enemy")
        {
            if (other.tag != "Player")
            {
                return;
            }
            if (other.GetComponent<Health>() != null)
            {
                other.GetComponent<Health>().TakeDamage(attackDamage);
                Destroy(this.gameObject);
            }
        } 
    }
}
