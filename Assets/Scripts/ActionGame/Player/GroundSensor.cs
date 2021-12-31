using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    public GirlHero root;

    //从父对象获取接口
    void Start()
    {
        root = this.transform.root.GetComponent<GirlHero>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            if (root.rb.velocity.y <= 0)
            {
                root.grounded = true;
                root.anim.SetBool("Grounded", true);
                root.currentJumpCount = 0;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        root.grounded = false;
        root.anim.SetBool("Grounded", false);
    }
}