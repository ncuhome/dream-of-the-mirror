using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlider : MonoBehaviour
{
    public GirlHero girlHero;
    public SliderController sliderController;
    public Health _health;
    public SpriteRenderer sRend;
    public EnemyAttackConsciousness enemyAttackConsciousness;

    void Start()
    {
        girlHero = PlayerManager.instance.girlHero;
        sRend = GetComponent<SpriteRenderer>();
        enemyAttackConsciousness = GetComponent<EnemyAttackConsciousness>();
        sliderController = SliderControllerManager.instance.sliderController.GetComponent<SliderController>();
        if (GetComponent<Health>() != null)
        {
            _health = GetComponent<Health>();
        }
        else
        {
            Debug.Log("It Should have Health");
        }
    }

    void FixedUpdate()
    {
        if (enemyAttackConsciousness.heroDistance > enemyAttackConsciousness.attackConsciousnessRange)
        {
            return;
        }
        FixSlider();
    }

    public void FixSlider()
    {
        sliderController.maxHealth = _health.maxHealth;
        sliderController.health = _health.currentHealth;
        sliderController.sprite = sRend.sprite;
    }
}
