using System.Collections;
using UnityEngine;

public enum Facing
{
    Left = -1,
    Right = 1
}

public class GirlHero : MonoBehaviour
{
    // For GroundSensor
    public Rigidbody2D rb;
    // 当前跳跃次数
    public int currentJumpCount = 0;
    // 是否落地（通过地面触发器）
    public bool grounded = false;
    // 水平运动比例
    public float moveX;
    //现在面朝方向（用于未移动时判定）
    public Facing facing = Facing.Right;

    // 通过翻滚协程判断翻滚是否结束
    public bool rolled = false;

    public CapsuleCollider2D capsuleCollider;
    public Animator anim;

    [Header("实例化的ButtonClickController脚本")]

    public ButtonClickController jumpBtn;
    public ButtonClickController rollBtn;
    public ButtonClickController swordAttackBtn;
    public ButtonClickController magicAttackBtn;

    [Header("线性变量")]
    public float rollSpeed;

    [Header("[Setting]")]
    // 左右移动速度
    public float moveSpeed;
    // 翻滚速度
    public float rollForce;
    // 最大跳跃次数
    public int maxJumpCount;
    // 跳跃升力
    public float jumpForce;
    // 每秒攻击次数
    //修改同时也需要修改动画采样点个数
    public float attackRate;

    public float rollCd;

    public float jumpCd;

    [Header("粒子")]
    public ParticleSystem dust;

    float nextRollTime = 0f;

    float nextAttackTime = 0f;

    float nextJumpTime = 0f;

    Vector3 velocity = Vector3.zero;

    bool spawnLandDust = false;


    MobileHorizontalInputController inputController;

    //暂时提供一下。。。
    private bool curAnimIs(string animName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }

    // 获取组件
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        anim = this.transform.Find("HeroModel").GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        GameObject directionJoyStick = GameObject.Find("DirectionJoyStick");
        inputController = directionJoyStick.GetComponent<MobileHorizontalInputController>();
    }

    void FixedUpdate()
    {
        // 虚拟轴水平移动
        if (inputController.dragging)
        {
            moveX = inputController.horizontal;
        }
        else
        {
            moveX = Input.GetAxisRaw("Horizontal"); // 键盘也算
        }

        // 左右水平移动（因为想要修改成攻击和翻滚不能移动）
        if (moveX == 0
            || curAnimIs("GirlHero_Sword")
            || curAnimIs("GirlHero_Magic")
            || curAnimIs("GirlHero_Roll"))
        {
            anim.SetBool("Running", false);
        }
        else
        {
            if (!grounded)
                anim.SetBool("Running", false);
            else
                anim.SetBool("Running", true);


            facing = moveX > 0 ? Facing.Right : Facing.Left;

            Flip(moveX > 0);


            //先试试放到fixUpdate里面线性移动行不行
            Vector2 tPos = transform.position;
            tPos.x += moveX * moveSpeed * Time.fixedDeltaTime;
            transform.position = tPos;

            //暂时取消，后期迭代
            // Vector3 targetVelocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
            // rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
        }

    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButton("Fire1") || swordAttackBtn.pressed)
            {
                SwordAttack();
                nextAttackTime = GetNextTime(1f / attackRate);
            }
            if (Input.GetButton("Fire2") || magicAttackBtn.pressed)
            {
                MagicAttack();
                nextAttackTime = GetNextTime(1f / attackRate);
            }
        }

        if (Time.time >= nextRollTime)
        {
            if (Input.GetButtonDown("Roll") || rollBtn.pressed)
            {
                //实现点一次按一下（可能不好。。。）
                rollBtn.pressed = false;
                Roll();
                nextRollTime = GetNextTime(rollCd);
            }
        }

        if (Time.time >= nextJumpTime)
        {
            if (jumpBtn.pressed || Input.GetAxisRaw("Vertical") > 0 || Input.GetButtonDown("Jump"))
            {
                if (currentJumpCount < maxJumpCount)
                {
                    //实现点一次按一下（可能不好。。。）
                    jumpBtn.pressed = false;
                    // 跳跃行为
                    Jump();
                    nextJumpTime = GetNextTime(jumpCd);
                }
            }
        }

        if (grounded)
        {
            if (spawnLandDust)
            {
                spawnLandDust = false;
                CreateDust();
            }
        }
        else
        {
            spawnLandDust = true;
        }
    }

    // TODO: finish SwordAttack
    void SwordAttack()
    {
        anim.SetTrigger("SwordAttack");
    }

    // TODO: finish MagicAttack
    void MagicAttack()
    {
        anim.SetTrigger("MagicAttack");
    }

    // TODO: Fix roll distance
    void Roll()
    {
        anim.SetTrigger("Roll");
        CreateDust();

        rb.AddForce(new Vector2((int)facing * rollForce, 0), ForceMode2D.Impulse);
    }

    // TODO: Fix doubleJump
    void Jump()
    {
        anim.SetTrigger("Jump");
        CreateDust();

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        currentJumpCount++;
    }

    // Flip 参数表示此时在哪边，会把人物翻转到另一边
    void Flip(bool right)
    {
        float next = right ? 0 : 180;
        if (transform.localRotation.y != next)
        {
            if (grounded)
            {
                CreateDust();
            }
            transform.rotation = Quaternion.Euler(0, next, 0);
        }
    }

    void CreateDust()
    {
        dust.Play();
    }

    float GetNextTime(float offset)
    {
        return Time.time + offset;
    }
}
