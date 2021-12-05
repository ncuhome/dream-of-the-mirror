// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;

// public class DicButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
// {
//     bool ispressed = false;

//     void Update()
//     {
//         if(SceneController.instance.hero1 == null || SceneController.instance.hero2 == null)
//             return;
//         if (!ispressed)
//         {
//             return;
//         }
//         CallHero();
//     }

//     public void OnPointerDown(PointerEventData eventData)
//     {
//         ispressed = true;
//     }
 
//     public void OnPointerUp(PointerEventData eventData)
//     {
//         ispressed = false;
//     }

//     void CallHero()
//     {
//         string name = gameObject.name;
//         switch(name)
//         {
//             case "LeftButton":
//                 SceneController.instance.hero1.dirHeld = 2;                 
//                 SceneController.instance.hero2.dirHeld = 2;
//                 break;
//             case "RightButton":
//                 SceneController.instance.hero1.dirHeld = 0; 
//                 SceneController.instance.hero2.dirHeld = 0;
//                 break;
//             case "UpButton":
//                 SceneController.instance.hero1.dirHeld = 1; 
//                 SceneController.instance.hero2.dirHeld = 1;
//                 break;
//             case "DownButton":
//                 SceneController.instance.hero1.dirHeld = 3; 
//                 SceneController.instance.hero2.dirHeld = 3;
//                 break;
//         }
//     }
// }
