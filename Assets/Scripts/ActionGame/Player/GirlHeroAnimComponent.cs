using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlHeroAnimComponent : MonoBehaviour
{
    public Animator animator_;
    public SpriteRenderer sRend;

    private AnimationClip[] clips;
    private GirlHeroPhysicsComponent physics;

    public Animator Animator_
    {
        get
        {
            return animator_;
        }
    }

    void Awake()
    {
        clips = animator_.runtimeAnimatorController.animationClips;  
        physics = GetComponent<GirlHeroPhysicsComponent>(); 
    }

    public AnimationClip GetClip(string name_)
    {
        foreach(AnimationClip clip in clips)
        {
            if(clip.name.Equals(name_))
            {
                return clip;
            }
        }
        Debug.Log("Don't have " + name_);
        return null;
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
