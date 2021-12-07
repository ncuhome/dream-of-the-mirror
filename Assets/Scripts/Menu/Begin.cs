using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//暂时的垃圾加载下个场景的按钮脚本
public class Begin : MonoBehaviour
{
    public Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(LoadNextScene);
    }

    void LoadNextScene()
    {
        int nextIndex = int.Parse(button.name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + nextIndex);
    }
}
