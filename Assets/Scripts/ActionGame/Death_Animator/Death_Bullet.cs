using UnityEngine;

public class Death_Bullet : StateMachineBehaviour
{
    public Enemy death;

    [Header("Reference: ")]
    public Bullet bullet;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        death = animator.gameObject.GetComponent<Enemy>();

        Quaternion bulletRotation = death.transform.rotation;
        bulletRotation.y = (death.transform.rotation.y == 0) ? 180 : 0;
        Instantiate(bullet, death.transform.position + death.transform.right * (-1), bulletRotation);
    }
}
