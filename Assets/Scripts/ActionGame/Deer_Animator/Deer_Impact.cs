using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deer_Impact : StateMachineBehaviour
{
    public Enemy deer;
    public float speed;
    public Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        deer = animator.gameObject.GetComponent<Enemy>();
        rb = deer.rb;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //判断贴图方向
        deer.Flip((deer.girlHero.transform.position.x - deer.transform.position.x) > 0);

        if (deer.enemyAttackConsciousness.attackConsciousness && !deer.health.isRepelled)
        {
            rb.velocity = new Vector2(deer.transform.right.x * speed, rb.velocity.y);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
}
