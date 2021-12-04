using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DicButton : EventTrigger
{
    public Button dicButton;

    void Awake() 
    {
        dicButton = GetComponent<Button>();    
    }

    void Start() 
    {
        dicButton.onClick.AddListener(CallHero);    
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        print("fxd");
        base.OnPointerDown(eventData);
        CallHero();
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        print("djj");
        base.OnPointerUp(eventData);
        SceneController.instance.hero1.dirHeld = -1; 
        SceneController.instance.hero2.dirHeld = -1;
    }

    void CallHero()
    {
        if(SceneController.instance.hero1 == null || SceneController.instance.hero2 == null)
            return;
        string name = gameObject.name;
        switch(name)
        {
            case "RightButton":
                SceneController.instance.hero1.dirHeld = 0; 
                SceneController.instance.hero2.dirHeld = 0;
                break;
            case "UpButton":
                SceneController.instance.hero1.dirHeld = 1; 
                SceneController.instance.hero2.dirHeld = 1;
                break;
            case "LeftButton":
                SceneController.instance.hero1.dirHeld = 2; 
                SceneController.instance.hero2.dirHeld = 2;
                break;
            case "DownButton":
                SceneController.instance.hero1.dirHeld = 3; 
                SceneController.instance.hero2.dirHeld = 3;
                break;

        }

    }
}
