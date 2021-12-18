using UnityEngine;

public class MapTile : MonoBehaviour
{
    //LocalPosition.x
    public int x; 
    //LocalPosition.y
    public int y; 
    //该块碰撞属性
    public char collisionCharacter;

    /// <summary>
    /// 子地图脚本完善自身信息与位置
    /// </summary>
    /// <param name="x">子地图相对坐标x值</param>
    /// <param name="y">子地图相对坐标y值</param>
    /// <param name="sprite">子地图对于的贴图的引用</param>
    public void SetMapTile(int x, int y, ref Sprite sprite) 
    {
        this.x = x;
        this.y = y;

        transform.localPosition = new Vector3(x, y, 0);
        gameObject.name = x.ToString("D2")+" * "+y.ToString("D2");
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
