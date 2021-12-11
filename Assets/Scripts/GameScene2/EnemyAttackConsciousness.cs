using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackConsciousness : MonoBehaviour
{
    public bool attackConsciousness = false;
    public float attackConsciousnessRange;

    public GirlHero girlHero;
    public SliderController sliderController;
    public Health playerHealth;
    public SpriteRenderer sRend;

    public float heroDistance;

    void Awake()
    {
        sliderController = GameObject.Find("SliderController").GetComponent<SliderController>();
        playerHealth = GetComponent<Health>();
        sRend = GetComponent<SpriteRenderer>();
        girlHero = GameObject.Find("GirlHero").GetComponent<GirlHero>();
    }
    
    void Update()
    { 
        Vector2 dir;
        dir = girlHero.transform.position - transform.position;
        heroDistance = dir.magnitude;

        if (attackConsciousness)
        {
            sliderController.health = playerHealth.currentHealth;
        }
    }

    public void ChangeSlider()
    {
        print(gameObject.name);
        sliderController.maxHealth = playerHealth.maxHealth;
        sliderController.health = playerHealth.currentHealth;
        sliderController.sprite = sRend.sprite;
    }
}
