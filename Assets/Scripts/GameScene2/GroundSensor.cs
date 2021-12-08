using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    //地面触发器接口
    public GirlHero root;

    //从父对象获取接口
    void Start()
    {
        root = this.transform.root.GetComponent<GirlHero>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Block"))
        {
            if (other.CompareTag("Ground"))
            {
                root.onGroundedTag = true;
            }
            else
            {
                root.onGroundedTag = false;
            }

            if (root.rb.velocity.y <= 0)
            {
                root.grounded = true;
                root.currentJumpCount = 0;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        root.grounded = false;
    }
}