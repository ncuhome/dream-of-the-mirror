using UnityEngine;

public class EnemyAttackConsciousness : MonoBehaviour
{
    public bool attackConsciousness = false;
    public float attackConsciousnessRange;

    public GirlHero girlHero;

    [HideInInspector]
    public float heroDistance;

    void Start()
    {
        girlHero = PlayerManager.instance.girlHero;
    }
    
    void Update()
    { 
        Vector2 dir;
        dir = girlHero.transform.position - transform.position;
        heroDistance = dir.magnitude;

        attackConsciousness = (heroDistance <= attackConsciousnessRange);
    }
}
