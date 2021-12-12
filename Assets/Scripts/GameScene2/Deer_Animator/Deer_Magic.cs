using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deer_Magic : StateMachineBehaviour
{
    public Enemy deer;

    [Header("需要填充: ")]
    public GameObject boPrefab;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        deer = GameObject.Find("Deer").GetComponent<Enemy>();
        Instantiate(boPrefab, deer.transform.position + deer.transform.right, deer.transform.rotation);
    }
}
