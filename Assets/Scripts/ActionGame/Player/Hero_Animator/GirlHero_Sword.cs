using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlHero_Sword : StateMachineBehaviour
{
    public GirlHero girlHero;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        girlHero = animator.gameObject.GetComponentInParent<GirlHero>();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(girlHero.readyToRoll == true)
        {
            girlHero.Roll();
            girlHero.readyToRoll = false;
        }
    }
}
