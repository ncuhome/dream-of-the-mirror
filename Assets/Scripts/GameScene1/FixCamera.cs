using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class FixCamera : MonoBehaviour
 {
     // change these numbers to your preferred apect ratio (16:9 in this case)
     const int resolutionX = 16;
     const int resolutionY = 9;
 
     void Start()
     {
         float screenRatio = Screen.width*1f / Screen.height;
         float bestRatio = resolutionX*1f / resolutionY;
         if (screenRatio <= bestRatio)
         {
             GetComponent<Camera>().rect = new Rect(0,(1f- screenRatio / bestRatio)/2f, 1, screenRatio / bestRatio);
         }else if(screenRatio > bestRatio)
         {
             GetComponent<Camera>().rect = new Rect((1f- bestRatio / screenRatio) /2f, 0, bestRatio / screenRatio, 1);
         }
     }
 }
