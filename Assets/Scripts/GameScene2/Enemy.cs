using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy: ")]
    public string ID;
    public int maxHealth;
    public int health;
    public float attackRange;
    public float heroDistance;
    public float invincibleDuration = 0.5f;
    public bool invincible = false;
    public SliderController sliderController;

    protected GirlHero girlHero;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected SpriteRenderer sRend;

    protected float nextInvincibleTime = 0f;
    protected Vector2 dir = Vector2.zero;

    protected virtual void Start()
    {
        ID = gameObject.name;
        health = maxHealth;
        girlHero = GameObject.Find("GirlHero").GetComponent<GirlHero>();
        sliderController = GameObject.Find("SliderCanvas").transform.Find("SliderPanel").GetComponent<SliderController>();

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sRend = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        dir = girlHero.transform.position - transform.position;
        heroDistance = dir.magnitude;
        dir.Normalize();

        if (heroDistance > attackRange)
        {
            return;
        }

        //唤醒滑动条
        WakeUpSlider();
        //确认无敌和被击退状态
        if (invincible && Time.time > nextInvincibleTime)
        {
            nextInvincibleTime = Time.time + invincibleDuration;
            StartCoroutine(GetInvincibility());
        }
    }

    //进入无敌状态与取消无敌状态的协程
    IEnumerator GetInvincibility()
    {
        while (Time.time < nextInvincibleTime)
        {
            sRend.color = Color.red;
            yield return null;
        }
        invincible = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D colld)
    {
        if(invincible)
            return;
        DamageEffect damageEffect = colld.gameObject.GetComponent<DamageEffect>();
        if(damageEffect == null || colld.tag == "Enemy")
            return;

        health -= damageEffect.damage;
        if(health <= 0)
            Die();

        invincible = true;
    }

    protected void Die()
    {
        Destroy(gameObject);
    }

    protected void WakeUpSlider()
    {
        sliderController.gameObject.SetActive(true);
        sliderController.enemy = this;
        sliderController.health = health;
        sliderController.sprite = sRend.sprite;
        sliderController.maxHealth = maxHealth;
    }

    public void Flip(bool right)
    {
        transform.localScale = new Vector3(right ? 1 : -1, 1, 1);
    }
}
