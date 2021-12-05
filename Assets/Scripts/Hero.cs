using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public int mapNum;
    public float speed = 2f;
    public int dirHeld = -1; //方向移动键是否从键盘上按下，等于-1是表示不移动
    // public int facing = 1; //面向方向
    public bool HeroEnd = false;  //判断是否到达终点
    public float gridMult = 1.5f; //为后面求离人物最接近的以gridMult为倍数的单元格的位置的参数
    public int facing = 1; //面向方向
    public Vector2 mapFinish;

    private Vector3 vel;
    private SpriteRenderer sRend;
    private Rigidbody rigid;
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
        dirHeld = -1;
        sRend = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Start() 
    {
        if(SceneController.instance.hero1 != null)
            SceneController.instance.hero1 = this;
        else
            SceneController.instance.hero2 = this;

        StartCoroutine(ReachTheEnd());
    }

    void Update() 
    {
        if((Vector3)SceneController.instance.mapFinish1 == transform.position)
            HeroEnd = true;
        else
            HeroEnd = false;
        rigid.velocity = vel * speed;
    }

    void FixedUpdate()
    {
        GridMove();
    }

    void LateUpdate() 
    {
        facing = dirHeld;
        vel = Vector3.zero;
        if(dirHeld == -1)
        {
            vel = Vector3.zero;
        }
        else
        {
            vel = directions[dirHeld];
            anim.CrossFade("RightMove", 0);
            anim.speed = 1;
        }

        //重置按键方向
        dirHeld = -1; 
    }

    IEnumerator ReachTheEnd()
    {
        while(true)
        {
            
            if(mapNum == 1)
            {
                print(SceneController.instance.mapFinish1);
                print(transform.localPosition);
                if(Mathf.Abs(SceneController.instance.mapFinish1.x-transform.localPosition.x)<=0.25f && Mathf.Abs(SceneController.instance.mapFinish1.y-transform.localPosition.y)<=0.25f)
                {
                    HeroEnd = true;
                    print("hahahaha");
                }
                    
                else
                    HeroEnd = false;
            }
            else
            {
                if(Mathf.Abs(SceneController.instance.mapFinish2.x-transform.localPosition.x)<=0.25f && Mathf.Abs(SceneController.instance.mapFinish2.y-transform.localPosition.y)<=0.25f)
                {
                    HeroEnd = true;
                    print("cdz sz");
                }
                    
                else
                    HeroEnd = false;
            }
            
            yield return new WaitForSeconds(0.2f);
        }
    }

    // public int GetFacing()
    // {
    //     return facing;
    // }

    public Vector2 GetRoomPosOnGrid(float mult = -1)
    {
        if(mult == -1)
        {
            mult = gridMult;
        }

        Vector2 rPos = transform.localPosition;
        rPos /= mult;
        rPos.x = Mathf.Round(rPos.x);
        rPos.y = Mathf.Round(rPos.y);
        rPos *= mult; //将一个小数先乘2,再进行就近取整,再除以2和直接将这个小数就近取整会使结果会更接近原数（0.5的倍数）
        return rPos;
    }

    void GridMove()
    {
        if(vel == Vector3.zero) return;
        //如果在一个方向移动，分配到网格
        //首先，获取网格位置
        Vector2 rPosGrid = GetRoomPosOnGrid(0.5f);
        
        // //移动到网格行
        float delta = 0;
        if(dirHeld == 0 || dirHeld == 2)
        {
            //水平移动，分配到y网格
            delta = rPosGrid.y - transform.localPosition.y;
        }
        else
        {
            //垂直移动，分配到x网格
            delta = rPosGrid.x - transform.localPosition.x;
        }
        if(delta == 0) return;

        float move = speed * Time.fixedDeltaTime;
        move = Mathf.Min(move, Mathf.Abs(delta));
        if(delta < 0) move = -move;

        Vector3 tLocalPosition = transform.localPosition;
        if(dirHeld == 0 || dirHeld == 2)
        {

            // tLocalPosition.y += move;
            // transform.localPosition = tLocalPosition;
        }
        else
        {
            // tLocalPosition.x += move;
            // transform.localPosition = tLocalPosition;
        }
    }
}
