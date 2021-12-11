using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deer_Jump : StateMachineBehaviour
{
    public DeerEnemy deer;
    public float speed;
    public float posY;
    public float timeDuration;
    public float timeDone;
    public Rigidbody2D rb;
    //三点正弦插值
    public Vector2[] points;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        deer = GameObject.Find("Deer").GetComponent<DeerEnemy>();
        rb = deer.rb;
        points = new Vector2[3];

        Vector2 targetPos;
        targetPos.x = deer.girlHero.transform.position.x;
        targetPos.y = rb.position.y;
        points[0] = deer.transform.position;
        points[2] = targetPos;
        points[1].x = points[0].x + (points[2].x - points[0].x)/2;
        points[1].y = points[1].y + posY;

        timeDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        timeDone = Time.time + timeDuration;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float u = (timeDone - Time.time) / timeDuration;
        if (u > 0)
        {
            //插值
            Vector3 p01, p12;
            u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
            p01 = u * points[0] + (1 - u) * points[1];
            p12 = u * points[1] + (1 - u) * points[2];
            rb.position = u * p01 + (1 - u) * p12;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
