// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Rendering;
// using UnityEngine.Rendering.Universal;
// using UnityEngine.SceneManagement;

// public class Navigate : MonoBehaviour
// {
//     public GameObject volumeObj;

//     public float step = 0.1f;

//     public int index = 0;

//     ChromaticAberration chromaticAberration;

//     DepthOfField depthOfField;

//     void Start()
//     {
//         if (volumeObj != null)
//         {
//             var volume = volumeObj.GetComponent<Volume>();

//             if (volume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
//             {
//             }
//             else
//             {
//                 volume.profile.Add<ChromaticAberration>(true);
//                 volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
//             }

//             if (volume.profile.TryGet<DepthOfField>(out depthOfField))
//             {
//             }
//             else
//             {
//                 volume.profile.Add<DepthOfField>(true);
//                 volume.profile.TryGet<DepthOfField>(out depthOfField);
//             }
//         }
//     }
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             StartCoroutine(TransToScene());
//         }
//     }


//     void GoTo()
//     {
//         SceneManager.LoadScene(index);
//     }

//     IEnumerator TransToScene()
//     {
//         if (chromaticAberration == null && depthOfField == null)
//         {
//             GoTo();
//             yield return null;
//         }
//         while (chromaticAberration.intensity.value < 1)
//         {
//             chromaticAberration.intensity.value += step;
//             if (depthOfField != null && depthOfField.focusDistance.value > 0)
//             {
//                 depthOfField.focusDistance.value -= step * 10f;
//             }
//             yield return new WaitForSeconds(step);
//         }
//         GoTo();
//     }
// }
