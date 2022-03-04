using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PausePanelCamera : MonoBehaviour
{
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var pauseCamera = gameObject.GetComponent<Camera>();
        Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(pauseCamera);
    }
}
