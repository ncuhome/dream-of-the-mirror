using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMap : MonoBehaviour
{
    [Header("提前录入：")]
    public Hero hero1Prefab;
    public Hero hero2Prefab;
    
    [Header("非提前录入：")]
    public int x;
    public int y;

    private BoxCollider boxCollider;
    
    void Awake() 
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public void SetSmallMap(Map map, int i, int j, int num = -1)
    {
        x = (int)map.MAPTRANS[i, j].x;
        y = (int)map.MAPTRANS[i, j].y;
        transform.localPosition = new Vector3(x, y, 0);
        gameObject.name = x.ToString("D2")+"×"+y.ToString("D2");
        GetComponent<SpriteRenderer>().sprite = map.SPRITES[num];

        SetCollider(map, num);
    }

    //为图块分配碰撞器
    void SetCollider(Map map, int num)
    {
        boxCollider.enabled = true;
        char c = map.COLLISIONS[num];
        switch(c)
        {
            case 'H': //完全碰撞
                boxCollider.center = Vector3.zero;
                boxCollider.size = Vector3.one;
                break;
            case 'W': //顶部碰撞
                boxCollider.center = new Vector3(0, 0.25f, 0);
                boxCollider.size = new Vector3(1, 0.5f, 1);
                break;
            case 'S': //底部碰撞
                boxCollider.center = new Vector3(0, -0.25f, 0);
                boxCollider.size = new Vector3(0.5f, 0.5f, 1);
                break;
            case 'A': //左碰撞
                boxCollider.center = new Vector3(-0.25f, 0, 0);
                boxCollider.size = new Vector3(0.5f, 1, 1);
                break;
            case 'D': //右碰撞
                boxCollider.center = new Vector3(0.25f, 0, 0);
                boxCollider.size = new Vector3(0.5f, 1, 1);
                break;
            case 'Q': //顶部&左碰撞
                boxCollider.center = new Vector3(-0.25f, 0.25f, 0);
                boxCollider.size = new Vector3(0.5f, 0.5f, 1);
                break;
            case 'E': //顶部&右碰撞
                boxCollider.center = new Vector3(0.25f, 0.25f, 0);
                boxCollider.size = new Vector3(0.5f, 0.5f, 1);
                break;
            case 'Z': //底部&左碰撞
                boxCollider.center = new Vector3(-0.25f, -0.25f, 0);
                boxCollider.size = new Vector3(0.5f, 0.5f, 1);
                break;
            case 'C': //底部&右碰撞
                boxCollider.center = new Vector3(0.25f, -0.25f, 0);
                boxCollider.size = new Vector3(0.5f, 0.5f, 1);
                break;
            case 'T': //出生地
                Hero tHero;
                if(this.transform.parent.name == "Map1Anchor")
                {
                    tHero = Instantiate<Hero>(hero1Prefab);
                    SceneController.instance.hero1 = tHero;
                }
                else
                {
                    tHero = Instantiate<Hero>(hero2Prefab);
                    SceneController.instance.hero2 = tHero;
                }
                tHero.transform.SetParent(this.transform.parent);
                tHero.SetHero(x, y);
                boxCollider.enabled = false;
                break;
            case 'F': //最终地
                if(SceneController.instance.mapFinish1 == null)
                    SceneController.instance.mapFinish1 = new Vector2(x, y);
                else
                    SceneController.instance.mapFinish2 = new Vector2(x, y);
                boxCollider.center = Vector3.zero;
                boxCollider.size = Vector3.one;
                break;
            default: //其他字符无碰撞
                boxCollider.enabled = false;
                break;
        }
    }
}
