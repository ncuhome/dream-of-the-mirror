using System.Collections;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public Direction dir = Direction.Idle;
    //是第一张地图还是第二张
    public int mapNum;
    //判断是否到达终点
    public bool ended = false;
    //终点坐标
    public Vector2 mapFinish;

    [Header("Movement Attribute")]
    //移动速度
    public float speed = 2f;

    [Header("Reference")]
    //CharacterController组件
    // CharacterController controller;
    Rigidbody rb;


    //动画组件
    private Animator anim;

    private Vector3[] directions = new Vector3[]{
        Vector3.right, Vector3.up, Vector3.left, Vector3.down
    };

    public void SetHero(int eX, int eY)
    {
        transform.localPosition = new Vector3(eX, eY, 0);
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        // controller = GetComponent<CharacterController>();
    }

    //将该游戏对象赋给SceneController脚本同时启用ReachTheEnd()协程
    void Start()
    {
        if (SceneController.instance.hero1 != null)
            SceneController.instance.hero1 = this;
        else
            SceneController.instance.hero2 = this;

        StartCoroutine(ReachTheEnd());
    }

    private void FixedUpdate()
    {
        //让Hero在移动时可以逐渐移动到格子里面
        GridMove();
    }
    void Update()
    {
        Move();
    }

    public void Move()
    {
        Vector3 vel = Vector3.zero;
        if (dir == Direction.Idle)
        {
            anim.CrossFade("Hero_Walk_" + 0, 0);
            anim.speed = 0;
        }
        else
        {
            vel = directions[(int)(dir)];
            anim.CrossFade("Hero_Walk_" + (int)dir, 0);
            anim.speed = 1;
        }

        rb.MovePosition(this.transform.position + vel * speed * Time.fixedDeltaTime);
        // controller.Move(vel * speed * Time.deltaTime);
    }

    public Vector2 GetPosOnGrid(float mult = 0.5f)
    {
        Vector2 rPos = transform.localPosition;
        rPos /= mult;
        rPos.x = Mathf.Round(rPos.x);
        rPos.y = Mathf.Round(rPos.y);
        rPos *= mult; //将一个小数先乘2,再进行就近取整,再除以2和直接将这个小数就近取整会使结果会更接近原数（0.5的倍数）
        return rPos;
    }
    void GridMove()
    {
        //中心移动
        if (dir == Direction.Idle) return;
        //如果在一个方向移动，分配到网格
        //首先，获取网格位置
        Vector2 rPosGrid = GetPosOnGrid(1f);
        if (Mathf.Abs(rPosGrid.x - transform.localPosition.x) >= 0.1f
                    && Mathf.Abs(rPosGrid.y - transform.localPosition.y) >= 0.1f)
        {
            Vector2 delta = Vector2.zero;
            delta = rPosGrid - (Vector2)transform.localPosition;
            if (delta == Vector2.zero) return;

            Vector2 move = speed * Time.fixedDeltaTime * Vector2.one;
            move.x = Mathf.Min(move.x, Mathf.Abs(delta.x));
            move.y = Mathf.Max(move.y, Mathf.Abs(delta.y));
            if (delta.x < 0) move.x = -move.x;
            if (delta.y < 0) move.y = -move.y;

            Vector2 tLocalPosition = transform.localPosition;
            tLocalPosition += move;
            transform.localPosition = tLocalPosition;
        }

        //线性移动
        // //移动到网格行
        // float delta = 0;
        // if (dir == Direction.Right || dir == Direction.Left)
        // {
        //     //水平移动，分配到y网格
        //     delta = rPosGrid.y - transform.localPosition.y;
        // }
        // else
        // {
        //     //垂直移动，分配到x网格
        //     delta = rPosGrid.x - transform.localPosition.x;
        // }
        // if (delta == 0) return;

        // float move = speed * Time.fixedDeltaTime;
        // move = Mathf.Min(move, Mathf.Abs(delta));
        // if (delta < 0) move = -move;

        // Vector3 tLocalPosition = transform.localPosition;
        // if (dir == Direction.Right || dir == Direction.Left)
        // {

        //     tLocalPosition.y += move;
        //     transform.localPosition = tLocalPosition;
        // }
        // else
        // {
        //     tLocalPosition.x += move;
        //     transform.localPosition = tLocalPosition;
        // }
    }

    //判断该游戏对象是否达到中间（使用协程的目的是减少判定次数）
    IEnumerator ReachTheEnd()
    {
        while (true)
        {
            if (mapNum == 1)
            {
                if (Mathf.Abs(SceneController.instance.mapFinish1.x - transform.localPosition.x) <= 0.25f
                    && Mathf.Abs(SceneController.instance.mapFinish1.y - transform.localPosition.y) <= 0.25f)
                {
                    ended = true;
                }
                else
                    ended = false;
            }
            else
            {
                if (Mathf.Abs(SceneController.instance.mapFinish2.x - transform.localPosition.x) <= 0.25f
                    && Mathf.Abs(SceneController.instance.mapFinish2.y - transform.localPosition.y) <= 0.25f)
                {
                    ended = true;
                }
                else
                    ended = false;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
}
