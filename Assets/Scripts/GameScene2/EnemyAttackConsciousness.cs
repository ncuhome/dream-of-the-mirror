using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackConsciousness : MonoBehaviour
{
    public bool attackConsciousness = false;
    public float attackConsciousnessRange;

    protected GirlHero girlHero;
    public SliderController sliderController;
    public PlayerHealth playerHealth;
    protected SpriteRenderer sRend;

    void Start()
    {
        sliderController = GameObject.Find("Canvas").transform.Find("Slider").GetComponent<SliderController>();
        playerHealth = GetComponent<PlayerHealth>();
        sRend = GetComponent<SpriteRenderer>();
        girlHero = GameObject.Find("GirlHero").GetComponent<GirlHero>();
    }
    
    void Update()
    {
        float heroDistance;
        Vector2 dir;
        sliderController.health = playerHealth.currentHealth;
        dir = girlHero.transform.position - transform.position;
        heroDistance = dir.magnitude;

        if (heroDistance <= attackConsciousnessRange)
        {
            attackConsciousness = true;
        }

        if (attackConsciousness)
        {
            //唤醒滑动条
            WakeUpSlider();
        }
        else
        {
            //使滑动条休眠
            SleepSlider();
        }
    }

    public void WakeUpSlider()
    {
        sliderController.gameObject.SetActive(true);
        sliderController.maxHealth = playerHealth.maxHealth;
        sliderController.health = playerHealth.currentHealth;
        sliderController.enemyAttackConsciousness = this;
        sliderController.sprite = sRend.sprite;
    }

    public void SleepSlider()
    {
        sliderController.gameObject.SetActive(false);
    }
}
