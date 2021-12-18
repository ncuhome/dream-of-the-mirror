using UnityEngine;

public class MapTile : MonoBehaviour
{
    //LocalPosition.x
    public int x; 
    //LocalPosition.y
    public int y; 
    //该块碰撞属性
    public char collisionCharacter;

    //图块碰撞器
    private BoxCollider2D boxCollider2D; 
    
    //必须放到Awake里面
    void Awake() 
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// 子地图脚本完善自身信息与位置
    /// </summary>
    /// <param name="x">子地图相对坐标x值</param>
    /// <param name="y">子地图相对坐标y值</param>
    /// <param name="sprite">子地图对于的贴图的引用</param>
    /// <param name="collisionCharacter">子地图对应的碰撞体类型（通过字符判断）</param>
    public void SetMapTile(int x, int y, ref Sprite sprite, char collisionCharacter) 
    {
        this.x = x;
        this.y = y;
        this.collisionCharacter = collisionCharacter;

        transform.localPosition = new Vector3(x, y, 0);
        gameObject.name = x.ToString("D2")+" * "+y.ToString("D2");
        GetComponent<SpriteRenderer>().sprite = sprite;

        SetCollider2D();
    }

    //为图块分配碰撞器
    void SetCollider2D()
    {   
        boxCollider2D.enabled = true;
        switch (collisionCharacter)
        {
            case 'H': //完全碰撞
                boxCollider2D.offset = Vector2.zero;
                boxCollider2D.size = Vector2.one;
                break;
            case '_': //下划线无碰撞
                boxCollider2D.enabled = false;
                break;
        }
    }
}
