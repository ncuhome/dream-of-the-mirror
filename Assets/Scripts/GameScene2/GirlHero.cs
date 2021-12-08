using System.Collections;
using UnityEngine;

public class GirlHero : MonoBehaviour
{
    // <For GroundSensor>
    public Rigidbody2D rb;
    // 当前跳跃次数
    public int currentJumpCount = 0;
    // 是否落地（通过地面触发器）
    public bool grounded = false;
    // 判断是否落到标签为Ground的2D碰撞体上面
    public bool onGroundedTag = false;
    // </For GroundSensor>

    // 水平运动比例
    public float moveX;

    // 通过是否落地的协程判定一次跳跃是否结束（是否在空中）
    public bool onceJumped = false;
    // 通过翻滚协程判断翻滚是否结束
    public bool rolled = false;

    public CapsuleCollider2D capsuleCollider;

    public Animator anim;

    [Header("实例化的ButtonClickController脚本")]
    public ButtonClickController jumpBtn;
    public ButtonClickController rollBtn;
    public ButtonClickController swordAttackBtn;
    public ButtonClickController magicAttackBtn;

    [Header("[Setting]")]
    // 左右移动速度
    public float moveSpeed = 6f;
    // 翻滚速度
    public float rollSpeed = 15f;
    // 最大跳跃次数
    public int maxJumpCount = 2;
    // 跳跃升力
    public float jumpForce = 12f;

    // 使用GameObject.Find()查找
    private MobileInputController mobileInputController;

    private bool curAnimIs(string animName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }

    // 获取组件
    private void Start()
    {
        capsuleCollider = this.transform.GetComponent<CapsuleCollider2D>();
        anim = this.transform.Find("HeroModel").GetComponent<Animator>();
        rb = this.transform.GetComponent<Rigidbody2D>();

        GameObject directionJoyStick = GameObject.Find("DirectionJoyStick");
        mobileInputController = directionJoyStick.GetComponent<MobileInputController>();
    }

    private void Update()
    {
        // 虚拟轴水平移动
        if (mobileInputController.dragging)
        {
            moveX = mobileInputController.horizontal;
        }
        else
        {
            moveX = Input.GetAxisRaw("Horizontal"); // 键盘也算
        }

        // 电脑和轴输入按键以及通过ButtonClickController脚本的按钮输入判定
        CheckInput();
    }

    // 动画判定顺序：翻滚>攻击>跳跃>奔跑=默认
    // Animator设定攻击一次结束后会自动转换成GirlHero_Idle动画）
    public void CheckInput()
    {
        // 如果在空中，进行跳跃协程
        if (onceJumped)
        {
            StartCoroutine(GroundCheck());
        }

        // 动画判定
        if (!curAnimIs("GirlHero_Roll")
            && !curAnimIs("GirlHero_Sword")
            && !curAnimIs("GirlHero_Magic"))
        {
            if (Input.GetKey(KeyCode.S) || rollBtn.pressed)
            {
                anim.Play("GirlHero_Roll");
                if (!rolled)
                {
                    StartCoroutine(Roll());
                }
            }
            else if (Input.GetKey(KeyCode.J) || swordAttackBtn.pressed)
            {
                anim.Play("GirlHero_Sword");
            }
            else if (Input.GetKey(KeyCode.K) || magicAttackBtn.pressed)
            {
                anim.Play("GirlHero_Magic");
            }
            else
            {
                // 奔跑动画播放时机动画
                if (moveX == 0)
                {
                    if (!onceJumped)
                        anim.Play("GirlHero_Idle");
                }
                else
                {
                    anim.Play("GirlHero_Run");
                }
            }
        }

        // 键盘A，D键或虚拟轴控制水平移动，跑动动画播放在攻击里面就要判定（因为一次只能播放一个动画）
        if (Input.GetKey(KeyCode.D) || (mobileInputController.dragging && moveX >= 0))
        {
            // 判定在地面走的时候能不能边走边打
            //  if (grounded)
            //  {
            //      if (curAnimIs("GirlHero_Sword"))
            //          return;

            //      transform.Translate(Vector2.right* moveX * moveSpeed * Time.deltaTime);
            //  }
            //  else
            //  {
            transform.Translate(new Vector3(moveX * moveSpeed * Time.deltaTime, 0, 0));
            //  }

            // 因为攻击时候换方向会很奇怪，所以如果在攻击则人物不会转向
            if (curAnimIs("GirlHero_Sword"))
            {
                return;
            }

            // Filp参数true表示此时往左边移动（x为正），要把人物翻到面朝左边
            // 如果不加判定条件此时再同时按住A，D键，会出现人物移动和方向不一致的情况
            if (moveX >= 0)
            {
                Filp(true); // 表明无需翻转
            }
        }
        else if (Input.GetKey(KeyCode.A) || (mobileInputController.dragging && moveX < 0))
        {
            //  if (grounded)
            //  {
            //      if (curAnimIs("GirlHero_Sword"))
            //          return;

            //      transform.Translate(Vector2.right * moveX * moveSpeed * Time.deltaTime);
            //  }
            //  else
            //  {
            transform.Translate(new Vector3(moveX * moveSpeed * Time.deltaTime, 0, 0));
            //  }

            if (curAnimIs("GirlHero_Sword"))
            {
                return;
            }

            if (moveX < 0)
            {
                print("aaa");
                Filp(false);
            }
        }

        // 空格跳跃键
        if (Input.GetKeyDown(KeyCode.Space) || jumpBtn.pressed)
        {
            // 使其不能长按
            jumpBtn.pressed = false;
            // 让攻击动画打完再跳，要不然会鬼畜。。。
            // 因为跳跃不打断翻滚，所以也是翻滚动画翻完再跳
            if (curAnimIs("GirlHero_Sword")
                || curAnimIs("GirlHero_Magic")
                || curAnimIs("GirlHero_Roll"))
                return;

            // 着陆后currentJumpCount会归零
            if (currentJumpCount < maxJumpCount)
            {
                // 跳跃行为
                Jump();
            }
        }
    }

    // 跳跃时的行为
    public void Jump()
    {
        anim.Play("GirlHero_Idle");

        rb.velocity = new Vector2(0, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        onceJumped = true;
        grounded = false;
        currentJumpCount++;
    }

    // 下面这个协程功能：每0.01s判定是否落地:是否可进行下一次跳跃（即onceJumped需不需要更改）
    // 主角前一刻y坐标
    float PretmpY;
    IEnumerator GroundCheck()
    {
        // 如果在空中，需要判定是否可进行下一次跳跃
        while (onceJumped)
        {
            if (PretmpY == 0)
            {
                PretmpY = transform.position.y;
            }
            else
            {
                // 判断是否下落
                float reY = transform.position.y - PretmpY;
                // 如果在下落
                if (reY <= 0)
                {
                    // 如果着陆，可以进行下一次跳跃
                    if (grounded)
                    {
                        LandingEvent();
                        onceJumped = false;
                    }
                }
                PretmpY = transform.position.y;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    // 落地播放默认动画
    public void LandingEvent()
    {
        if (!curAnimIs("GirlHero_Run")
            && !curAnimIs("GirlHero_Sword")
            && !curAnimIs("GirlHero_Magic")
            && !curAnimIs("GirlHero_Roll"))
        {
            anim.Play("GirlHero_Idle");
        }
    }

    // 翻滚协程
    float RollTime;
    public IEnumerator Roll()
    {
        RollTime = GetLengthByName("GirlHero_Roll");
        rolled = true;
        rb.useAutoMass = false;

        while (rolled)
        {
            if (RollTime <= 0)
            {
                rb.useAutoMass = true;
                rolled = false;
                break;
            }

            if (moveX >= 0)
            {
                transform.Translate(new Vector3(1.0f * rollSpeed * Time.deltaTime, 0, 0));
            }
            else
                transform.Translate(new Vector3((-1.0f) * rollSpeed * Time.deltaTime, 0, 0));

            RollTime -= Time.deltaTime;
            yield return null;
        }
    }

    public float GetLengthByName(string name)
    {
        float length = 0;
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Equals(name))
            {
                length = clip.length;
                break;
            }
        }
        return length;
    }

    // Flip 参数表示此时在哪边，会把人物翻转到另一边
    public void Filp(bool bLeft)
    {
        transform.localScale = new Vector3(bLeft ? 1 : -1, 1, 1);
    }

}
