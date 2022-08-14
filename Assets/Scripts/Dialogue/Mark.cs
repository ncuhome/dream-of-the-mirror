using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    public Animator mark; 
    private AnimationClip[] clips;
    public Transform markT;
    public float moveSpeed = 1f;

    void Start()
    {
        clips = mark.runtimeAnimatorController.animationClips;     
    }

    public void ConnectPlayer(bool isConnect)
    {
        if (isConnect)
        {
            mark.SetTrigger("pop");
            StartCoroutine(MarkRise(true));
        }
        else
        {

            mark.SetTrigger("push");
            StartCoroutine(MarkRise(false));
        }
    }

    IEnumerator MarkRise(bool t)
    {
        if (t)
        {
            float time = GetClipTime("pop");
            Vector2 v = markT.position;
            while (time > 0)
            {
                time -= Time.deltaTime;
                v.y += moveSpeed * Time.deltaTime;
                mark.transform.position = v;
                yield return null;
            }
        }
        else
        {
            float time = GetClipTime("push");
            Vector2 v = markT.position;
            while (time > 0)
            {
                time -= Time.deltaTime;
                v.y -= moveSpeed * Time.deltaTime;
                mark.transform.position = v;
                yield return null;
            }
        }
    }

    float GetClipTime(string name_)
    {
        foreach(AnimationClip clip in clips)
        {
            if(clip.name.Equals(name_))
            {
                return clip.length;
            }
        }
        Debug.Log("Don't have " + name_);
        return 0;
    }
}
