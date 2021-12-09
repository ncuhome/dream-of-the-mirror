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

    [Header("[Setting]")]
    // 左右移动速度
    public float moveSpeed;
    // 翻滚速度
    public float rollForce;
    // 最大跳跃次数
    public int maxJumpCount;
    //平滑移动阻尼（修改时速度也需要修改）
    public float smoothTime;
    // 跳跃升力
    public float jumpForce;
    // 每秒攻击次数
    public float attackRate;

    public float rollCd;

    float nextRollTime = 0f;

    float nextAttackTime = 0f;

    Vector3 velocity = Vector3.zero;

    MobileHorizontalInputController inputController;

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

        // 左右水平移动
        if (moveX != 0)
        {
            anim.SetBool("Running", true);

            Flip(moveX > 0);

            Vector3 targetVelocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smoothTime);
            print(rb.velocity);
        }
        else
        {
            anim.SetBool("Running", false);
        }
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButton("Fire1") || swordAttackBtn.pressed)
            {
                SwordAttack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
            if (Input.GetButton("Fire2") || magicAttackBtn.pressed)
            {
                MagicAttack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        if (Time.time >= nextRollTime)
        {
            if (Input.GetButton("Roll") || rollBtn.pressed)
            {
                Roll();
                nextRollTime = Time.time + rollCd;
            }
        }

        if (jumpBtn.pressed || Input.GetAxisRaw("Vertical") > 0 || Input.GetButtonDown("Jump"))
        {
            if (currentJumpCount < maxJumpCount)
            {
                // 跳跃行为
                Jump();
            }
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

        Vector2 dir = new Vector2(transform.localScale.x, 0);
        rb.AddForce(dir * rollForce, ForceMode2D.Impulse);
    }

    // TODO: Fix doubleJump
    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        grounded = false;
        currentJumpCount++;
    }

    // Flip 参数表示此时在哪边，会把人物翻转到另一边
    void Flip(bool right)
    {
        transform.localScale = new Vector3(right ? 1 : -1, 1, 1);
    }
}
