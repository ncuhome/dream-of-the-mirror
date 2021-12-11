using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject hero;
    public Image[] hearts;

    PlayerHealth playerHealth;

    SpriteRenderer[] sprites;

    Color defaultColor;

    Color hightlightColor = new Color(1f, 0.1075269f, 0f, 1);

    void Start()
    {
        playerHealth = hero.GetComponent<PlayerHealth>();

        sprites = GetComponentsInChildren<SpriteRenderer>();

        defaultColor = sprites[0].color;
    }

    void Update()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (i < playerHealth.currentHealth)
            {
                sprites[i].color = hightlightColor;
            }
            else
            {
                sprites[i].color = Color.white;
            }
        }
    }
}
