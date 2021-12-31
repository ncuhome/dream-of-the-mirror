using UnityEngine;

public class Deer_Jump : StateMachineBehaviour
{
    public Enemy deer;
    public float speed;
    public float posY;
    public float timeDone;
    public Rigidbody2D rb;
    //三点正弦插值
    public Vector2[] points;

    [Header("注；必须在动画前跳完")]
    public float jumpTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        deer = animator.gameObject.GetComponent<Enemy>();
        rb = deer.rb;
        points = new Vector2[3];

        Vector2 targetPos;
        targetPos.x = deer.girlHero.transform.position.x;
        targetPos.y = rb.position.y;
        points[0] = deer.transform.position;
        points[2] = targetPos;
        points[1].x = points[0].x + (points[2].x - points[0].x)/2;
        points[1].y = points[1].y + posY;

        timeDone = Time.time + jumpTime;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float u = (timeDone - Time.time) / jumpTime;
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
}
