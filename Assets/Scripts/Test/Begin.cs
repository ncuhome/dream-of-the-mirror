using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//暂时的垃圾加载下个场景的按钮脚本
public class Begin : MonoBehaviour
{
    public Button button;

    void Start() 
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(NextScene);    
    }

    void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
