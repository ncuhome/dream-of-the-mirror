using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxEffect;

    private float length, startpos, ypos;
    private Transform cam;
    private Vector3 previousCamPos;

    void Awake()
    {
        // set up camera the reference
        cam = Camera.main.transform;
    }

    void Start()
    {
        startpos = transform.position.x;
        ypos = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);
        float ydist = (cam.transform.position.y * parallaxEffect);

        transform.position = new Vector3(startpos + dist, ypos, transform.position.z);
        // if (temp > startpos + length)
        // {
        //     startpos += length;
        // }
        // else if (temp < startpos - length)
        // {
        //     startpos -= length;
        // }
    }
}