using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoader : MonoBehaviour
{
    public Button button;
    public GameObject volumeObj;

    public int sceneIndex;

    public float step;

    ChromaticAberration chromaticAberration;

    void Start()
    {
        Volume volume = volumeObj.GetComponent<Volume>();

        if (volume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
        {
            button.onClick.AddListener(() => StartCoroutine("LoadNextScene"));
        }
    }

    IEnumerator LoadNextScene()
    {
        while (chromaticAberration.intensity.value < 1)
        {
            chromaticAberration.intensity.value += step;
            yield return new WaitForSeconds(step);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + sceneIndex);
    }

}
