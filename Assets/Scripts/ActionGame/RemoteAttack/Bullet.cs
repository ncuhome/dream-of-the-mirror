using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float liveTime = 2f;

    //贴图朝向
    public Facing facing;
    
    private float liveDone;
    private Vector2 tPos;

    void Start()
    {
        liveDone = Time.time + liveTime;    
    }
    
    void FixedUpdate()
    {
        tPos = transform.position;
        tPos.x = tPos.x + transform.right.x * (int)facing * speed * Time.fixedDeltaTime;
        transform.position = tPos;

        if (Time.time > liveDone)
        {
            Destroy(this.gameObject);
        }
    }
}
