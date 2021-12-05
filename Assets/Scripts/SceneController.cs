using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance; //单例
    public int level = 1; //现在是解谜的第几关（用于读取）
    public Map map1; 
    public Map map2;
    public Hero hero1; //当人物生成后，人物脚本会赋值给SceneController
    public Hero hero2;
    public Vector2 mapFinish1 = Vector2.zero;
    public Vector2 mapFinish2 = Vector2.zero;

    [Header("用于测试：")]
    public Vector3 scaling; //地图缩放比例（包括Hero）

    void Awake() 
    {
        if(this != null) 
            instance = this;    
    }

    void Update() //若两个Hero同时到达终点，加载下一个场景
    {
        if(hero1 == null || hero2 == null)
            return;
        if(hero1.HeroEnd && hero2.HeroEnd)
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex+1));
        
    }

    IEnumerator LoadScene(int index) //读取下一个场景，并在下一个场景结束后调用OnLoadScene
    {
        yield return new WaitForSeconds(0.25f);
        // yield return new WaitForSeconds(2f);

        AsyncOperation async = SceneManager.LoadSceneAsync(index);
        async.completed += OnLoadScene;
    }

    private void OnLoadScene(AsyncOperation obj)
    {

    }
}
