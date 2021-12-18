using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //单例
    public static SceneController instance;
    //现在是解谜的第几关（用于读取）
    public int level = 1;
    //当人物生成后，人物脚本会赋值给SceneController
    public LeftHero leftHero;
    public RightHero rightHero;
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
        if (leftHero == null || rightHero == null || inputManager == null)
        {
            return;
        }

        if (leftHero.ended && rightHero.ended)
        {
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        }
            
        Direction dir = inputManager.GetDirection();
        leftHero.dir = dir;
        rightHero.dir = dir;  
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
