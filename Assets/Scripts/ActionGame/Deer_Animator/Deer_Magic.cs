using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deer_Magic : StateMachineBehaviour
{
    public Enemy deer;

    [Header("需要填充: ")]
    public GameObject boPrefab;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        deer = animator.gameObject.GetComponent<Enemy>();
        Instantiate(boPrefab, deer.transform.position + deer.transform.right, deer.transform.rotation);
    }
}
