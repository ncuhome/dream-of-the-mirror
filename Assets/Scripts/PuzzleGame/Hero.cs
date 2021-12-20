using System.Collections;
using UnityEngine;

public enum HeroId
{
    LeftHero,
    RightHero
}

public class Hero : MonoBehaviour
{
    //判断是否到达终点
    public bool ended = false;
    //起点坐标
    public Vector2 startLocalPoint;
    //终点坐标
    public Vector2 endLocalPoint;
    public HeroId heroId;

    //人物2D碰撞体
    private Rigidbody2D rb2D;
    //人物动画
    private Animator anim;

    /// <summary>
    /// 补充起点终点信息，同时修改人物相对位置
    /// </summary>
    /// <param name="startLocalPoint">人物起点相对于地图的坐标</param>
    /// <param name="endLocalPoint">人物终点相对于地图的坐标</param>
    public void SetHero(Vector2 startLocalPoint, Vector2 endLocalPoint, HeroId heroId)
    {
        this.startLocalPoint = startLocalPoint;
        this.endLocalPoint = endLocalPoint;
        this.heroId = heroId;

        transform.localPosition = startLocalPoint;
    }

    //将该游戏对象赋给SceneController脚本同时启用ReachTheEnd()协程
    void Start()
    {
        //需要让SceneController结束游戏
        if (heroId == HeroId.LeftHero)
        {
            SceneController.instance.leftHero = this;
        }
        if (heroId == HeroId.RightHero)
        {
            SceneController.instance.rightHero = this;
        }
        
        //需要HeroMoveController控制人物移动
        if (heroId == HeroId.LeftHero)
        {
            HeroMoveController.instance.leftHero = this;
        }
        if (heroId == HeroId.RightHero)
        {
            HeroMoveController.instance.rightHero = this;
        }
        
        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        StartCoroutine(ReachTheEnd());
    }

    public void Move(Direction dir, Vector2 pos)
    {
        rb2D.MovePosition(rb2D.position + pos);

        if (dir != Direction.Idle)
        {
            anim.CrossFade("Hero_Walk_" + (int)dir, 0);
            anim.speed = 1;
        }
    }

    public void Idle(Direction dir)
    {
        if (dir != Direction.Idle)
        {
            anim.CrossFade("Hero_Walk_" + (int)dir, 0);
            anim.speed = 0;
        }
    }

    //判断该游戏对象是否达到中间（使用协程的目的是减少判定次数）
    IEnumerator ReachTheEnd()
    {
        while (true)
        {
            if (Mathf.Abs(endLocalPoint.x - transform.localPosition.x) <= 0.25f
                && Mathf.Abs(endLocalPoint.y - transform.localPosition.y) <= 0.25f)
            {
                ended = true;
            }
            else
            {
                ended = false;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
}
