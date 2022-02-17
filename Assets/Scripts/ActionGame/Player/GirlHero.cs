using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlHero : MonoBehaviour
{
    // For GroundSensor
    public Rigidbody2D rb;
    public bool grounded = false;

    public BoxCollider2D boxCollider;
    public Animator anim;
    public Health playerHealth;

    [Header("实例化的ButtonClickController脚本")]
    public ButtonClickController jumpBtn;
    public ButtonClickController rollBtn;
    public ButtonClickController swordAttackBtn;
    public ButtonClickController magicAttackBtn;

    [Header("[Setting]")]
    // 左右移动速度
    public float moveSpeed;

    // 最大跳跃次数
    public int maxJumpCount;
    //最大翻滚次数（限制空中多次翻滚）
    public int maxRollCount = 1;
    public int currentJumpCount = 0;
    public int currentRollCount = 0;

    // 每秒攻击次数
    public float attackRate;
    
    public float rollCd;
    public float jumpCd;

    public bool readyToRoll = false;

    [Header("贴图默认朝向")]
    public Facing facing;

    [Header("粒子")]
    public GameObject dustEffect;

    [Header("音频")]
    public AudioSource attackAudio;
    public AudioSource jumpAudio;
    public AudioSource runAudio;
    public AudioSource rollAudio;

    // 水平运动比例
    private float moveX;
    private float nextRollTime = 0f;
    private float nextAttackTime = 0f;

    private bool spawnLandDust = false;
    private ParticleSystem dust;

    private MobileHorizontalInputController inputController;

    private float distToGround;

    private bool curAnimIs(string animName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }

    // 获取组件
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<Health>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = transform.Find("HeroModel").GetComponent<Animator>();
        
        GameObject directionJoyStick = GameObject.FindGameObjectWithTag("DirectionJoyStick");
        inputController = directionJoyStick.GetComponent<MobileHorizontalInputController>();

        var dustPos = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        var dustPrefab = Instantiate(dustEffect, dustPos, Quaternion.identity);
        dustPrefab.transform.SetParent(transform);
        dust = dustPrefab.GetComponent<ParticleSystem>();

        distToGround = boxCollider.bounds.extents.y;
    }

    void FixedUpdate()
    {
        if (PauseControl.gameIsPaused) return;

        // 虚拟轴水平移动
        if (inputController.dragging)
        {
            moveX = inputController.horizontal;
        }
        else
        {
            moveX = Input.GetAxisRaw("Horizontal");
        }

        // 左右水平移动（因为想要实现只有攻击和翻滚不能移动，其它情况下可以移动，且空中移动播放空中动画）
        if (moveX == 0
            // || curAnimIs("GirlHero_Sword")
            || curAnimIs("GirlHero_Magic")
            || curAnimIs("GirlHero_Roll"))
        {
            anim.SetBool("Running", false);
            StopAudio(runAudio);
        }
        else if (curAnimIs("GirlHero_Sword"))
        {
            anim.SetBool("Running", false);
            StopAudio(runAudio);

            Flip(moveX > 0);

            float swordMoveSpeed = moveSpeed / 2;

            Vector2 tPos = transform.position;
            tPos.x += moveX * swordMoveSpeed * Time.fixedDeltaTime;
            transform.position = tPos;
        }
        else
        {
            if (!grounded)
            {
                anim.SetBool("Running", false);
                StopAudio(runAudio);
            }
            else
            {
                anim.SetBool("Running", true);
                PlayAudio(runAudio);
            }

            Flip(moveX > 0);

            //不能直接使用MovePosition，否则将停止使用重力下落，transform会后于物理执行
            // Vector2 newPos = Vector2.MoveTowards(rb.position, rb.position + (Vector2)transform.right, moveSpeed * Time.fixedDeltaTime);
            // rb.MovePosition(newPos);
            // Vector2 positionOffset = Physics2D.gravity / rb.gravityScale;

            if (!playerHealth.isRepelled)
            {
                Vector2 tPos = transform.position;
                tPos.x += moveX * moveSpeed * Time.fixedDeltaTime;
                transform.position = tPos;
            }

        }
    }

    void Update()
    {
        if (PauseControl.gameIsPaused) return;

        //使人物可以识别持续碰撞
        rb.WakeUp();
        
        if (Time.time >= nextAttackTime && !curAnimIs("GirlHero_Roll"))
        {
            if (Input.GetButtonDown("Fire1") || swordAttackBtn.pressed)
            {
                SwordAttack();
            }
            if (Input.GetButtonDown("Fire2") || magicAttackBtn.pressed)
            {
                MagicAttack();
            }
        }

        grounded = IsGrounded();

        if (grounded)
        {
            //因为grounded的判定要先于接触地面
            if (rb.velocity.y == 0)
            {
                currentRollCount = 0;
                currentJumpCount = 0;
            }
            
            if (spawnLandDust)
            {
                spawnLandDust = false;
                CreateDust();
                PlayAudio(jumpAudio);
            }
        }
        else
        {
            spawnLandDust = true;
        }

        if (Input.GetButtonDown("Roll") || rollBtn.pressed)
        {
            if (curAnimIs("GirlHero_Sword"))
            {
                readyToRoll = true;
            }
            else if (Time.time >= nextRollTime && !curAnimIs("GirlHero_Magic") && (currentRollCount < maxRollCount))
            {
                Roll();
            }
        }

        if (jumpBtn.pressed || Input.GetButtonDown("Jump"))
        {
            if (currentJumpCount < maxJumpCount - 1)
            {
                Jump();
            }
        }
    }

    // TODO: finish SwordAttack
    void SwordAttack()
    {
        swordAttackBtn.pressed = false;

        // PlayAudio(attackAudio);
        attackAudio.Play();
        anim.SetTrigger("SwordAttack");

        nextAttackTime = GetNextTime(1f / attackRate);
    }

    // TODO: finish MagicAttack
    void MagicAttack()
    {
        //实现点一次按一下（可能实现方法不好。。。）
        magicAttackBtn.pressed = false;

        anim.SetTrigger("MagicAttack");

        nextAttackTime = GetNextTime(1f / attackRate);
    }

    // TODO: Fix roll distance
    public void Roll()
    {
        currentRollCount++;

        //实现点一次按一下（可能实现方法不好。。。）
        rollBtn.pressed = false;

        PlayAudio(rollAudio);
        anim.SetTrigger("Roll");
        CreateDust();

        nextRollTime = GetNextTime(rollCd);
    }

    // TODO: Fix doubleJump
    void Jump()
    {
        currentJumpCount++;

        jumpBtn.pressed = false;
        PlayAudio(jumpAudio);

        anim.SetTrigger("Jump");
        CreateDust();
    }

    bool IsGrounded()
    {
        Vector2 startPos = transform.position;
        startPos += Vector2.down * (distToGround + 0.1f);
        RaycastHit2D hitData = Physics2D.Raycast(startPos, Vector3.back * (-1), 200, 1<<8);
        if (hitData.collider != null)
        {
            anim.SetBool("Grounded", true);
            return true;
        }
        else
        {
            anim.SetBool("Grounded", false);
            return false;
        }
    }

    /// <summary>
    /// 会根据贴图方向通过传入的水平移动方向参数进行翻转
    /// </summary>
    /// <param name="right">正在向x轴正方向移动</param>
    void Flip(bool right)
    {
        if (facing == Facing.Left)
        {
            right = !right;
        }
        
        float next = right ? 0 : 180;
        if (transform.rotation.eulerAngles.y != next)
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

    void PlayAudio(AudioSource t)
    {
        if (t != null && !t.isPlaying)
        {
            t.Play();
        }
    }

    void StopAudio(AudioSource t)
    {
        if (t != null && t.isPlaying)
        {
            t.Stop();
        }
    }
}