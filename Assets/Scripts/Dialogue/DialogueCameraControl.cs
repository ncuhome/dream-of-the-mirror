using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCameraControl : MonoBehaviour
{
    public Vector2 borderX;
    public Transform target;

    private float camWidth;
    private float camHeight;

    private void Start()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    void Update()
    {
        Vector3 tPos = new Vector3(Mathf.Clamp(target.transform.position.x, borderX.x + camWidth, borderX.y - camWidth), transform.position.y, transform.position.z);
        transform.position = tPos;
    }
}
