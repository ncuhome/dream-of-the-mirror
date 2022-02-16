using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject hero;
    public Image[] hearts;

    Health _health;

    SpriteRenderer[] sprites;

    Color defaultColor;

    Color hightlightColor = new Color(1f, 0.1075269f, 0f, 1);

    void Start()
    {
        _health = hero.GetComponent<Health>();

        sprites = GetComponentsInChildren<SpriteRenderer>();

        defaultColor = sprites[0].color;
    }

    void Update()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (i < _health.currentHealth)
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