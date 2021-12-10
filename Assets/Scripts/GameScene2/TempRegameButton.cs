using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TempRegameButton : MonoBehaviour
{
    public Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ReGame);    
    }

    public void ReGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
