using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public float speed = 2f;
    public int dirHeld = -1; //方向移动键是否从键盘上按下，等于-1是表示不移动
    // public int facing = 1; //面向方向
    public bool HeroEnd = false;  //判断是否到达终点
    public float gridMult = 1.5f; //为后面求离人物最接近的以gridMult为倍数的单元格的位置的参数

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
    }

    void Update()
    {
        Vector3 vel = Vector3.zero;
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
        
        rigid.velocity = vel * speed;
    }

    void LateUpdate() 
    {
        //重置主角移动
        // dirHeld = -1;

        // //获取游戏对象的半网格位置
        // Vector2 rPos = GetRoomPosOnGrid(0.5f);
    }

    void OnCollisionEnter(Collision coll) 
    {
        if(coll.gameObject.tag == "End")
        {
            HeroEnd = true;
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

        Vector2 rPos = this.transform.position;
        rPos /= mult;
        rPos.x = Mathf.Round(rPos.x);
        rPos.y = Mathf.Round(rPos.y);
        rPos *= mult; //将一个小数先乘2,再进行就近取整,再除以2和直接将这个小数就近取整会使结果会更接近原数（0.5的倍数）
        return rPos;
    }
}
