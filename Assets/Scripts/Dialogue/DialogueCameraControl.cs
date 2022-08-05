using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCameraControl : MonoBehaviour
{
    public Vector2 borderX;
    public Transform target;

    void Update()
    {
        Vector3 tPos = new Vector3(Mathf.Clamp(target.transform.position.x, borderX.x, borderX.y), transform.position.y, transform.position.z);
        transform.position = tPos;
    }
}
