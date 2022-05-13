using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSlider : MonoBehaviour
{
    protected static EnemyHealthSlider instance;
    [Header("仅基类血量条脚本需要填充")]
    [SerializeField]
    private Image handleImage;
    [SerializeField]
    private Slider slider;
    private CanvasGroup sliderCanvasGroup;
    private EnemyHealthSlider currentSlider = null;
    private HashSet<EnemyHealthSlider> activeEnemySlider;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("More than one EnemyHealthSlider Working,it must be a singleton.");
        }    
    }

    protected virtual void Start()
    {
        slider.value = 1;
        sliderCanvasGroup = GetComponent<CanvasGroup>();
        sliderCanvasGroup.alpha = 0;
        activeEnemySlider = new HashSet<EnemyHealthSlider>();
    }

    private void Update()
    {
        if (currentSlider == null)
        {
            sliderCanvasGroup.alpha = 0;
            return;
        }
        sliderCanvasGroup.alpha = 1;
        currentSlider.UpdateSlider(ref handleImage, ref slider);
    }

    protected virtual void UpdateSlider(ref Image image, ref Slider slider)
    {

    }

    public void ImportEnemyHealthSlider(EnemyHealthSlider enemyHealthSlider)
    {
        if (currentSlider == null)
        {
            currentSlider = enemyHealthSlider;
            activeEnemySlider.Add(enemyHealthSlider);
            return;
        }
        if (currentSlider == enemyHealthSlider || activeEnemySlider.Contains(enemyHealthSlider))
        {
            return;
        }
        activeEnemySlider.Add(enemyHealthSlider);
    } 

    public void ExportEnemyHealthSlider(EnemyHealthSlider enemyHealthSlider)
    {
        if (currentSlider == null)
        {
            return;
        }
        if (enemyHealthSlider == currentSlider)
        {
            activeEnemySlider.Remove(enemyHealthSlider);
            if (activeEnemySlider.Count == 0)
            {
                currentSlider = null;
            }
            else
            {
                currentSlider = activeEnemySlider.GetEnumerator().Current;
            }
            return;
        }
        activeEnemySlider.Remove(enemyHealthSlider);
    }
}
