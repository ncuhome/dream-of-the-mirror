using UnityEngine;

public class DeathEnemy : Enemy
{
    [Header("Death: ")]
    //Death最少攻击思考时间
    public float timeThinkMin = 4f;
    //Death最大攻击思考时间
    public float timeThinkMax = 6f;
    public float attackRange = 1.5f;
    public float attackCd = 0.5f;
    public int attackDamage = 1;
    public float timeNextDecision = 0;

    //Death需要血量，判定什么时候能进入第二形态
    public Health _health;
    //附在敌人上的敌人血量划动条脚本
    public EnemySlider enemySlider;

    private float nextAttackTime;
    //远程两种方式
    private string[] remoteAttack = new string[]{
        "Bullet", "Teleport"
    };

    protected override void Start()
    {
        base.Start();
        _health = GetComponent<Health>();
        enemySlider = GetComponent<EnemySlider>();
    }

    protected override void Update()
    {
        base.Update();

        if (!enemyAttackConsciousness.attackConsciousness)
        {
            return;
        }
        enemySlider.FixSlider();

        if (_health.currentHealth <= _health.maxHealth/2)
        {
            timeThinkMin /= 2;
            timeThinkMax /= 2;
        }
        if (!curAnimIs("Death_Teleport"))
        {
            //判断贴图方向
            Flip((girlHero.transform.position.x - transform.position.x) > 0);
        }

        //特殊动作判定
        if (Time.time > timeNextDecision)
        {
            anim.SetTrigger(remoteAttack[Random.Range(0, remoteAttack.Length)]);

            //进行这一次攻击，同时为下一次攻击做准备
            float thinkTime = Random.Range(timeThinkMin, timeThinkMax);
            timeNextDecision = Time.time + thinkTime;
        }

        //攻击判定
        if (!curAnimIs("Death_Magic") && Time.time >= nextAttackTime)
        {
            if (enemyAttackConsciousness.heroDistance < attackRange)
            {
                anim.SetTrigger("Attack");
                nextAttackTime += attackCd;
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other) 
    {

    }

    protected override void OnTriggerStay2D(Collider2D other)
    {

    }
}
