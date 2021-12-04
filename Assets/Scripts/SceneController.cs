using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance; 
    public int level = 1;
    public Map map1;
    public Map map2;
    public Hero hero1; //当人物生成后，人物脚本会赋值给SceneController
    public Hero hero2;
    public Vector2 mapFinish1 = Vector2.zero;
    public Vector2 mapFinish2 = Vector2.zero;

    void Awake() 
    {
        if(this != null) 
            instance = this;    
    }

    void Update() 
    {
        if(hero1 == null || hero2 == null)
            return;
        if(hero1.HeroEnd && hero2.HeroEnd)
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex+1));
        
    }

    IEnumerator LoadScene(int index)
    {
        yield return new WaitForSeconds(2f);

        AsyncOperation async = SceneManager.LoadSceneAsync(index);
        async.completed += OnLoadScene;
    }

    private void OnLoadScene(AsyncOperation obj)
    {

    }
}
