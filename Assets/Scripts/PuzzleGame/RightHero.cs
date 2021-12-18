using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHero : MonoBehaviour
{
    public Direction dir = Direction.Idle;
    //判断是否到达终点
    public bool ended = false;
    //起点坐标
    public Vector2 startLocalPoint;
    //终点坐标
    public Vector2 endLocalPoint;

    [Header("Movement Attribute")]
    //移动一格需要的秒数
    public int TIME_SET;

    //人物2D碰撞体
    private Rigidbody2D rb2D;
    //人物动画
    private Animator anim;
    
    //人物下一个FixedUpdate要走到的地方
    private Vector2 pos;
    private int timer;

    /// <summary>
    /// 补充起点终点信息，同时修改人物相对位置
    /// </summary>
    /// <param name="startLocalPoint">人物起点相对于地图的坐标</param>
    /// <param name="endLocalPoint">人物终点相对于地图的坐标</param>
    public void SetHero(Vector2 startLocalPoint, Vector2 endLocalPoint)
    {
        this.startLocalPoint = startLocalPoint;
        this.endLocalPoint = endLocalPoint;
        pos = startLocalPoint;
        timer = 0;

        transform.localPosition = startLocalPoint;
    }

    //将该游戏对象赋给SceneController脚本同时启用ReachTheEnd()协程
    void Start()
    {
        //最后需要让SceneController结束游戏
        SceneController.instance.rightHero = this;
        
        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        StartCoroutine(ReachTheEnd());
    }

    private void FixedUpdate()
    {
        if(timer > 0)
        {
            rb2D.MovePosition(rb2D.position + pos);
            timer--;
            return;
        }
        if (dir == Direction.Up)
        {
            Up();
            return;
        }
        if (dir == Direction.Down)
        {
            Down();
            return;
        }
        if (dir == Direction.Left)
        {
            Left();
            return;
        }
        if (dir == Direction.Right)
        {
            Right();
            return;
        }
    }

    private void Up()
    {
        //发出射线检测是否有墙壁，有就停止，不写了
        pos = Vector2.up / TIME_SET;
        timer = TIME_SET;
    }
    private void Down()
    {
        pos = Vector2.down / TIME_SET;
        timer = TIME_SET;
    }
    private void Left()
    {
        pos = Vector2.left / TIME_SET;
        timer = TIME_SET;
    }
    private void Right()
    {
        pos = Vector2.right / TIME_SET;
        timer = TIME_SET;
    }

    //修改移动动画
    void Update()
    {
        if (dir == Direction.Idle)
        {
            anim.CrossFade("Hero_Walk_" + 0, 0);
            anim.speed = 0;
        }
        else
        {
            anim.CrossFade("Hero_Walk_" + (int)dir, 0);
            anim.speed = 1;
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
