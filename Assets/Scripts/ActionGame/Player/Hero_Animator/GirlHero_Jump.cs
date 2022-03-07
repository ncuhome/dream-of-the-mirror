using UnityEngine;

public class GirlHero_Jump : StateMachineBehaviour
{
    public GirlHero girlHero;
    public Rigidbody2D rb;
    public ButtonClickController jumpBtn;
    // 小跳跃升力
    public float shortJumpForce;
    // 大跳跃升力
    public float longJumpForce;

    public bool isJump = false;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        girlHero = animator.gameObject.GetComponentInParent<GirlHero>();

        rb = girlHero.rb;
        jumpBtn = girlHero.jumpBtn;

        rb.velocity = Vector2.zero;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isJump)
        {
            if (!jumpBtn.isStart)
            {
                rb.AddForce(Vector2.up * shortJumpForce, ForceMode2D.Impulse);
                isJump = true;
            }
            else if (Time.time - jumpBtn.lastTime > jumpBtn.thinkTime)
            {
                jumpBtn.isStart = false;
                rb.AddForce(Vector2.up * longJumpForce, ForceMode2D.Impulse);
                isJump = true;
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isJump = false;
    }
}
