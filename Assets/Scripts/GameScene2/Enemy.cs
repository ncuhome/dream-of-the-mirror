using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy: ")]
    public string ID;
    public float attackRange;
    public float heroDistance;

    public SliderController sliderController;
    public PlayerHealth playerHealth;

    protected GirlHero girlHero;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected SpriteRenderer sRend;

    protected Vector2 dir = Vector2.zero;

    protected virtual void Start()
    {
        ID = gameObject.name;
        girlHero = GameObject.Find("GirlHero").GetComponent<GirlHero>();
        sliderController = GameObject.Find("Canvas").transform.Find("Slider").GetComponent<SliderController>();

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sRend = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    protected virtual void Update()
    {
        sliderController.health = playerHealth.currentHealth;
        dir = girlHero.transform.position - transform.position;
        heroDistance = dir.magnitude;
        dir.Normalize();

        if (heroDistance > attackRange)
        {
            return;
        }

        //唤醒滑动条
        WakeUpSlider();
    }

    protected void WakeUpSlider()
    {
        sliderController.gameObject.SetActive(true);
        sliderController.maxHealth = playerHealth.maxHealth;
        sliderController.health = playerHealth.currentHealth;
        sliderController.enemy = this;
        sliderController.sprite = sRend.sprite;
    }

    protected void Flip(bool right)
    {
        float next = right ? 0 : 180;
        if (transform.rotation.eulerAngles.y != next)
        {
            transform.rotation = Quaternion.Euler(0, next, 0);
        }
    }
}
