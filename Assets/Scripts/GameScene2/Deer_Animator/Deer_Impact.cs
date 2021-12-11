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
       deer = GameObject.Find("Deer").GetComponent<Enemy>();
       rb = deer.rb;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //判断贴图方向
        if ((deer.girlHero.transform.position.x - deer.transform.position.x) > 0)
        {
            deer.Flip(true);
        }
        else
        {
            deer.Flip(false);
        }

        Vector2 target = new Vector2(deer.girlHero.transform.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
