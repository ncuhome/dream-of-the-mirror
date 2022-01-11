using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Boss_Magic : StateMachineBehaviour
{
    public Enemy boss;

    [Header("Reference: ")]
    public Bo boPrefab;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.gameObject.GetComponent<Enemy>();
        
        Instantiate(boPrefab, boss.transform.position + boss.transform.right * (int)(boPrefab.facing), boss.transform.rotation);
    }
}
