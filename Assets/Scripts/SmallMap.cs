using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMap : MonoBehaviour
{
    public Hero hero;
    
    [Header("Set Dynamicallly")]
    public int x;
    public int y;

    private BoxCollider boxCollider;
    
    void Awake() 
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public void SetSmallMap(int eX, int eY, int num = -1)
    {
        x = eX;
        y = eY;
        transform.localPosition = new Vector3(x, y, 0);
        gameObject.name = x.ToString("D3")+"×"+y.ToString("D3");
        GetComponent<SpriteRenderer>().sprite = Map.SPRITES[num];

        SetCollider(num);
    }

    //为图块分配碰撞器
    void SetCollider(int num)
    {
        boxCollider.enabled = true;
        char c = Map.COLLISIONS[num];
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
                hero.SetHero(x, y);
                boxCollider.enabled = false;
                break;
            default: //其他字符无碰撞
                boxCollider.enabled = false;
                break;
        }
    }
}
