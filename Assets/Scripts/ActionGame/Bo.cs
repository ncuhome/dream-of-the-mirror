using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bo : MonoBehaviour
{
    public float speed;
    public float liveTime = 2f;
    
    private float liveDone;
    private Vector2 tPos;

    void Start()
    {
        liveDone = Time.time + liveTime;    
    }
    
    void FixedUpdate()
    {
        tPos = transform.position;
        tPos.x = tPos.x + transform.right.x * speed * Time.fixedDeltaTime;
        transform.position = tPos;

        if (Time.time > liveDone)
        {
            Destroy(this.gameObject);
        }
    }
}
