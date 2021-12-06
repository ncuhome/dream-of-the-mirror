using UnityEngine;

public class SmallMap : MonoBehaviour
{
    //LocalPosition.x
    public int x; 
    //LocalPosition.y
    public int y; 

    //图块碰撞器
    private BoxCollider boxCollider; 
    
    void Awake() 
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    //通过Map生成SmallMap
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
        switch (c)
        {
            case 'H': //完全碰撞
                boxCollider.center = Vector3.zero;
                boxCollider.size = Vector3.one;
                break;
            case 'W': //顶部碰撞
                boxCollider.center = new Vector3(0, 0.45f, 0);
                boxCollider.size = new Vector3(1, 0.1f, 1);
                break;
            case 'S': //底部碰撞
                boxCollider.center = new Vector3(0, -0.45f, 0);
                boxCollider.size = new Vector3(1, 0.1f, 1);
                break;
            case 'A': //左碰撞
                boxCollider.center = new Vector3(-0.45f, 0, 0);
                boxCollider.size = new Vector3(0.1f, 1, 1);
                break;
            case 'D': //右碰撞
                boxCollider.center = new Vector3(0.45f, 0, 0);
                boxCollider.size = new Vector3(0.1f, 1, 1);
                break;
            // case 'Q': //顶部&左碰撞
            //     boxCollider.center = new Vector3(-0.45f, 0.45f, 0);
            //     boxCollider.size = new Vector3(0.1f, 0.1f, 1);
            //     break;
            // case 'E': //顶部&右碰撞
            //     boxCollider.center = new Vector3(0.45f, 0.45f, 0);
            //     boxCollider.size = new Vector3(0.1f, 0.1f, 1);
            //     break;
            // case 'Z': //底部&左碰撞
            //     boxCollider.center = new Vector3(-0.45f, -0.45f, 0);
            //     boxCollider.size = new Vector3(0.1f, 0.1f, 1);
            //     break;
            // case 'C': //底部&右碰撞
            //     boxCollider.center = new Vector3(0.45f, -0.45f, 0);
            //     boxCollider.size = new Vector3(0.1f, 0.1f, 1);
            //     break;
            default: //其他字符无碰撞
                boxCollider.enabled = false;
                break;
        }
    }
}
