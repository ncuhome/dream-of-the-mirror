using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class AttackShake : MonoBehaviour
{
    private static AttackShake instance;
    public static AttackShake Instance
    {
        get
        {
            if (instance == null)
                instance = Transform.FindObjectOfType<AttackShake>();
            return instance;
        }
    }
 
    public void HitPause(float duration)
    {
        StartCoroutine(Pause(duration));
    }
    IEnumerator Pause(float duration)
    {
        Time.timeScale = 0;//暂时暂停时间
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }
    private bool isShake;
    public void CameraShake(float duration,float strength)
    {
        if (!isShake) StartCoroutine(Shake(duration, strength));
    }
    IEnumerator Shake(float duration,float strength)
    {
        isShake = true;
        Transform camera = Camera.main.transform;
        Vector3 startPos = camera.position;
        while (duration > 0)
        {
            camera.position = Random.insideUnitSphere * strength+startPos;
            duration -= Time.deltaTime;
            yield return null;
        }
        isShake = false;
    }
}