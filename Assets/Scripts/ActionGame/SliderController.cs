using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    //怪物血量滑动条
    public Slider slider;
    //划动条上的怪物贴图
    public Sprite sprite;
    //划动条上的怪物贴图要赋给handle上的sprite
    public Image handleImage;
    //用来判断主角是否进入敌人的攻击范围
    public GameObject [] sliderEnemy;
    //是否显示
    public bool sliderSetActive = false;

    public int maxHealth;
    public int health;
    //滑动条移动长度的值
    public float value;

    void Start()
    {
        slider = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
        slider.value = 1;

        // healthText = transform.Find("Health").GetComponent<Text>();
        handleImage = slider.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>();
        if (sprite != null)
        {
            handleImage.sprite = sprite;
        }

        sliderEnemy = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {
        //遍历太垃圾了，但是我不知道怎么修改
        sliderEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        EnemyAttackConsciousness enemyAttackConsciousness;
        for (int i = 0; i < sliderEnemy.Length; i++)
        {
            enemyAttackConsciousness = sliderEnemy[i].GetComponent<EnemyAttackConsciousness>();
            if (enemyAttackConsciousness.attackConsciousness)
            {
                sliderSetActive = true;
                break;
            }
            else
            {
                if (i == sliderEnemy.Length -1)
                {
                    sliderSetActive = false;
                    break;
                }
            }
        }

        if (sliderSetActive == true)
        {
            slider.gameObject.SetActive(true);
            handleImage.sprite = sprite;
            slider.value = 1.0f * health / maxHealth;
        }
        else
        {
            slider.gameObject.SetActive(false);
        }
    }
}
