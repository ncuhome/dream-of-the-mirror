using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
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

    [Header("Reference: ")]
    //怪物血量滑动条
    public Slider slider;

    //临时存储一个敌人是否进入战斗状态
    private EnemyAttackConsciousness enemyAttackConsciousness;

    void Start()
    {
        slider.value = 1;

        handleImage = slider.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>();
        if (sprite != null)
        {
            handleImage.sprite = sprite;
        }
    }

    void Update()
    {
        //获取场上的全部敌人（后期需要改动）
        sliderEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        int i;
        for (i = 0; i < sliderEnemy.Length; i++)
        {
            if (!sliderEnemy[i].GetComponent<EnemySlider>())
            {
                continue;
            }
            if (sliderEnemy[i].GetComponent<EnemyAttackConsciousness>().attackConsciousness)
            {
                sliderEnemy[i].GetComponent<EnemySlider>().FixSlider();
                sliderSetActive = true;
                break;
            }
        }
        if (i == sliderEnemy.Length)
        {
            sliderSetActive = false;
        }

        if (sliderSetActive == true)
        {
            handleImage.sprite = sprite;
            slider.value = 1.0f * health / maxHealth;
            slider.gameObject.SetActive(true);
        }
        else
        {
            slider.gameObject.SetActive(false);
        }
    }
}
