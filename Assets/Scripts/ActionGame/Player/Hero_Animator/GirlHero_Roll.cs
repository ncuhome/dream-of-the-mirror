using UnityEngine;

public class GirlHero_Roll : StateMachineBehaviour
{
    public GirlHero girlHero;
    public Health _health;
    public Rigidbody2D rb;
    public float rollSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        girlHero = animator.gameObject.GetComponentInParent<GirlHero>();

        rb = girlHero.rb;
        rb.velocity = Vector2.zero;   

        _health = girlHero.playerHealth;
        _health.invincible = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 newPos = Vector2.MoveTowards(girlHero.rb.position, girlHero.rb.position + (Vector2)girlHero.transform.right, rollSpeed * Time.fixedDeltaTime);
        girlHero.rb.MovePosition(newPos);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = Vector2.zero;
        _health.invincible = false;
    }
}
