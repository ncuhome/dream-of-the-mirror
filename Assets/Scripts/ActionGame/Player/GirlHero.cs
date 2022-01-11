using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlHero : MonoBehaviour
{
    // For GroundSensor
    public Rigidbody2D rb;
    public int currentJumpCount = 0;
    public bool grounded = false;

    public CapsuleCollider2D capsuleCollider;
    public Animator anim;
    public Health playHealth;

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
    // 每秒攻击次数
    public float attackRate;
    // 跳跃升力
    public float jumpForce;

    public float rollCd;
    public float jumpCd;

    [Header("贴图默认朝向")]
    public Facing facing;

    [Header("粒子")]
    public GameObject dustEffect;

    [Header("魔法冲击波")]
    public GameObject boPrefab;

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

    private bool curAnimIs(string animName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }

    // 获取组件
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playHealth = GetComponent<Health>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        anim = transform.Find("HeroModel").GetComponent<Animator>();
        
        GameObject directionJoyStick = GameObject.FindGameObjectWithTag("DirectionJoyStick");
        inputController = directionJoyStick.GetComponent<MobileHorizontalInputController>();

        var dustPos = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        var dustPrefab = Instantiate(dustEffect, dustPos, Quaternion.identity);
        dustPrefab.transform.SetParent(transform);
        dust = dustPrefab.GetComponent<ParticleSystem>();
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
            || curAnimIs("GirlHero_Sword")
            || curAnimIs("GirlHero_Magic")
            || curAnimIs("GirlHero_Roll"))
        {
            anim.SetBool("Running", false);
            StopAudio(runAudio);
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

            //不能使用MovePosition，否则将停止使用重力下落
            // Vector2 newPos = Vector2.MoveTowards(rb.position, rb.position + (Vector2)transform.right, moveSpeed * Time.fixedDeltaTime);
            // rb.MovePosition(newPos);
            Vector2 tPos = transform.position;
            tPos.x += moveX * moveSpeed * Time.fixedDeltaTime;
            transform.position = tPos;
        }
    }

    void Update()
    {
        if (PauseControl.gameIsPaused) return;

        //使人物可以识别持续碰撞
        rb.WakeUp();
        
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

        if (jumpBtn.pressed || Input.GetButtonDown("Jump"))
        {
            if (currentJumpCount < maxJumpCount)
            {
                jumpBtn.pressed = false;
                // 跳跃行为
                Jump();
            }
        }

        if (grounded)
        {
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
    }

    // TODO: finish SwordAttack
    void SwordAttack()
    {
        PlayAudio(attackAudio);

        anim.SetTrigger("SwordAttack");
    }

    // TODO: finish MagicAttack
    void MagicAttack()
    {
        StartCoroutine(SpawnBo());
        anim.SetTrigger("MagicAttack");
    }

    // TODO: Fix roll distance
    void Roll()
    {
        PlayAudio(rollAudio);

        anim.SetTrigger("Roll");
        CreateDust();
    }

    // TODO: Fix doubleJump
    void Jump()
    {
        PlayAudio(jumpAudio);

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        currentJumpCount++;

        anim.SetTrigger("Jump");
        CreateDust();
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

    IEnumerator SpawnBo()
    {
        // yield return new WaitForSeconds(1.0f / attackRate - 0.1f);
        yield return new WaitForSeconds(1.0f / 2);
        Instantiate(boPrefab, transform.position + transform.right + transform.up * 0.25f, transform.rotation);
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