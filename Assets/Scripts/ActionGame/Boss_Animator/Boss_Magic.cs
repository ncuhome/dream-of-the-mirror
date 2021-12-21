using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Magic : StateMachineBehaviour
{
    public Enemy boss;

    [Header("需要填充: ")]
    public GameObject boPrefab;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = GameObject.Find("Boss").GetComponent<Enemy>();
        
        if (boss.transform.rotation.y == 0)
            Instantiate(boPrefab, boss.transform.position + boss.transform.right * (-1), Quaternion.Euler(0, 180, 0));
        else
            Instantiate(boPrefab, boss.transform.position + boss.transform.right * (-1), Quaternion.Euler(0, 0, 0));
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
