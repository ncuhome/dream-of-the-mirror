using UnityEngine;

public class EnemyAttackConsciousness : MonoBehaviour
{
    [HideInInspector] public string enemyName;
    public float attackConsciousnessRange;

    private GameObject girlHero;
    private bool isFirstMeetHero = true;

    void Start()
    {
        girlHero = PlayerManager.instance.girlHero;
    }

    public bool CheckAttackConsciousness()
    {
        float heroDistance = (girlHero.transform.position - transform.position).magnitude;
        if (heroDistance <= attackConsciousnessRange)
        {
            if (isFirstMeetHero)
            {
                TextHint.instance.ShowHint(enemyName, Color.red);
                isFirstMeetHero = false;
            }
            return true;
        }
        return false;
    }

    public int WalkDir()
    {   
        return NormalizeInt(girlHero.transform.position.x - transform.position.x);
    }

    private int NormalizeInt(float x)
    {
        if (x > 0)
        {
            return 1;
        }
        if (x < 0)
        {
            return -1;
        }
        return 0;
    }

    public Vector2 HeroDistance()
    {
        return (girlHero.transform.position - transform.position);
    }
}
