using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
