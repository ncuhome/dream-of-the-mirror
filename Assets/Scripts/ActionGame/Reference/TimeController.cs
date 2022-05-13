using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private float speed;
    private bool isRestored = false;

    void Update()
    {
        if (isRestored)
        {
            if (Time.timeScale < 1f)
            {
                Time.timeScale += Time.deltaTime * speed;
            }
            else
            {
                Time.timeScale = 1f;
                isRestored = false;
            }
        }    
    }

    public void StopTime(float changeTimeScale, int restoreSpeed, float delay)
    {
        speed = restoreSpeed;

        if (delay > 0)
        {
            StopCoroutine(StartTimeAgain(delay));
            StartCoroutine(StartTimeAgain(delay));
        }       
        else
        {
            isRestored = true;
        } 

        Time.timeScale = changeTimeScale;
    }

    private IEnumerator StartTimeAgain(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        isRestored = true;
    }
}
