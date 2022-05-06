using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image[] hearts;

    private GirlHeroHealth health_;
    private SpriteRenderer[] sprites;
    private Color defaultColor;
    private Color hightlightColor = new Color(1f, 0.1075269f, 0f, 1);

    void Start()
    {
        health_ = PlayerManager.instance.girlHero.GetComponent<GirlHeroHealth>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
        defaultColor = sprites[0].color;
    }

    void Update()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (i < health_.CurrentHealth)
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