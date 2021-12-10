using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bo : MonoBehaviour
{
    public float speed;

    private Vector2 tPos;
    
    void FixedUpdate()
    {
        tPos = transform.position;
        tPos.x = tPos.x + transform.right.x * speed * Time.fixedDeltaTime;
        transform.position = tPos;
    }
}
