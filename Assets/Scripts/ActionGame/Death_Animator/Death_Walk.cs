using UnityEngine;

public class Death_Walk : StateMachineBehaviour
{
    public DeathEnemy death;
    public float speed;
    public Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        death = animator.gameObject.GetComponent<DeathEnemy>();
        rb = death.rb;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (death.enemyAttackConsciousness.attackConsciousness 
            && death.enemyAttackConsciousness.heroDistance > death.attackRange && !death.health.isRepelled)
        {
            death.Flip((death.girlHero.transform.position.x - death.transform.position.x) < 0);
            rb.velocity = new Vector2((-1) * death.transform.right.x * speed, rb.velocity.y);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
}
