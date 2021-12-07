using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //单例
    public static SceneController instance;
    //现在是解谜的第几关（用于读取）
    public int level = 1;
    public Map map1;
    public Map map2;
    //当人物生成后，人物脚本会赋值给SceneController
    public Hero hero1;
    public Hero hero2;
    public Vector2 mapFinish1 = Vector2.zero;
    public Vector2 mapFinish2 = Vector2.zero;

    [Header("用于测试：")]
    //地图缩放比例（包括Hero）
    public Vector3 scaling;

    private InputManager inputManager;

    void Awake()
    {
        if (this != null)
            instance = this;
        inputManager = gameObject.AddComponent<InputManager>();
    }

    //若两个Hero同时到达终点，加载下一个场景
    void Update()
    {
        if (hero1 == null || hero2 == null || inputManager == null)
            return;
        Direction dir = inputManager.GetDirection();
        hero1.dir = dir;
        hero2.dir = dir;
        if (hero1.HeroEnd && hero2.HeroEnd)
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    //读取下一个场景，并在下一个场景结束后调用OnLoadScene
    IEnumerator LoadScene(int index)
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
