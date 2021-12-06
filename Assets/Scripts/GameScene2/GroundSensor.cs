using UnityEngine;

public class GroundSensor : MonoBehaviour 
{
    //地面触发器接口
    public IGroundSensor m_root;

    //从父对象获取接口
    void Start()
    {
        m_root = this.transform.root.GetComponent<IGroundSensor>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Block"))
        {
            if (other.CompareTag("Ground"))
            {
                m_root.Is_DownJump_GroundCheck = true;
            }
            else
            {
                m_root.Is_DownJump_GroundCheck = false;
            }

            if (m_root.M_rigidbody.velocity.y <= 0)
            {
                m_root.IsGrounded = true;
                m_root.CurrentJumpCount = 0;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        m_root.IsGrounded = false;
    }
}