using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    private SpriteRenderer rend;
    public Sprite cursor;

    public GameObject clickEffect;
    public GameObject trailEffect;
    public float timeBtwSpawn = 0.1f;

    void Start()
    {
        Cursor.visible = false;
        rend.GetComponent<SpriteRenderer>();
        rend.sprite = cursor;
    }

    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            Instantiate(clickEffect, transform.position, Quaternion.identity);
        }

        if (timeBtwSpawn <= 0)
        {
            Instantiate(trailEffect, transform.position, Quaternion.identity);
            timeBtwSpawn = 0.1f;
        }
        else
        {
            timeBtwSpawn -= Time.deltaTime;
        }
    }
}
