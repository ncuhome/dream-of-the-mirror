using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlHero_Magic : StateMachineBehaviour
{
    public GirlHero girlHero;

    [Header("魔法冲击波")]
    public Bullet bullet;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        girlHero = animator.gameObject.GetComponentInParent<GirlHero>();
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Instantiate(bullet, girlHero.transform.position + girlHero.transform.right + girlHero.transform.up * 0.25f, girlHero.transform.rotation);
    }
}
