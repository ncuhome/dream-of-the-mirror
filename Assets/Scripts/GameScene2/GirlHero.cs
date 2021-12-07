using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlHero : MonoBehaviour, IGroundSensor
{
    //当前跳跃次数
    public int currentJumpCount = 0; 
    //是否落地（通过地面触发器）
    public bool IsGrounded = false;
    //判定一次跳跃有没有结束（是否在空中）
    public bool OnceJumpRayCheck = false;
    //判断落到了墙还是平台
    public bool IsDownJumpGroundCheck = false;
    //水平运动比例（轴）
    public float m_MoveX;

    public Rigidbody2D m_rigidbody;
    public CapsuleCollider2D m_CapsulleCollider;
    public Animator m_Anim;

    [Header("[Setting]")]
    //左右移动速度
    public float MoveSpeed = 6f;
    //最大跳跃次数
    public int JumpCount = 2;
    //跳跃升力
    public float jumpForce = 10f;

    //使用GameObject.Find()查找
    private MobileInputController mobileInputController;

    //实现IGroundSensor接口
    public Rigidbody2D M_rigidbody
    {
        get{
            return m_rigidbody;
        }
    }
    public bool Is_DownJump_GroundCheck
    {
        get{
            return IsDownJumpGroundCheck;
        }
        set{
            IsDownJumpGroundCheck = value;
        }
    }
    public bool _IsGrounded
    {
        get{
            return IsGrounded;
        }
        set{
            IsGrounded = value;
        }
    }
    public int CurrentJumpCount
    {
        get{
            return currentJumpCount;
        }
        set{
            currentJumpCount = value;
        }
    }

    //获取组件
    private void Start()
    {
        m_CapsulleCollider = this.transform.GetComponent<CapsuleCollider2D>();
        m_Anim = this.transform.Find("HeroModel").GetComponent<Animator>();
        m_rigidbody = this.transform.GetComponent<Rigidbody2D>();
        GameObject directionJoyStick = GameObject.Find("DirectionJoyStick");
        mobileInputController = directionJoyStick.GetComponent<MobileInputController>();
    }

    private void Update()
    {
        CheckInput();
    }

    public void CheckInput()
    {
        //虚拟轴水平移动
        // if (mobileInputController.dragging)
        // {
        //     m_MoveX = mobileInputController.Horizontal;
        // }
        // else
        // {
        //     m_MoveX = Input.GetAxisRaw("Horizontal");
        // }
   
        GroundCheckUpdate();


        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Sword"))
        {
            if (Input.GetKey(KeyCode.Mouse0) || Input.GetButtonDown("SwordAttack"))
            {
                m_Anim.Play("GirlHero_Sword");
            }
            else
            {
                //判定是否需要同时播放移动动画
                if (m_MoveX == 0)
                {
                    if (!OnceJumpRayCheck)
                        m_Anim.Play("GirlHero_Idle");
                }
                else
                {
                    m_Anim.Play("GirlHero_Run");
                }
            }
        }

        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Magic"))
        {
            if (Input.GetKey(KeyCode.Mouse1) || Input.GetButtonDown("MagicAttack"))
            {
                m_Anim.Play("GirlHero_Magic");
            }
            else
            {
                if (m_MoveX == 0)
                {
                    if (!OnceJumpRayCheck)
                        m_Anim.Play("GirlHero_Idle");
                }
                else
                {
                    m_Anim.Play("GirlHero_Run");
                }
            }
        }

        //键盘水平移动
        if (Input.GetKey(KeyCode.D))
        {
            if (IsGrounded)
            {
                if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Sword"))
                    return;

                transform.transform.Translate(Vector2.right* m_MoveX * MoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0));
            }

            //攻击键已处理过移动动画
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Sword"))
                return;

            //Filp参数true表示此时往左边移动（x为正），要把人物翻到面朝左边（）
            //如果不加判定条件此时再同时按住A，D键，会出现人物移动和方向不一致的情况
            if (!Input.GetKey(KeyCode.A))
                Filp(true); //表明无需翻转
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (IsGrounded)
            {
                if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Sword"))
                    return;

                transform.transform.Translate(Vector2.right * m_MoveX * MoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0));
            }

            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Sword"))
                return;

            if (!Input.GetKey(KeyCode.D))
                Filp(false);
        }

        //跳跃
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
        {
            //让攻击打完再跳
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Sword"))
                return;

            //着陆后currentJumpCount会归零
            if (currentJumpCount < JumpCount)  
            {
                PrefromJump();
            }
        }
    }

    //跳跃时的行为
    public void PrefromJump()
    {
        m_Anim.Play("GirlHero_Idle");

        m_rigidbody.velocity = new Vector2(0, 0);
        m_rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        OnceJumpRayCheck = true;
        IsGrounded = false;
        currentJumpCount++;
    }

    //主角前一刻y坐标
    float PretmpY;
    float GroundCheckUpdateTic = 0;
    float GroundCheckUpdateTime = 0.01f;
    public void GroundCheckUpdate()
    {
        //如果不在空中，不需要判定是否可进行下一次跳跃（既OnceJumpRayCheck不需要更改）
        if (!OnceJumpRayCheck)
            return;

        GroundCheckUpdateTic += Time.deltaTime;

        //每0.1s进行一次判定，没有用协程，因为空中的每帧都会调用这个函数。。。
        if (GroundCheckUpdateTic > GroundCheckUpdateTime)
        {
            GroundCheckUpdateTic = 0;

            if (PretmpY == 0)
            {
                PretmpY = transform.position.y;
                return;
            }

            //判断是否在空中
            float reY = transform.position.y - PretmpY;
            //如果在下落
            if (reY <= 0)
            {
                //如果着陆，可以进行下一次跳跃
                if (IsGrounded)
                {
                    LandingEvent();
                    OnceJumpRayCheck = false;
                }
            }

            PretmpY = transform.position.y;
        }
    }

    //落地播放默认动画
    public void LandingEvent()
    {
        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Run") 
            && !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Sword"))
        {
            m_Anim.Play("GirlHero_Idle");
        }
    }

    //Filp参数表示此时在哪边，会把人物翻转到另一边
    public void Filp(bool bLeft)
    {
        transform.localScale = new Vector3(bLeft ? 1 : -1, 1, 1);
    }
}
