using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider slider;
    public Text healthText;
    public Sprite sprite;
    public SpriteRenderer handle;

    public int maxHealth;
    public int health;
    public float value;

    void Start()
    {
        slider = transform.Find("Slider").GetComponent<Slider>();
        slider.value = 1;

        healthText = transform.Find("Health").GetComponent<Text>();
        handle = slider.transform.Find("HandleSlideArea").Find("Handle").GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            handle.sprite = sprite;
        }  
    }

    void Update()
    {
        slider.value = 1.0f * health / maxHealth;
        healthText.text = health.ToString();
    }
}
