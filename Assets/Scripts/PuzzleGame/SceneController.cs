using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    //现在是解谜的第几关（用于读取）
    public int level = 1;
    //当人物生成后，人物脚本会赋值给SceneController与HeroMoveController
    public Hero leftHero;
    public Hero rightHero;

    void Awake()
    {
        if (this != null)
            instance = this;
    }

    //若两个Hero同时到达终点，加载下一个场景
    void Update()
    {
        //因为人物是子对象，加载较慢
        if (leftHero == null || rightHero == null)
        {
            return;
        }

        if (leftHero.ended && rightHero.ended)
        {
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    //读取下一个场景，并在下一个场景结束后调用OnLoadScene
    IEnumerator LoadScene(int index)
    {
        yield return new WaitForSeconds(0.25f);

        AsyncOperation async = SceneManager.LoadSceneAsync(index);
        async.completed += OnLoadScene;
    }

    private void OnLoadScene(AsyncOperation obj)
    {

    }
}
