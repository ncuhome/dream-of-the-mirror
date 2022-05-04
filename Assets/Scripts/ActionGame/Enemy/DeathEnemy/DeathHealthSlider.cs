using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathHealthSlider : EnemyHealthSlider
{
    private DeathAnimComponent anim;
    private DeathHealth health;
    private EnemyAttackConsciousness enemyAttackConsciousness;

    protected override void Awake()
    {
        
    }

    protected override void Start()
    {
        anim = GetComponent<DeathAnimComponent>();    
        health = GetComponent<DeathHealth>();
        enemyAttackConsciousness = GetComponent<EnemyAttackConsciousness>();
    }

    void Update()
    {
        if (enemyAttackConsciousness.CheckAttackConsciousness())
        {
            EnemyHealthSlider.instance.ImportEnemyHealthSlider(this);
        }    
        else
        {
            EnemyHealthSlider.instance.ExportEnemyHealthSlider(this);
        }
    }

    protected override void UpdateSlider(ref Image image, ref Slider slider)
    {
        image.sprite = anim.SRend.sprite;
        slider.value = 1.0f * health.CurrentHealth / health.maxHealth;
    }
}