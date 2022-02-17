using UnityEngine;

public class Boss_Walk : StateMachineBehaviour
{
    public BossEnemy boss;
    public float speed;
    public Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.gameObject.GetComponent<BossEnemy>();
        rb = boss.rb;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss.enemyAttackConsciousness.attackConsciousness 
            && boss.enemyAttackConsciousness.heroDistance > boss.attackRange && !boss.health.isRepelled)
        {
            Vector2 target = new Vector2(boss.girlHero.transform.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
    }
}
