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
            Vector2 target = new Vector2(death.girlHero.transform.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
    }
}
