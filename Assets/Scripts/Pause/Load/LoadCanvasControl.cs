using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LoadCanvasControl : MonoBehaviour
{
    public TMP_Text hint;
    public TMP_Text loading;
    public Image img;

    private GameObject volumeObj;
    private Animator fade;
    private AnimationClip[] clips;
    private DepthOfField depthOfField;
    private ChromaticAberration chromaticAberration;

    void Start()
    {
        volumeObj = GlobalVolumeManager.instance.volumeObj;
        var volume = volumeObj.GetComponent<Volume>();
        volume.profile.TryGet<DepthOfField>(out depthOfField);
        volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
        fade = GetComponent<Animator>();
        clips = fade.runtimeAnimatorController.animationClips;  
        
    }

    public void FadeOut(float time)
    {
        depthOfField.focusDistance.Override(10f);
        chromaticAberration.intensity.value = 0;
        fade.SetTrigger("FadeOut");
    }

    public void FadeIn(float time)
    {
        fade.SetTrigger("FadeIn");
        // StartCoroutine(Restore(GetClipTime("FadeIn")));
    }

    public void LoadFont(float per)
    {
        loading.color = new Color(loading.color.r, loading.color.g, loading.color.b, per);
    }

    public void FontFlash()
    {
        fade.SetTrigger("Flash");
    }

    // IEnumerator DecreaseDepth(float time)
    // {
    //     yield return new WaitForSeconds(time);
    //     depthOfField.focusDistance.Override(1.35f);
    // }

    // private float GetClipTime(string name_)
    // {
    //     foreach(AnimationClip clip in clips)
    //     {
    //         if(clip.name.Equals(name_))
    //         {
    //             return clip.length;
    //         }
    //     }
    //     Debug.Log("Don't have " + name_);
    //     return 0;
    // }
}
