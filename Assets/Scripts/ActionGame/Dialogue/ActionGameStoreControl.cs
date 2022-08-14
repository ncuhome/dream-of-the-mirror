using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGameStoreControl : MonoBehaviour
{
    public GameObject devil;
    public GameObject devilTrigger;
    public GameObject death;
    public GameObject deathTrigger;
    public Animator store1;
    public Animator store2;
    public Animator store3;
    public Animator store5;
    public Vector2 girlHero1;
    public Vector2 girlHero2;
    public Vector2 girlHero3;
    public Vector2 girlHero5;

    private void Start()
    {
        switch(StaticData.storePoint)
        {
            case 0:
                break;
            case 1:
                PlayerManager.instance.girlHero.transform.position = girlHero1;
                break;
            case 2:
                PlayerManager.instance.girlHero.transform.position = girlHero2;
                break;
            case 3:
                PlayerManager.instance.girlHero.transform.position = girlHero3;
                Destroy(devil.gameObject);
                Destroy(devilTrigger.gameObject);
                break;
            case 5:
                PlayerManager.instance.girlHero.transform.position = girlHero5;
                Destroy(devil.gameObject);
                Destroy(devilTrigger.gameObject);
                break;
        }    
    }

    public void BeginStore(string _name)
    {
        switch(_name)
        {
            case "store1":
                store1.SetTrigger("Flash");
                TextHint.instance.ShowHint("已存档", Color.blue);
                StaticData.storePoint = 1;
                PlayerPrefs.SetInt("StorePoint", 1);
                break;
            case "store2":
                store2.SetTrigger("Flash");
                TextHint.instance.ShowHint("已存档", Color.blue);
                StaticData.storePoint = 2;
                PlayerPrefs.SetInt("StorePoint", 2);
                break;
            case "store3":
                store3.SetTrigger("Flash");
                TextHint.instance.ShowHint("已存档", Color.blue);
                StaticData.storePoint = 3;
                PlayerPrefs.SetInt("StorePoint", 3);
                break;
            case "store5":
                store5.SetTrigger("Flash");
                TextHint.instance.ShowHint("已存档", Color.blue);
                StaticData.storePoint = 5;
                PlayerPrefs.SetInt("StorePoint", 5);
                break;
        }
    }
}
