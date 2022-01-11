using UnityEngine;

public class EnemyAttackConsciousness : MonoBehaviour
{
    public bool attackConsciousness = false;
    public float attackConsciousnessRange;

    public GirlHero girlHero;

    public float heroDistance;

    void Awake()
    {
        girlHero = GameObject.FindGameObjectWithTag("Player").GetComponent<GirlHero>();
    }
    
    void Update()
    { 
        Vector2 dir;
        dir = girlHero.transform.position - transform.position;
        heroDistance = dir.magnitude;

        attackConsciousness = (heroDistance <= attackConsciousnessRange);
    }
}
