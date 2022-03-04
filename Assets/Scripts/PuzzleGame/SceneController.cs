using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    //现在是解谜的第几关（用于读取）
    public int level = 1;
    //当人物生成后，人物脚本会赋值给SceneController与HeroMoveController
    public Hero leftHero;
    public Hero rightHero;

    public GameObject volumeObj;
    public float step = 0.1f;
    ChromaticAberration chromaticAberration;
    DepthOfField depthOfField;

    private bool isToNextScene = false;

    void Awake()
    {
        if (this != null)
            instance = this;
    }

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

    //若两个Hero同时到达终点，加载下一个场景
    void Update()
    {
        //因为人物是子对象，加载较慢
        if (leftHero == null || rightHero == null)
        {
            return;
        }

        if (leftHero.ended && rightHero.ended)
        {
            if (!isToNextScene)
            {
                if (chromaticAberration != null)
                {
                    chromaticAberration.intensity.Override(0f);
                }
                if (depthOfField != null)
                {
                    depthOfField.focusDistance.Override(10f);
                }
                StartCoroutine(ToActionScene(SceneManager.GetActiveScene().buildIndex + 1));
                isToNextScene = true;
            }
        }
    }

    IEnumerator ToActionScene(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            step = operation.progress;
            chromaticAberration.intensity.value = step;
            if (depthOfField != null && depthOfField.focusDistance.value > 0)
            {
                depthOfField.focusDistance.value = 10 - step * 10f;
            }

            if (operation.progress >= 0.9f)
            {
                if (Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }

    private void OnLoadScene(AsyncOperation obj)
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
    }
}
