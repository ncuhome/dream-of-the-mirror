using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Boss_Magic : StateMachineBehaviour
{
    public Enemy boss;

    [Header("Reference: ")]
    public Bo boPrefab;


    //解决是否旋转，便于使用transform.right
    int isRotate = 1;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.gameObject.GetComponent<Enemy>();

        float yR = boss.transform.rotation.y;
        if (yR < 5f)
        {
            isRotate = 1;
        }
        else if (yR > 175f)
        {
            isRotate = 0;
        }

        float boRotation = ((int)boPrefab.facing * (int)boss.facing * isRotate == 1) ? 0 : 180;
        Instantiate(boPrefab, boss.transform.position + boss.transform.right * isRotate * (int)boss.facing, Quaternion.Euler(0, boRotation, 0));
    }
}
