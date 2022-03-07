using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class ExitControl : MonoBehaviour
{
    public GameObject volumeObj;

    public float step = 0.1f;

    ChromaticAberration chromaticAberration;

    DepthOfField depthOfField;

    void Start()
    {
        if (volumeObj != null)
        {
            var volume = volumeObj.GetComponent<Volume>();

            if (volume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
            {
            }
            else
            {
                volume.profile.Add<ChromaticAberration>(true);
                volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
            }

            if (volume.profile.TryGet<DepthOfField>(out depthOfField))
            {
            }
            else
            {
                volume.profile.Add<DepthOfField>(true);
                volume.profile.TryGet<DepthOfField>(out depthOfField);
            }
        }
    }


    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (chromaticAberration != null)
            {
                chromaticAberration.intensity.Override(0f);
            }
            if (depthOfField != null)
            {
                depthOfField.focusDistance.Override(10f);
            }
            StartCoroutine(ToStartScene());
        }
    }

    void GoBack()
    {
        var curIndex = SceneManager.GetActiveScene().buildIndex;
        if (curIndex == 0)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator ToStartScene()
    {
        if (chromaticAberration == null && depthOfField == null)
        {
            GoBack();
            yield return null;
        }
        while (chromaticAberration.intensity.value < 1)
        {
            chromaticAberration.intensity.value += step;
            if (depthOfField != null && depthOfField.focusDistance.value > 0)
            {
                depthOfField.focusDistance.value -= step * 10f;
            }
            yield return new WaitForSeconds(step);
        }
        GoBack();
    }
}
