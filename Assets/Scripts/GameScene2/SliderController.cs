using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    //怪物血量滑动条
    public Slider slider;
    //怪物血量
    public Text healthText;
    //划动条上的怪物贴图
    public Sprite sprite;
    //划动条上的怪物贴图要赋给handle上的sprite
    public SpriteRenderer handle;
    //用来判断主角是否进入敌人的攻击范围
    public Enemy enemy;

    public int maxHealth;
    public int health;
    //滑动条移动长度的值
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
        if (enemy == null)
        {
            gameObject.SetActive(false);
            return;
        }
        slider.value = 1.0f * health / maxHealth;
        healthText.text = health.ToString();
    }
}
