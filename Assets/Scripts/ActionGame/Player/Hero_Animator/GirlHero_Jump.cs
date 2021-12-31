using UnityEngine;

public class GirlHero_Jump : StateMachineBehaviour
{
    public GirlHero girlHero;
    // 跳跃升力
    public float jumpForce;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        girlHero = GameObject.FindGameObjectWithTag("Player").GetComponent<GirlHero>();
    }
}
