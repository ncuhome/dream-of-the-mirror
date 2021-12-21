using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplayer : MonoBehaviour
{
    public GameObject hero;
    public Sprite heartSprite;

    public Image[] hearts;

    Health playerHealth;

    void Start()
    {
        playerHealth = hero.GetComponent<Health>();
    }

    void Update()
    {
        for (int i = 0; i < playerHealth.currentHealth; i++)
        {
            hearts[i].sprite = heartSprite;
        }
    }
}
