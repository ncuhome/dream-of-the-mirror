using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlHero : MonoBehaviour, IGroundSensor
{
    //当前跳跃次数
    public int currentJumpCount = 0; 
    //是否落地（通过地面触发器）
    public bool IsGrounded = false;
    //通过是否落地的协程判定一次跳跃有没有结束（是否在空中）
    public bool OnceJumpCheck = false;
    //判断是否落到标签为Ground的2D碰撞体上面
    public bool IsDownJumpGroundCheck = false;
    //水平运动比例
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
    public float jumpForce = 12f;

    //使用GameObject.Find()查找
    private MobileInputController mobileInputController;

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
        //电脑和轴输入判定
        CheckInput();
    }

    //判定顺序：翻滚-普通攻击-魔法攻击-移动-跳跃
    //Animator设定攻击一次结束后会自动转换成GirlHero_Idle动画）
    public void CheckInput()
    {
        //虚拟轴水平移动
        if (mobileInputController.dragging)
        {
            m_MoveX = mobileInputController.Horizontal;
        }
        else
        {
            //键盘也算
            m_MoveX = Input.GetAxisRaw("Horizontal");
        }

        //翻滚
        if (Input.GetKey("down"))
        {
            Roll();
            m_Anim.Play("GirlHero_Sword");
        }

        if(OnceJumpCheck)
        {
            StartCoroutine(GroundCheck());
        }

        //普通攻击
        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Sword") 
            && !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Magic"))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                m_Anim.Play("GirlHero_Sword");
            }
            else
            {
                //判定攻击的时候是否需要同时播放移动动画
                if (m_MoveX == 0)
                {
                    if (!OnceJumpCheck)
                        m_Anim.Play("GirlHero_Idle");
                }
                else
                {
                    m_Anim.Play("GirlHero_Run");
                }
            }
        }

        //法杖攻击暂时同普通攻击
        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Magic") 
            && !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Sword"))
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                m_Anim.Play("GirlHero_Magic");
            }
            else
            {
                if (m_MoveX == 0)
                {
                    if (!OnceJumpCheck)
                        m_Anim.Play("GirlHero_Idle");
                }
                else
                {
                    m_Anim.Play("GirlHero_Run");
                }
            }
        }

        //键盘A，D键或虚拟轴控制水平移动，跑动动画播放在攻击里面就要判定（因为一次只能播放一个动画）
        if (Input.GetKey(KeyCode.D) || mobileInputController.dragging)
        {
            //判定在地面走的时候能不能边走边打
            // if (IsGrounded)
            // {
            //     if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Sword"))
            //         return;

            //     transform.transform.Translate(Vector2.right* m_MoveX * MoveSpeed * Time.deltaTime);
            // }
            // else
            // {
            transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0));
            // }

            //因为攻击时候换方向会很奇怪，所以如果在攻击则人物不会转向
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Sword"))
            {
                return;
            }

            //Filp参数true表示此时往左边移动（x为正），要把人物翻到面朝左边
            //如果不加判定条件此时再同时按住A，D键，会出现人物移动和方向不一致的情况
            if (m_MoveX >= 0)
            {
                Filp(true); //表明无需翻转
            }
        }
        else if (Input.GetKey(KeyCode.A) || mobileInputController.dragging)
        {
            // if (IsGrounded)
            // {
            //     if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Sword"))
            //         return;

            //     transform.transform.Translate(Vector2.right * m_MoveX * MoveSpeed * Time.deltaTime);
            // }
            // else
            // {
            transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0));
            // }

            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Sword"))
            {
                return;
            }
                
            if (m_MoveX < 0)
            {
                Filp(false);
            } 
        }

        //空格跳跃键
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //让攻击打完再跳，要不然会鬼畜。。。
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GirlHero_Sword"))
                return;

            //着陆后currentJumpCount会归零
            if (currentJumpCount < JumpCount)  
            {
                //跳跃行为
                Jump();
            }
        }
    }

    //跳跃时的行为
    public void Jump()
    {
        m_Anim.Play("GirlHero_Idle");

        m_rigidbody.velocity = new Vector2(0, 0);
        m_rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        OnceJumpCheck = true;
        IsGrounded = false;
        currentJumpCount++;
    }

    //下面这个协程功能：每0.01s判定是否落地:是否可进行下一次跳跃（即OnceJumpCheck需不需要更改）
    //主角前一刻y坐标
    float PretmpY;
    IEnumerator GroundCheck()
    { 
        //如果在空中，需要判定是否可进行下一次跳跃
        while(OnceJumpCheck)
        {
            if (PretmpY == 0)
            {
                PretmpY = transform.position.y;
            }
            else
            {
                //判断是否下落
                float reY = transform.position.y - PretmpY;
                //如果在下落
                if (reY <= 0)
                {
                    //如果着陆，可以进行下一次跳跃
                    if (IsGrounded)
                    {
                        LandingEvent();
                        OnceJumpCheck = false;
                    }
                }
                PretmpY = transform.position.y;
            }
            
            yield return new WaitForSeconds(0.01f);
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

    //翻滚
    public void Roll()
    {

    }

    //Filp参数表示此时在哪边，会把人物翻转到另一边
    public void Filp(bool bLeft)
    {
        transform.localScale = new Vector3(bLeft ? 1 : -1, 1, 1);
    }

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

}
