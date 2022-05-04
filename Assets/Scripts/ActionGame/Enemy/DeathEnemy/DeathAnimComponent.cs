using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimComponent : MonoBehaviour
{
    private Animator animator_;
    private AnimationClip[] clips;
    public SpriteRenderer sRend;

    public Animator Animator_
    {
        get
        {
            return animator_;
        }
    }

    public SpriteRenderer SRend
    {
        get
        {
            return sRend;
        }
    }

    void Awake()
    {
        sRend = GetComponent<SpriteRenderer>();
        animator_ = GetComponent<Animator>();
        clips = animator_.runtimeAnimatorController.animationClips;    
    }

    public float GetClipTime(string name_)
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

    public void SetColor(Color color)
    {
        sRend.color = color;
    }
}
